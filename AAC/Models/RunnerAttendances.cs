using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AAC.Models
{
    [AddINotifyPropertyChangedInterface]
    public class RunnerAttendancesModel
    {
        public string Name { get; set; }
        public List<DateTime> Attendaces { get; set; }
        public RunnerAttendancesModel()
        {

        }
    }
}
