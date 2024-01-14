using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace FotoKiosk
{
    public sealed partial class PhotoPage : UserControl
    {
        public PhotoPage()
        {
            this.InitializeComponent();
            loadPhotos();
            //set timer, so it refresh every 15 seconds
            timer();
        }

        //timer voor elke 15 seconden
        private async void timer()
        {
            while (true)
            {
                await Task.Delay(15000);
                loadPhotos();
            }
        }
        private async void loadPhotos()
        {
            //nieuwe img lijst maken, pad naar de foto's aangeven
            var afbeeldingen = new Dictionary<DateTime, Image>();
            var appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var fotosFolder = await appFolder.GetFolderAsync("Assets\\Fotos");

            //dagnummer opvrgagen
            var now = DateTime.Now;
            int day = (int)now.DayOfWeek;

            foreach (StorageFolder folder in await fotosFolder.GetFoldersAsync())
            {
                //mapnaam scheiden
                string folderName = folder.Name;
                string[] parts = folderName.Split('_');
                string eersteDeel = parts[0];

                //voegt afbeeldingen van dag toe aan list
                if (day.ToString() == eersteDeel)
                {
                    var files = await folder.GetFilesAsync();
                    foreach (var file in files)
                    {
                        //tijd pakken
                        int second = (int)now.Second;
                        int minute = (int)now.Minute;
                        int hour = (int)now.Hour;


                        //afbeelding naam scheiden in parts
                        string fileTime = file.Name;
                        string[] timeParts = fileTime.Split('_');
                        string hourPart = timeParts[0];
                        string minutePart = timeParts[1];
                        string secondPart = timeParts[2];

                        int hourPartInt = int.Parse(hourPart);
                        int minutePartInt = int.Parse(minutePart);
                        int secondsPartInt = int.Parse(secondPart);

                        // Create DateTime photoDate based on hourPart, minutePart and secondPart
                        DateTime photoDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourPartInt, minutePartInt, secondsPartInt);


                        // 30 mins geleden          photoDate               2 mins geleden
                        if (DateTime.Now - TimeSpan.FromMinutes(30) < photoDate && photoDate < DateTime.Now - TimeSpan.FromMinutes(2))
                        {
                            afbeeldingen.Add(photoDate, new Image() { Afbeelding = file.Path });
                        }
                    }
                }
            }

            //new list 
            var afbeeldingenSorted = new List<Image>();

            foreach ((DateTime time, Image image) in afbeeldingen)
            {
                if (!afbeeldingenSorted.Contains(image))
                {
                    afbeeldingenSorted.Add(image);
                    DateTime linkedImage = time.AddMinutes(1);
                    if (afbeeldingen.ContainsKey(linkedImage))
                    {
                        afbeeldingenSorted.Add(afbeeldingen[linkedImage]);
                    }
                }
            }
            //laat foto's op scherm zien
            FotoEl.ItemsSource = afbeeldingenSorted;
        }   
    }
}
