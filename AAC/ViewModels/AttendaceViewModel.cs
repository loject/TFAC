using AAC.Models;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AttendaceViewModel
    {
        public List<RunnerAttendancesModel> RunnerAttendance { get; set; } 
        public AttendaceViewModel()
        {
            var RAT = App.AttendanceDatabase.GetAttendanceList();
            RAT.Wait();
            var RA = RAT.Result;
            for (int i = 0; i < RA.Count; i++)
            {
                var index = RunnerAttendance.FindIndex(a => a.Name == RA[i].Name);
                if (index > 0)
                {
                    RunnerAttendance[index].Attendaces.Add(RA[i].AttendanceDateTime);
                }
                else
                {
                    RunnerAttendance.Add(new RunnerAttendancesModel { Name = RA[i].Name, Attendaces = { RA[i].AttendanceDateTime } });
                }
            }
        }
    }
}
