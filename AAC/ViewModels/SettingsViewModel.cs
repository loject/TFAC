using AAC.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

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
