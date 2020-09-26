using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    public class Runner
    {
        public string Name { get; set; }
        public Runner()
        {

        }
    }
    public class Group : ObservableCollection<Runner>
    {
        public string Name { get; set; }
        public Group() : base()
        {
            AddNewRunner = new Command(AddRunner);
        }
        public Group(string name, ObservableCollection<Runner> runners) : base(runners)
        {
            Name = name;
            AddNewRunner = new Command(AddRunner);
        }

        #region Commands
        public ICommand AddNewRunner { get; private set; }
        #endregion
        #region Functions
        private async void AddRunner()
        {
            var RunnerName = await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayPromptAsync("Имя спортсмена", "");
            if (string.IsNullOrWhiteSpace(RunnerName))
            {
                await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Ошибка", "Имя не может быть пустым", "Ок");
            }
            else
            {
                Items.Add(new Runner { Name = RunnerName });
            }
        }
        #endregion
    }
    [AddINotifyPropertyChangedInterface]
    public class GroupsViewModel 
    {
        public ObservableCollection<Group> Groups { get; set; }
        public GroupsViewModel()
        {
            Groups = new ObservableCollection<Group>();
            CreateNewGroup = new Command(async () =>
            {
                var GroupName = await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayPromptAsync("Имя группы", "");
                if (string.IsNullOrWhiteSpace(GroupName))
                {
                    await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Ошибка", "Имя группы не может быть пустым", "Ок");
                }
                else
                {
                    Groups.Add(new Group(GroupName, new ObservableCollection<Runner>() ));
                }
            });
        }

        public ICommand CreateNewGroup { get; private set; }
    }
}
