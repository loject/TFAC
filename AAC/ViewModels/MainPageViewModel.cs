using AAC.Views;
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
        public ICommand GoToSettingsCommand { get; private set; } = new Command(() =>
        {
            var SettingsPage = new SettingsPage();
            var SettingsVM = new SettingsViewModel();
            SettingsPage.BindingContext = SettingsVM;
            App.Current.MainPage.Navigation.PushAsync(SettingsPage);
        });
    }
}
