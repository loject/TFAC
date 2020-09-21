using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AAC.Models
{
    [AddINotifyPropertyChangedInterface]
    public class RunnerAttendancesModel
    {
        const double DaysPerMonth = 30.5;
        const double DaysPerYear = 356;

        public string Name { get; set; }
        public List<DateTime> Attendaces { get; set; }
        public RunnerAttendancesModel()
        {

        }
        #region Properties
        public DateTime FirstAttend
        {
            get
            {
                Attendaces.Sort();
                return Attendaces[0];
            }
        }
        public double AttenAvgMonth
        {
            get
            {
                TimeSpan period = DateTime.Now - FirstAttend;
                return period.TotalDays / DaysPerMonth / Attendaces.Count;
            }
        }
        public int AttenLastMonth
        {
            get
            {
                Attendaces.Sort();
                int res = 0;
                DateTime MonthAgo = DateTime.Now - TimeSpan.FromDays(DaysPerMonth);
                for (int i = Attendaces.Count - 1; i >= 0; --i)
                {
                    if (Attendaces[i] < MonthAgo) res++;
                    else break;
                }
                return res;
            }
        }
        public double AttenAvgYear
        {
            get
            {
                TimeSpan period = DateTime.Now - FirstAttend;
                return period.TotalDays / DaysPerYear / Attendaces.Count;
            }
        }
        public int AttenLastYear
        {
            get
            {
                Attendaces.Sort();
                int res = 0;
                DateTime YearAgo = DateTime.Now - TimeSpan.FromDays(DaysPerYear);
                for (int i = Attendaces.Count - 1; i >= 0; --i)
                {
                    if (Attendaces[i] < YearAgo) res++;
                    else break;
                }
                return res;
            }
        }
        #endregion
    }
}
