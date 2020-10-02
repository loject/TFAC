using AAC.Databases;
using AAC.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AttendaceViewModel
    {
        public ObservableCollection<Group> RunnersGroups { get; set; }
        public DateTime AttendDate { get; set; } = DateTime.Now;
        public TimeSpan AttendTime { get; set; } = DateTime.Now.TimeOfDay;
        public AttendaceViewModel()
        {
            UpdateFromLocalStorage();

            MarkAttend = new Command<string>(RunnerName =>
            {
                var attendDateTime = AttendDate.Date + AttendTime;
                AddAttend(RunnerName, attendDateTime);
                var attendanceNote = new AttendanceNote { Name = RunnerName, AttendanceDateTime = attendDateTime };
                App.AttendanceDatabase.SaveAttendanceNote(attendanceNote);
            });
        }
        #region Commands
        public ICommand MarkAttend { get; set; }
        #endregion
        #region Functions
        private void AddAttend(string Name, DateTime dt)
        {
            var GroupIndex = -1;
            var RunnerIndex = -1;
            for (int i = 0;i < RunnersGroups.Count && GroupIndex < 0; ++i)
            {
                for (int j = 0; j < RunnersGroups[i].Count && RunnerIndex < 0; ++j)
                { 
                    if (RunnersGroups[i][j].Name == Name)
                    {
                        GroupIndex = i;
                        RunnerIndex = j;
                    }
                }
            }
            if (GroupIndex >= 0 && RunnerIndex >= 0)
            {
                RunnersGroups[GroupIndex][RunnerIndex].Add(dt);
                App.AttendanceDatabase.SaveAttendanceNote(new AttendanceNote { Name = Name, AttendanceDateTime = dt });
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Ошибка", "Произошла внутреняя ошибка", "ОК");
            }
        }
        public void UpdateFromLocalStorage()
        {
            try
            {
                if (RunnersGroups == null) RunnersGroups = new ObservableCollection<Group>();
                var GetAttendanceListTask = App.AttendanceDatabase.GetAttendanceList();
                GetAttendanceListTask.Wait();
                List<AttendanceNote> AttendanceNotes = GetAttendanceListTask.Result;
                List<Runner> RunnersList = new List<Runner>();
                foreach(var note in AttendanceNotes)
                {
                    var index = RunnersList.IndexOf(r => r.Name == note.Name);
                    if (index > -1)
                    {
                        RunnersList[index].Add(note.AttendanceDateTime);
                    }
                    else
                    {
                        RunnersList.Add(new Runner { Name = note.Name });
                        RunnersList[^1].Add(note.AttendanceDateTime);
                    }
                }
                var GetGroupListTask = App.GroupsDatabase.GetGroupList();
                GetGroupListTask.Wait();
                List<GroupNote> GroupsNotes = GetGroupListTask.Result;
                foreach (var note in GroupsNotes)
                {
                    var RunnerIndex = RunnersList.IndexOf(r => r.Name == note.RunnerName);
                    Runner runner = (RunnerIndex < 0) ? new Runner { Name = note.RunnerName } : RunnersList[RunnerIndex];
                    var GroupIndex = RunnersGroups.IndexOf(g => g.Name == note.Name);
                    if (GroupIndex > -1)
                    {
                        RunnersGroups[GroupIndex].Add(runner);
                    }
                    else
                    {
                        var group = new Group { Name = note.Name };
                        group.Add(runner);
                        RunnersGroups.Add(group);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                App.Current.MainPage.DisplayAlert("Ошибка", e.Message, "ОК");
            }
        }
        #endregion
    }
}
