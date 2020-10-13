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
                AttendanceNote attendanceNote = new AttendanceNote { Name = RunnerName, AttendanceDateTime = attendDateTime };
                App.AttendanceDatabase.SaveAttendanceNote(attendanceNote);
                (MarkAttend as Command).ChangeCanExecute();
            }, RunnerName =>
            {
                if (RunnerName == null) return false;
                var indexes = GetIds(RunnerName);
                bool res = true;
                if (indexes.Item1 < 0 || indexes.Item2 < 0)
                {
                    App.Current.MainPage.DisplayAlert("Ошибка", "Произошла внутреняя ошибка", "ОК");
                }
                else
                {
                    /* TODO: Optimize this(binary search) */
                    var MinPeriod = Settings.MinPeriod;
                    var Attendance = RunnersGroups[indexes.Item1][indexes.Item2];
                    for (int i = 0; i < Attendance.Count && res; ++i)
                        if (Attendance[i].Date == AttendDate.Date && Math.Abs(Attendance[i].TimeOfDay.Ticks - AttendTime.Ticks) < MinPeriod.Ticks)
                            res = false;
                }
                return res;
            });
        }
        #region Commands
        public ICommand MarkAttend { get; set; }
        #endregion
        #region Functions
        private void AddAttend(string Name, DateTime dt)
        {
            var indexes = GetIds(Name);
            if (indexes.Item1 >= 0 && indexes.Item2 >= 0)
            {
                RunnersGroups[indexes.Item1][indexes.Item2].Add(dt);
                App.AttendanceDatabase.SaveAttendanceNote(new AttendanceNote { Name = Name, AttendanceDateTime = dt });
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Ошибка", "Произошла внутреняя ошибка", "ОК");
            }
        }
        /**
         * Find group id in RunnersGroup and runner id in group
         * @param  RunnerName runner name
         * @return Tuple with first group id and second id in group
         * @Note return -1 if not found
         * */
        private Tuple<int, int> GetIds(string RunnerName)
        {
            int GroupId = -1;
            int RunnerId = -1;
            for (int i = 0; i < RunnersGroups.Count && GroupId < 0; ++i)
            {
                for (int j = 0; j < RunnersGroups[i].Count && RunnerId < 0; ++j)
                {
                    if (RunnersGroups[i][j].Name == RunnerName)
                    {
                        GroupId = i;
                        RunnerId = j;
                        break;
                    }
                }
            }
            return new Tuple<int, int>(GroupId, RunnerId);
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
