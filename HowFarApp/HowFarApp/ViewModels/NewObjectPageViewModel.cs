﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HowFar.Core.Models;
using HowFarApp.Models;
using HowFarApp.Views;
using HowFarApp.Views.Packs;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace HowFarApp.ViewModels
{
    public class NewObjectPageViewModel : ViewModelBase
    {
        private readonly IMeasureConverters measure;
        private readonly IPageDialogService _dialog;
        private readonly INavigationService _navigationService;
        private ObjectMeasurement _selectedObject;
        private ObservableCollection<ObjectMeasurement> _objectMeasurements;
        private ObjectPack _selectedObjectPack;
        private bool _canCreateNew;
        private ObservableCollection<ObjectPack> _objectPacks;
        private string _singleName;
        private double _measurementValue;
        private string _pluralName;

        public ObservableCollection<ObjectPack> ObjectPacks
        {
            get => _objectPacks;
            set => SetProperty(ref _objectPacks, value);
        }

        public string SingleName
        {
            get => _singleName;
            set => SetProperty(ref _singleName, value);
        }

        public double MeasurementValue
        {
            get => _measurementValue;
            set => SetProperty(ref _measurementValue, value);
        }

        public string PluralName
        {
            get => _pluralName;
            set => SetProperty(ref _pluralName, value);
        }

        public ObjectPack SelectedObjectPack
        {
            get => _selectedObjectPack;
            set => SetProperty(ref _selectedObjectPack, value);
        }

        public bool CanCreateNew
        {
            get => _canCreateNew;
            set => SetProperty(ref _canCreateNew, value);
        }

        public DelegateCommand NewCommand { get; }
        public NewObjectPageViewModel(IMeasureConverters measure, IPageDialogService dialog, INavigationService navigationService) : base(navigationService)
        {
            ObjectPacks = measure.ObjectPacks;
            ObjectMeasurements = measure.ObjectMeasurements;

            SelectedObjectPack = ObjectPacks.FirstOrDefault(p => p.Name == "Custom");
            this.measure = measure;
            _dialog = dialog;
            _navigationService = navigationService;
            NewCommand = new DelegateCommand(NewButton_OnClicked).ObservesCanExecute(() => CanCreateNew);
            NewObjectPackCommand = new DelegateCommand(ButtonNewObjectPack);
            //NewButton.IsEnabled = false;
        }

        public DelegateCommand NewObjectPackCommand { get; set; }
        public ObservableCollection<ObjectMeasurement> ObjectMeasurements
        {
            get => _objectMeasurements;
            set => SetProperty(ref _objectMeasurements, value);
        }

        public ObjectMeasurement SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                OnPropertyChanged();
                if (_selectedObject != null)
                {
                    CanCreateNew = true;
                }
            }
        }

        async Task<bool> NewObject()
        {

            var validator = new NewObjectValidator();
            var result = validator.Validate(this);
            if (result.IsValid)
            {
                var measurement = measure.NewObject(PluralName, SingleName, MeasurementValue, SelectedObject, SelectedObjectPack.Name);

                return true;
            }
            else
            {
                await Ext.DisplayError(result, _dialog);
                return false;
            }

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ObjectPacks = measure.ObjectPacks;
            ObjectMeasurements = measure.ObjectMeasurements;
            base.OnNavigatedTo(parameters);
        }

      

        private async void NewButton_OnClicked()
        {
            if (await NewObject())
            {
                //await Navigation.PopAsync(true);
                await _navigationService.GoBackAsync();
            }
        }

        private async void ButtonNewObjectPack()
        {
            await _navigationService.NavigateAsync(nameof(NewObjectPackPage));
        }
    }
}