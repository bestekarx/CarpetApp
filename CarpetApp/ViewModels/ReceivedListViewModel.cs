using System.Collections.ObjectModel;
using System.Windows.Input;
using CarpetApp.Models.API.Response;
using CarpetApp.Services.API.Interfaces;
using CarpetApp.Models;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels
{
    public class ReceivedListViewModel : Base.ViewModelBase
    {
        private readonly IBaseApiService _apiService;
        public ObservableCollection<ReceivedListItemModel> ReceivedItems { get; set; } = new();
        public ObservableCollection<VehicleModel> VehicleList { get; set; } = new();
        public ObservableCollection<AreaModel> AreaList { get; set; } = new();
        private VehicleModel _selectedVehicle;
        
        public VehicleModel SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                SetProperty(ref _selectedVehicle, value);
                SelectedVehicleId = value.Id.ToString();
                LoadReceivedListAsync();
            }
        }
        private AreaModel _selectedArea;
        public AreaModel SelectedArea
        {
            get => _selectedArea;
            set
            {
                SetProperty(ref _selectedArea, value);
                SelectedAreaId = value.Id.ToString();
                LoadReceivedListAsync();
            }
        }
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                LoadReceivedListAsync();
            }
        }
        private DateTime? _selectedDate = DateTime.Today;
        public string SelectedVehicleId { get; set; }
        public string SelectedAreaId { get; set; }

        public ICommand RefreshCommand { get; }
        public IAsyncRelayCommand InitializeAsyncCommand { get; }
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ReceivedListViewModel(IBaseApiService apiService)
        {
            _apiService = apiService;
            RefreshCommand = new Command(async () => await LoadReceivedListAsync());
            InitializeAsyncCommand = new AsyncRelayCommand(InitializeAsync);
        }

        public async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await LoadVehicleListAsync();
                await LoadAreaListAsync();
                SelectedDate = DateTime.Today;
                await LoadReceivedListAsync();
            }
            catch (Exception ex)
            {
                // TODO: Hata loglama veya kullanıcıya gösterme
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task LoadVehicleListAsync()
        {
            var result = await _apiService.GetVehicleList(new Models.API.Filter.BaseFilterModel());
            VehicleList.Clear();
            if (result?.Items != null)
                foreach (var item in result.Items)
                    VehicleList.Add(item);
        }
        private async Task LoadAreaListAsync()
        {
            var result = await _apiService.GetAreaList(new Models.API.Filter.BaseFilterModel());
            AreaList.Clear();
            if (result?.Items != null)
                foreach (var item in result.Items)
                    AreaList.Add(item);
        }

        public async Task LoadReceivedListAsync()
        {
            var filter = new Models.API.Filter.ReceivedFilterParameters
            {
                Date = SelectedDate,
                VehicleId = SelectedVehicleId,
                AreaId = SelectedAreaId
            };
            var result = await _apiService.GetReceivedList(filter);
            ReceivedItems.Clear();
            if (result?.Items != null)
            {
                foreach (var item in result.Items)
                    ReceivedItems.Add(item);
            }
        }
    }
} 