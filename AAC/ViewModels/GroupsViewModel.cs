using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    public class Group
    {
        public string Name { get; set; }
        public ObservableCollection<string> RunnersNames { get; set; }
        public Group()
        {

        }
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
                    Groups.Add(new Group { Name = GroupName, RunnersNames = new ObservableCollection<string>() });
                }
            });
        }

        public ICommand CreateNewGroup { get; private set; }
    }
}
