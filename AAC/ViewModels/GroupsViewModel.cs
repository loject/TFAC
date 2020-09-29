using AAC.Databases;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    public class Runner /* TODO(loject): useless? */
    {
        public Group Group { get; set; }/* for deleting */
        public string Name { get; set; }
        public Runner()
        {
            DeleteRunnerCommand = new Command<string>(RN => Group.Remove(this));
        }
        public ICommand DeleteRunnerCommand { get; private set; }
    }
    public class Group : ObservableCollection<Runner>
    {
        public string Name { get; set; }
        public Group() : base()
        {
            AddNewRunnerCommand = new Command(AddRunner);
        }
        public Group(string name, ObservableCollection<Runner> runners) : base(runners)
        {
            Name = name;
            AddNewRunnerCommand = new Command(AddRunner);
        }

        #region Commands
        public ICommand AddNewRunnerCommand { get; private set; }
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
                Items.Add(new Runner { Name = RunnerName, Group = this});
                try
                {
                    App.GroupsDatabase.SaveGroupsNote(new Databases.GroupNote { Name = Name, RunnerName = RunnerName }).Wait();
                }
                catch(AggregateException e)
                {
                    foreach (var Error in e.InnerExceptions)
                        Console.WriteLine(Error.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public async void DeleteRunner(object RaName)
        {
            string RName = "alksdjf";
            for (int i = 0; i < Items.Count; ++i)
            {
                /* remove from list */
                var index = -1;
                for (int j = 0; j < Items.Count; ++j)
                {
                    if (Items[j].Name == RName)
                    {
                        index = j;
                        break;
                    }
                }
                if (index > -1) Items.Remove(Items[i]);
                else await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Ошибка", "Произошла неизвестная внутренняя ошибка", "Ок");
                /* remove grom storage */
            }
            try
            {
                App.GroupsDatabase.RemoveGroupNote(new GroupNote { Name = Name, RunnerName = RName}).Wait();
            }
            catch (AggregateException e)
            {
                foreach (var Error in e.InnerExceptions)
                    Console.WriteLine(Error.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            /* read data from storage */
            List<GroupNote> DBGroups;
            try
            {
                var tmp = App.GroupsDatabase.GetGroupList();
                tmp.Wait();
                DBGroups = tmp.Result;
                foreach (var Note in DBGroups)
                {
                    var index = -1;
                    for (int i = 0; i < Groups.Count; ++i)
                    {
                        if (Groups[i].Name == Note.Name)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index > -1)
                    {
                        Groups[index].Add(new Runner { Name = Note.RunnerName, Group = Groups[index] });
                    }
                    else
                    {
                        Groups.Add(new Group { Name = Note.Name });
                        Groups[Groups.Count - 1].Add(new Runner { Name = Note.RunnerName, Group = Groups[Groups.Count - 1] });
                    }
                }
            }
            catch (AggregateException e)
            {
                foreach (var Error in e.InnerExceptions)
                    Console.WriteLine(Error.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /* commands */
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
        #region Commands
        public ICommand CreateNewGroup { get; private set; }
        #endregion
    }
}
