using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AAC.Models
{
    [AddINotifyPropertyChangedInterface]
    public class RunnerAttendancesModel
    {
        const double DaysPerMonth = 30.5;
        const double DaysPerYear = 356;

        public string Name { get; set; }
        public ObservableCollection<DateTime> Attendaces { get; set; }
        public RunnerAttendancesModel()
        {

        }
        #region Properties
        [DependsOn(nameof(Attendaces))]
        public DateTime FirstAttend
        {
            get
            {
                Attendaces.OrderBy(dt => dt);
                return Attendaces[0];
            }
        }
        [DependsOn(nameof(Attendaces))]
        public double AttenAvgMonth
        {
            get
            {
                TimeSpan period = DateTime.Now - FirstAttend;
                return Attendaces.Count / (period.TotalDays / DaysPerMonth);
            }
        }
        [DependsOn(nameof(Attendaces))]
        public int AttenLastMonth
        {
            get
            {
                Attendaces.OrderBy(dt => dt);
                int res = 0;
                DateTime MonthAgo = DateTime.Now - TimeSpan.FromDays(DaysPerMonth);
                for (int i = 0; i < Attendaces.Count; ++i)
                {
                    if (Attendaces[i] >= MonthAgo) res++;
                    else break;
                }
                return res;
            }
        }
        [DependsOn(nameof(Attendaces))]
        public double AttenAvgYear
        {
            get
            {
                TimeSpan period = DateTime.Now - FirstAttend;
                return Attendaces.Count / (period.TotalDays / DaysPerYear);
            }
        }
        [DependsOn(nameof(Attendaces))]
        public int AttenLastYear
        {
            get
            {
                Attendaces.OrderBy(dt => dt);
                int res = 0;
                DateTime YearAgo = DateTime.Now - TimeSpan.FromDays(DaysPerYear);
                for (int i = 0; i < Attendaces.Count; ++i)
                    {
                    if (Attendaces[i] >= YearAgo) res++;
                    else break;
                }
                return res;
            }
        }
        #endregion
    }
}
