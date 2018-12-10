﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HowFar.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HowFarApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectPackDetail : ContentPage
    {
        public ObjectPack Pack { get; }
        public ObservableCollection<ObjectMeasurement> Items { get; set; }

        public ObjectPackDetail(ObjectPack pack)
        {
            Pack = pack;
            InitializeComponent();
            BindingContext = this;
            Items = new ObservableCollection<ObjectMeasurement>(pack.ObjectMeasurements);
			
			MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

          //  await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}