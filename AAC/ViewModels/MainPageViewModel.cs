using AAC.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    class MainPageViewModel
    {
        public ICommand GoToAttendanceCommand { get; private set; } = new Command(() =>
        {
            var AttendacePage = new AttendancePage();
            //var AttendaceVM = AttendanceViewModel();
            AttendacePage.BindingContext = null;/* TODO: fix this */
            App.Current.MainPage.Navigation.PushAsync(AttendacePage);
        });
        public ICommand GoToSettingsPage { get; private set; } = new Command(() =>
        {
            Console.WriteLine("Hello, settings page");
        });
    }
}
