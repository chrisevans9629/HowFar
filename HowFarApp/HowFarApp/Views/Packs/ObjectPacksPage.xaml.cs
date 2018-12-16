﻿using HowFar.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HowFarApp.Views.Packs;
using Unity;
using Unity.Resolution;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HowFarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectPacksPage : ContentPage
    {
        private readonly IMeasureConverters _converters;
        private readonly IUnityContainer _container;
        private ObjectPack _selectedObjectPack;
        private ObservableCollection<ObjectPack> _objects;

        public ObservableCollection<ObjectPack> Objects
        {
            get => _objects;
            set
            {
                _objects = value; 
                OnPropertyChanged();
            }
        }

        public ObjectPack SelectedObjectPack
        {
            get => _selectedObjectPack;
            set
            {
                _selectedObjectPack = value;
                OnPropertyChanged();

                if (_selectedObjectPack != null)
                {
                    Navigation.PushAsync(_container.Resolve<ObjectPackDetail>(new DependencyOverride(typeof(ObjectPack), SelectedObjectPack)));
                }
            }
        }

        public ObjectPacksPage(IMeasureConverters converters, IUnityContainer container)
        {
            _converters = converters;
            _container = container;
            InitializeComponent();
            BindingContext = this;

            this.Objects = new ObservableCollection<ObjectPack>(converters.ObjectPacks);

          //  this.ObjectPacksList.ItemsSource = this.Objects;
        }

        protected override void OnAppearing()
        {
            this.Objects = new ObservableCollection<ObjectPack>(_converters.ObjectPacks);

            base.OnAppearing();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Congrats!", "You now have a new package!", "Ok");
        }

        private async void ButtonNewObjectPack(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_container.Resolve<NewObjectPackPage>(), true);
        }
    }
}