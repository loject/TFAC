using AAC.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    public class MainPageViewModel
    {
        public ICommand GoToGroupEditingCommand { get; private set; } = new Command(() =>
        {
            var GroupPage = new GroupsPage();
            var GroupVM = new GroupsViewModel();
            GroupPage.BindingContext = GroupVM;
            App.Current.MainPage.Navigation.PushAsync(GroupPage);
        });
        public ICommand GoToAttendanceCommand { get; private set; } = new Command(() =>
        {
            var AttendacePage = new AttendancePage();
            var AttendaceVM = new AttendaceViewModel();
            AttendacePage.BindingContext = AttendaceVM;
            App.Current.MainPage.Navigation.PushAsync(AttendacePage);
        });
        public ICommand GoToSettingsPage { get; private set; } = new Command(() =>
        {
            Console.WriteLine("Hello, settings page");
        });
    }
}
