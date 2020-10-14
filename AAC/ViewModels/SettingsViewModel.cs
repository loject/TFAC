using AAC.Models;
using PropertyChanged;
using System;

namespace AAC.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class SettingsViewModel
    {
        public SettingsViewModel()
        { }
        #region properties
        public TimeSpan MinPeriod
        {
            get => Settings.MinPeriod;
            set => Settings.MinPeriod = value;
        }
        #endregion
    }
}
