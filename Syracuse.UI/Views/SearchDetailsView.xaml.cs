﻿using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms.Xaml;
using Syracuse.Mobitheque.Core.Models;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Syracuse.Mobitheque.Core;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchDetailsView : MvxContentPage<SearchDetailsViewModel>
    {
        public SearchDetailsView()
        {
            InitializeComponent();
        }
        private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            var model = e.CurrentItem as Result;
            Console.WriteLine("CurrentItem's Name:" + model.FieldList.CroppedTitle);
        }

        private void carouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            Console.WriteLine("CurrentItem's CurrentPosition:" + e.CurrentPosition);
            this.ViewModel.IsPositionVisible = true;
        }
        private async void OnCarouselViewRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            Console.WriteLine("OnCarouselViewRemainingItemsThresholdReached:");
            if (!this.ViewModel.InLoadMore)
            {
                await this.ViewModel.LoadMore();
            }
            
        }
        private void OnScrollEvent(object sender, ScrolledEventArgs e) {

            Console.WriteLine($"ScrollX: {e.ScrollX}, ScrollY: {e.ScrollY}");
            if (e.ScrollY == 0)
            {
                Console.WriteLine($"IsSwipeEnable: IsSwipeEnable, ScrollY: {e.ScrollY}");
                this.ViewModel.IsSwipeEnable = true;
                this.ViewModel.RaiseAllPropertiesChanged();
            }
            else
            {
                this.ViewModel.IsSwipeEnable = false;
                this.ViewModel.RaiseAllPropertiesChanged();
            }
        }

        private async void OnSwipeEvent(object sender, SwipeStartedEventArgs e)
        {
            Console.WriteLine($"Swipe Direction: {e.SwipeDirection}");
            if (this.ViewModel.IsSwipeEnable)
            {
                await this.Navigation.PopAsync();
            }

        }


        private async void OnSwipeEvent(object sender, SwipeChangingEventArgs e)
        {
            Console.WriteLine($"Swipe Direction: {e.SwipeDirection}");
            if (this.ViewModel.IsSwipeEnable)
            {
               await this.Navigation.PopAsync();
            }

        }

        private async void OnSwipeEvent(object sender, SwipeEndedEventArgs e)
        {
            Console.WriteLine($"Swipe Direction: {e.SwipeDirection}");
            if (this.ViewModel.IsSwipeEnable)
            {
                await this.Navigation.PopAsync();
            }

        }

        private async void HoldingButton_Clicked(object sender, EventArgs e)
        {
            Holdings data = ((Button)sender).BindingContext as Holdings;
            bool answer = await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.HoldingChoice, data.Site), ApplicationResource.Yes, ApplicationResource.No);
            if (answer)
            {
                await this.ViewModel.Holding(data.Holdingid, data.RecordId, data.BaseName);
            }
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as SearchDetailsViewModel).OnDisplayAlert += SearchDetailsView_OnDisplayAlert;
            (this.DataContext as SearchDetailsViewModel).OnDisplayAlertMult += SearchDetailsView_OnDisplayAlertMult;
            base.OnBindingContextChanged();
        }
        private void SearchDetailsView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
        private Task<bool> SearchDetailsView_OnDisplayAlertMult(string title, string message, string buttonYes, string buttonNo)
        {
            var res = this.DisplayAlert(title, message, buttonYes, buttonNo);
            return res;
        }
    }
}