using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    public class BackgroundManager : ViewModelBase
    {
        public static BackgroundTheme CurrentTheme { get; set; }

        private String backgroundImage;

        public BackgroundManager()
        {
            CurrentTheme = (BackgroundTheme)RandomProvider.Random.Next(0, 4);
            UpdateTheme();
        }

        public String BackgroundImage
        {
            get { return backgroundImage; }
            set
            {
                if (value.Equals(backgroundImage)) return;

                backgroundImage = value;
                OnPropertyChanged();
            }
        }

        public void UpdateTheme()
        {
            if (CurrentTheme == BackgroundTheme.Night)
            {
                CurrentTheme = BackgroundTheme.Morning;
            }
            else 
            {
                CurrentTheme++;
            }

            var theme = (BackgroundTheme)CurrentTheme;
            BackgroundImage = GetImagePath(theme);
        }

        private String GetImagePath(BackgroundTheme theme)
        {
            switch (theme)
            {
                case (BackgroundTheme.Morning): return @"Resources\morning.jpg";
                case (BackgroundTheme.Day): return @"Resources\day.jpg";
                case (BackgroundTheme.Evening): return @"Resources\evening.jpg";
                default: return @"Resources\night.jpg";
            }
        }
    }
}
