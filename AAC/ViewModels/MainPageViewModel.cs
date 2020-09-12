using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    class MainPageViewModel
    {
        public ICommand GoToGoToattendanceCommandCommand { get; private set; } = new Command(() =>
        {
            Console.WriteLine("Hello, GoToattendanceCommand page");
        });
        public ICommand GoToSettingsPage { get; private set; } = new Command(() =>
        {
            Console.WriteLine("Hello, settings page");
        });
    }
}
