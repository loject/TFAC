using System;
using Xamarin.Essentials;

namespace AAC.Models
{
    public class Settings
    {
        public static TimeSpan MinPeriod
        {
            get => Preferences.Get(nameof(MinPeriod), DateTime.MinValue + TimeSpan.FromHours(2)).TimeOfDay;
            set => Preferences.Set(nameof(MinPeriod), DateTime.MinValue + value);
        }
    }
}
