using AAC.Databases;
using AAC.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
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
                        Groups[^1].Add(new Runner { Name = Note.RunnerName, Group = Groups[^1] });
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
                if (string.IsNullOrWhiteSpace(GroupName)) await App.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Ошибка", "Имя группы не может быть пустым", "Ок");
                else Groups.Add(new Group(GroupName, new ObservableCollection<Runner>() ));
            });
        }
        #region Commands
        public ICommand CreateNewGroup { get; private set; }
        #endregion
    }
}
