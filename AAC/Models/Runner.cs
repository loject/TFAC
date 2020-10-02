using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AAC.Models
{
    /* collection storing attendences */
    [AddINotifyPropertyChangedInterface]
    public class Runner : ObservableCollection<DateTime>
    {
        public Group Group { get; set; }/* for deleting */
        public string Name { get; set; }
        public Runner()
        {
            DeleteRunnerCommand = new Command<string>(RN => Group.DeleteRunner(RN));
        }
        public ICommand DeleteRunnerCommand { get; private set; }
    }
}
