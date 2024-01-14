using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FotoKiosk
{
    public sealed partial class ReportsPage : Page
    {

        private GlobaalReport globaalReport;

        public ReportsPage()
        {
            this.InitializeComponent();
        }


        private async void ReportButton(object sender, RoutedEventArgs e)
        {

            //maakt een filepicker
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".csv");

            var file = await picker.PickSingleFileAsync();

            //checkt of er een (csv) file gekozen is
            if (file == null)
            {
                tbFileStatus.Text = "Geen geldig betand gekozen. Kies een .csv bestand.";
                return;
            }


            //laat path van de geopende file zien
            tbFileStatus.Text = file.Path;

            using (var fileAccess = await file.OpenReadAsync())
            {
                using (var stream = fileAccess.AsStreamForRead())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            globaalReport = new GlobaalReport();

                            //mapnaam scheiden voor dagnaam in rapportage
                            string fileName = file.Name;
                            string[] parts = fileName.Split('_');
                            string tweedeDeel = parts[1];
                            string[] parts1  = tweedeDeel.Split('.');
                            string derdeDeel = parts1[0];
                            

                            //declareer inhoud globaalreport
                            var reports = csv.GetRecords<Report>();
                            foreach (Report report in reports)
                            {
                                globaalReport.Amount += report.Amount;
                                globaalReport.TotalPrice += report.TotalPrice;
                                globaalReport.OrderCount += 1;
                                globaalReport.Day = derdeDeel;
                            }
                            globaalReport.PurchaseRatio = globaalReport.OrderCount / globaalReport.Amount;
                            globaalReport.PurchaseRatio = globaalReport.PurchaseRatio * 100;
                            globaalReport.PurchaseRatio = Math.Round(globaalReport.PurchaseRatio, 2);
                            globaalReport.TotalPrice = Math.Round(globaalReport.TotalPrice, 2);

                            //stopt inhoud globaalreport in een lijst
                            List<GlobaalReport> reportList = new List<GlobaalReport>();
                            reportList.Add(globaalReport);
                            lvReport.ItemsSource = reportList;
                        }
                    }
                }
            }
        }

        private async void saveRapport_Click(object sender, RoutedEventArgs e)
        {
            //msg for save succesfull, not working while using it
            string msg = "Rapport opgeslagen";

            //picker voor opslaan file 
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "Rapport_Vandaag";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            //text die in de file moet staan, werkt alleen wnr er een file gekozen is
            if (file != null)
            {
                await FileIO.AppendTextAsync(file, "Dagrapportage " + globaalReport.Day + "\n");
                await FileIO.AppendTextAsync(file, "Opgeslagen op: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\n");
                await FileIO.AppendTextAsync(file, "Ratio order / bezoekers: " + globaalReport.PurchaseRatio + "\n");
                await FileIO.AppendTextAsync(file, "Aantal orders: " + globaalReport.Amount + "\n");
                await FileIO.AppendTextAsync(file, "Aantal producten verkocht: " + globaalReport.OrderCount + "\n");
                await FileIO.AppendTextAsync(file, "Totaal: " + globaalReport.TotalPrice + "\n");
            }
            //no one gets it working lol
            //saveMsg.Text = msg;
        }
    }
}