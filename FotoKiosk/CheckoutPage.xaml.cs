using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class CheckoutPage : UserControl
    {
        public CheckoutPage()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            choise.SelectionChanged += Choise_SelectionChanged;
            amount.TextChanged += Amount_TextChanged;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            // Add to shopping cart logic
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            // Reset logic
        }

        private void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Quantity change logic
            CalculateTotalPrice();
        }

        private void Choise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Selected item change logic
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            if (choise.SelectedItem != null && !string.IsNullOrEmpty(amount.Text))
            {
                double itemPrice = GetSelectedItemPrice();
                int itemCount = int.Parse(amount.Text);

                double totalPrice = itemPrice * itemCount;
                total.Text = totalPrice.ToString("C", CultureInfo.CurrentCulture); // Display as currency using current culture
            }
            else
            {
                total.Text = string.Empty;
            }
        }

        private double GetSelectedItemPrice()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)choise.SelectedItem;

            switch (selectedItem.Content.ToString())
            {
                case "Foto 10x15":
                    return 2.55;
                case "Foto 30x20":
                    return 4.95;
                case "Mok met foto":
                    return 9.95;
                case "Sleutelhanger":
                    return 6.12;
                case "T-shirt":
                    return 11.99;
                default:
                    return 0.0;
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Verwijder de toegevoegde items
            fid.Text = string.Empty; // Leegmaken van het FOTO-id veld
            choise.SelectedIndex = -1; // Reset de geselecteerde index van het Product ComboBox
            amount.Text = string.Empty; // Leegmaken van het Aantal veld

            // Voeg hier extra code toe om andere items te resetten indien nodig

            // Reset de totaalprijs naar €0
            total.Text = "€0";
        }


        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
