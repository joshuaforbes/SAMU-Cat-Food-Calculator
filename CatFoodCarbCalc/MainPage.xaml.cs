using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CatFoodCarbCalc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Checks each input field to determine if any are blank.
        /// </summary>
        /// <returns>
        /// Returns true if any field is blank and false otherwise.
        /// </returns>
        private Boolean isAnythingBlank()
        {
            if (string.IsNullOrEmpty(inputAsh.Text)) return true;
            if (string.IsNullOrEmpty(inputProtein.Text)) return true;
            if (string.IsNullOrEmpty(inputFat.Text)) return true;
            if (string.IsNullOrEmpty(inputFiber.Text)) return true;
            if (string.IsNullOrEmpty(inputMoisture.Text)) return true;
            if (string.IsNullOrEmpty(inputTaurine.Text)) return true;
            return false;
        }

        /// <summary>
        /// Calculates the wet carbohydrate percentage based on the values passed in.
        /// </summary>
        /// <param name="values">
        /// A list of doubles containing the values to be used in the claculation. The values must be
        /// aligned in the list like so:
        ///     ash
        ///     protein
        ///     fat
        ///     fiber
        ///     moisture
        ///     taurine
        /// </param>
        /// <returns>
        /// Returnes the percentage as a double value
        /// </returns>
        private double calculateWetCarbPercent(List<double> values)
        {
            double val = 0.00;
            values.ForEach(p => val += p);
            val = 100.00 - val;
            return val;
        }

        /// <summary>
        /// Calculates the dry carbohydrate percentage based on the values passed in.
        /// </summary>
        /// <param name="moisture">
        /// The moisture content previously extracted.</param>
        /// <param name="wetCarb">
        /// The previously calculated wet carbohydrate percentage.</param>
        /// <returns></returns>
        private double calculateDryCarbPercent(double moisture, double wetCarb)
        {
            double divisor = 100 - moisture;
            return (wetCarb / divisor) * 100;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            //If anything is blank do nothing, the user will figure it out
            if (isAnythingBlank()) return;
            
            //Extract Input
            double ash, protein, fat, fiber, moisture, taurine, wcarb, dcarb;
            ash = protein = fat = fiber = moisture = taurine = wcarb = dcarb = 0.00;

            try
            {
                ash = Convert.ToDouble(inputAsh.Text);
                fat = Convert.ToDouble(inputFat.Text);
                fiber = Convert.ToDouble(inputFiber.Text);
                moisture = Convert.ToDouble(inputMoisture.Text);
                taurine = Convert.ToDouble(inputTaurine.Text);
                protein = Convert.ToDouble(inputProtein.Text);
            } catch (Exception)
            {
                //If conversion to double fails, user entered non numeric input. Do nothing.
                return;
            }

            //Calculate Values
            wcarb = calculateWetCarbPercent(new List<double> { ash, protein, fat, fiber, moisture, taurine});
            dcarb = calculateDryCarbPercent(moisture, wcarb);

            //Present to User
            outputDryCarb.Text = dcarb.ToString();
            outputWetCarb.Text = wcarb.ToString();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            string blank = "Enter Data";
            inputAsh.PlaceholderText = blank;
            inputFat.PlaceholderText = blank;
            inputFiber.PlaceholderText = blank;
            inputMoisture.PlaceholderText = blank;
            inputProtein.PlaceholderText = blank;
            inputTaurine.PlaceholderText = blank;
            outputDryCarb.Text = "0.00";
            outputWetCarb.Text = "0.00";
            
        }
    }
}
