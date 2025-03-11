namespace CarpetApp.Repositories.Entry.EntryBase;

public abstract class EntryRepositoryException
    : Exception
{
    public EntryRepositoryException(string message) : base(message)
    {
    }
}

public class ContentTypeStringUnknownException : EntryRepositoryException
{
    public ContentTypeStringUnknownException(string contentTypeString) : base(
        $"Content Type string '{contentTypeString}' is unknown.")
    {
        ContentTypeString = contentTypeString;
    }

    public string ContentTypeString { get; set; }
}

public class EntryTypeUnknownException : EntryRepositoryException
{
    public EntryTypeUnknownException(Type type) : base($"Entity Type '{type.FullName}' is unknown.")
    {
        Type = type;
    }

    public Type Type { get; set; }
}