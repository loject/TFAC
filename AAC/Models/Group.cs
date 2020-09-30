using AAC.Databases;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.Models
{
    /* collection storing attendences */
    public class Runner : ObservableCollection<DateTime>
    {
        public Group Group { get; set; }/* for deleting */
        public string Name { get; set; }
        public Runner()
        {
            DeleteRunnerCommand = new Command<string>(RN => Group.DeleteRunner(RN));
        }
        public ICommand DeleteRunnerCommand { get; private set; }
    }
    /* collection stroring storing runners from group */
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
                Items.Add(new Runner { Name = RunnerName, Group = this });
                try
                {
                    App.GroupsDatabase.SaveGroupsNote(new Databases.GroupNote { Name = Name, RunnerName = RunnerName }).Wait();
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
        }
        public async void DeleteRunner(string RName)
        {
            try
            {
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
                    if (index > -1) Remove(Items[i]);
                    else await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Ошибка", "Произошла неизвестная внутренняя ошибка", "Ок");
                }
                /* remove from storage */
                App.GroupsDatabase.RemoveGroupNote(new GroupNote { Name = Name, RunnerName = RName }).Wait();
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
}
