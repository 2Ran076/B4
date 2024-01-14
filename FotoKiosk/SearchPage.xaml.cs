using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }
        private async void zoekFotos_Click(object sender, RoutedEventArgs e)
        {
            //nieuwe img lijst maken, pad naar de foto's aangeven
            var afbeeldingen = new List<Image>();
            var appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var fotosFolder = await appFolder.GetFolderAsync("Assets\\Fotos");

            //variabelen declareren met de naam van de combobox
            int day = (int)pickDay.SelectedIndex;
            int hour = pickHour.SelectedIndex + 9;
            int minute = pickMinute.SelectedIndex + 1;

            DateTime selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, 0);

            foreach (StorageFolder folder in await fotosFolder.GetFoldersAsync())
            {
                //mapnaam scheiden
                string folderName = folder.Name;
                string[] parts = folderName.Split('_');
                string eersteDeel = parts[0];

                var files = await folder.GetFilesAsync();
                foreach (var file in files)
                {
                    if (eersteDeel == "0")
                    {
                        eersteDeel = "7";
                    }                      
                    //naam afbeelding scheiden op uur, minuut, sec
                    string fileTime = file.Name;
                    string[] timeParts = fileTime.Split('_');
                    string hourPart = timeParts[0];
                    string minutePart = timeParts[1];
                    string secondPart = timeParts[2];

                    int dayPartInt = int.Parse(eersteDeel);
                    int hourPartInt = int.Parse(hourPart);
                    int minutePartInt = int.Parse(minutePart);

                    //variabelen in datetime stoppen
                    DateTime photoDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayPartInt, hourPartInt, minutePartInt, 0);

                    //als de photodate 10 minuten na/en voor tijd is, deze afbeeldingen in afbeelding stoppen
                    if (photoDate > selectedDate.AddMinutes(-10) && photoDate < selectedDate.AddMinutes(10))
                    {
                        afbeeldingen.Add(new Image() { Afbeelding = file.Path });
                    }
                }
            }
            Foto2El.ItemsSource = afbeeldingen;
        }
    }
}
