using PropertyChanged;
using System.Collections.ObjectModel;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class GroupsViewModel
    {
        public ObservableCollection<string> RunnersNames { get; set; }
        public GroupsViewModel()
        {

        }
    }
}
