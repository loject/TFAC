using AAC.Databases;
using AAC.ViewModels;
using AAC.Views;
using System;
using System.IO;
using Xamarin.Forms;

namespace AAC
{
    public partial class App : Application
    {
        static AttendanceDatabase attendanceDatabase;
        public static AttendanceDatabase AttendanceDatabase
        {
            get
            {
                if (attendanceDatabase == null)
                {
                    attendanceDatabase = new AttendanceDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TFAC_Attendances.db3"));
                }
                return attendanceDatabase;
            }
        }
        public App()
        {
            InitializeComponent();

            var mainPageViewModel = new MainPageViewModel();
            var mainPage = new MainPage();
            mainPage.BindingContext = mainPageViewModel;
            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
