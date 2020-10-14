using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AAC.Models
{
    /* collection storing attendences */
    [AddINotifyPropertyChangedInterface]
    public class Runner : ObservableCollection<DateTime>
    {
        public Group Group { get; set; }/* for deleting */
        public string Name { get; set; }
        /* crutch for export page */
        public bool ForExport { get; set; }
        public Runner()
        {
            DeleteRunnerCommand = new Command<string>(RN => Group.DeleteRunner(RN));
            ForExport = false;
        }
        #region Properties
        public DateTime FirstAttend
        {
            get
            {
                var res = this?[0] ?? DateTime.Now;
                this.ForEach(t => { if (t < res) res = t; });
                return res;
            }
        }
        #endregion
        #region Commands
        public ICommand DeleteRunnerCommand { get; private set; }
        #endregion
        #region Functions
        public int AttendanceOn(DateTime date) => Items.Select(dt => dt.Date == date.Date).Count();
        public int AttendanceOn(DateTime start, DateTime end)
        {
            if (start > end) return 0;
            int res = 0;
            int i = 0;
            while (this[i] < start) i++;
            while (this[i] < end) { i++; res++; }
            return res;
        }
        #endregion
    }
}
