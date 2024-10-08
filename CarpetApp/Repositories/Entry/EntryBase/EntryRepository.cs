using System.Collections.Concurrent;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Repositories.Entry.EntryBase;

public class EntryRepository : IEntryRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Type> _entryTypes = new();
        private readonly Dictionary<Type, dynamic> _repos = new();
        private readonly Dictionary<Type, string> _type2str = new();
        private readonly Dictionary<string, Type> _str2type = new();

        public EntryRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RegisterEntries();
        }

        private void RegisterEntries()
        {
            //RegisterEntryType<ActivityV1>();
            //RegisterEntryType<DiaryV1>();
        }

        private void RegisterEntryType<TEntry>()
            where TEntry : Entities.Base.Entry
        {
            var type = typeof(TEntry);
            var contentTypeString = typeof(TEntry).Name;

            if (_entryTypes.Contains(type))
                throw new ArgumentException($"Duplicate type: {type} has been registered.");

            _entryTypes.Add(type);
            _type2str[type] = contentTypeString;
            _str2type[contentTypeString] = type;
            GetRepository(type);
        }

        public Type GetTypeFromContentTypeString(string contentTypeString)
        {
            if (!_str2type.ContainsKey(contentTypeString))
            {
                throw new ContentTypeStringUnknownException(contentTypeString);
            }

            return _str2type[contentTypeString];
        }

        public string GetContentTypeStringFromType(Type type)
        {
            if (!_type2str.ContainsKey(type))
            {
                throw new EntryTypeUnknownException(type);
            }

            return _type2str[type];
        }

        public dynamic GetRepository(Type type)
        {
            Guard.IsNotNull(type);

            if (_repos.TryGetValue(type, out dynamic? value))
            {
                if (value != null)
                    return value;
            }

            if (!_entryTypes.Contains(type))
            {
                ThrowHelper.ThrowArgumentException();
            }

            var repoType = typeof(IEntryRepository<>).MakeGenericType(type);
            dynamic repo = _serviceProvider.GetService(repoType)!;
            Guard.IsNotNull(repo);

            _repos[type] = repo;
            return repo;
        }

        private static async Task<Entities.Base.Entry> FindEntryInRepository(Guid uuid, dynamic repo)
        {
            return await repo.FindByUuidAsync(uuid).ConfigureAwait(false);
        }

        public async Task<Entities.Base.Entry?> FindEntryByUuidAsync(Guid uuid)
        {
            ConcurrentBag<Entities.Base.Entry> bag = new();

            await Parallel.ForEachAsync(_repos.Values, async (repo, _) =>
            {
                Entities.Base.Entry? entry = await repo.FindByUuidAsync(uuid);
                if (entry != null)
                {
                    bag.Add(entry);
                }
            });

            return bag.FirstOrDefault(defaultValue: null);
        }

        public async Task<List<Entities.Base.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids)
        {
            ConcurrentBag<Entities.Base.Entry> bag = new();

            await Parallel.ForEachAsync(uuids, async (uuid, _) =>
            {
                Entities.Base.Entry? entry = await FindEntryByUuidAsync(uuid);
                if (entry != null)
                {
                    bag.Add(entry);
                }
            });

            return bag.ToList();
        }

        /*
        public async Task<List<EntryMetadata>> GetAllMetadataAsync()
        {
            List<EntryMetadata> allMetadata = new();
            foreach (var type in _entryTypes)
            {
                dynamic repo = GetRepository(type);
                var metadata = (List<EntryMetadata>)await repo.GetAllMetadataAsync().ConfigureAwait(false);
                allMetadata.AddRange(metadata);
            }
            return allMetadata;
        } */

        public async Task SaveEntryAsync(Entities.Base.Entry entry)
        {
            dynamic repo = GetRepository(entry.GetType());
            dynamic runtime_entry = entry;
            await repo.SaveAsync(runtime_entry);
        }
    }