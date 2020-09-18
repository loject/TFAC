using AAC.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AttendaceViewModel
    {
        public ObservableCollection<RunnerAttendancesModel> RunnerAttendance { get; set; } 
        public AttendaceViewModel()
        {
            RunnerAttendance = new ObservableCollection<RunnerAttendancesModel>();
            var RAT = App.AttendanceDatabase.GetAttendanceList();
            RAT.Wait();
            var RA = RAT.Result;
            for (int i = 0; i < RA.Count; i++)
            {
                var index = -1;
                for (int j = 0; i < RunnerAttendance.Count; i++)
                {
                    if (RunnerAttendance[j].Name == RA[i].Name)
                    {
                        index = j;
                        break;
                    }
                }
                if (index > 0)
                {
                    RunnerAttendance[index].Attendaces.Add(RA[i].AttendanceDateTime);
                }
                else
                {
                    RunnerAttendance.Add(new RunnerAttendancesModel { Name = RA[i].Name, Attendaces = { RA[i].AttendanceDateTime } });
                }
            }
            /* TODO: for test */
            RunnerAttendance = new ObservableCollection<RunnerAttendancesModel>
            {
                new RunnerAttendancesModel { Name = "Runner 1", Attendaces = new List<DateTime>{ new DateTime(3845), new DateTime(38445), } },
                new RunnerAttendancesModel { Name = "Runner 2", Attendaces = new List<DateTime>{ new DateTime(213), new DateTime(345345345), } },
                new RunnerAttendancesModel { Name = "Runner 3", Attendaces = new List<DateTime>{ new DateTime(574563453), new DateTime(3453453), } },
            };
        }
    }
}
