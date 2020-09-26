using AAC.Databases;
using AAC.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AttendaceViewModel
    {
        public ObservableCollection<RunnerAttendancesModel> RunnerAttendance { get; set; }
        public DateTime AttendDate { get; set; } = DateTime.Now;
        public TimeSpan AttendTime { get; set; } = DateTime.Now.TimeOfDay;
        public AttendaceViewModel()
        {
            RunnerAttendance = new ObservableCollection<RunnerAttendancesModel>();
            var RAT = App.AttendanceDatabase.GetAttendanceList();
            RAT.Wait();
            RAT.Result.ForEach(r => AddAttend(r.Name, r.AttendanceDateTime));

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
            var index = -1;
            for (int j = 0; j < RunnerAttendance.Count; j++)
            {
                if (RunnerAttendance[j].Name == Name)
                {
                    index = j;
                    break;
                }
            }
            if (index > 0) RunnerAttendance[index].Attendaces.Add(dt);
            else RunnerAttendance.Add(new RunnerAttendancesModel { Name = Name, Attendaces = new ObservableCollection<DateTime>{ dt } });
        }
        #endregion
    }
}
