using AAC.ViewModels;
using AAC.Views;
using Xamarin.Forms;

namespace AAC
{
    public partial class App : Application
    {
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
