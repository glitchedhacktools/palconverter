using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using palconverter;

namespace palconverter
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            convertButton.IsEnabled = false;
        }

        Palconverter_functions functions = new Palconverter_functions();
        bool enableAuto = false;

        private void OpenDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Adobe Color Table|*.act|Dissassembly palette|*.pal";
            if (dialog.ShowDialog() == true)
            {
                enableAuto = false;
                pathBox.Text = dialog.FileName;
                if (dialog.FileName.EndsWith(".pal"))
                {
                    previewBox.Text = functions.ReadJASC(dialog.FileName);
                    formatCombo.SelectedIndex = 0;
                }

                if (dialog.FileName.EndsWith(".act"))
                {
                    previewBox.Text = functions.ReadHex(dialog.FileName);
                    formatCombo.SelectedIndex = 1;
                }
                enableAuto = true;
                convertButton.IsEnabled = true;
            }
        }

        private void ConvertPalAction(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            switch (formatCombo.SelectedIndex)
            {
                case 0:
                    dialog.Filter = "Dissassembly palette| *.pal";
                    break;
                case 1: dialog.Filter = "Adobe Color Table|*.act";
                    break;
                default: break;
            }
            if (dialog.ShowDialog() == true)
            {
                functions.SaveJASCfile(previewBox.Text, dialog.FileName);
            }
        }

        private void ConvertOnChangeIndex(object sender, SelectionChangedEventArgs e)
        {
            if (enableAuto)
            {
                switch (formatCombo.SelectedIndex)
                {
                    case 0:
                        previewBox.Text = functions.ConvertACTtoJASC(int.Parse(numColorBox.Text));
                        break;
                    case 1:
                        previewBox.Text = functions.ConvertJASCtoACT(255);
                        break;
                    default: break;
                }
            }
        }

        private void RecalcNumColors(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Content)
            {
                case "▲":
                    numColorBox.Text = (int.Parse(numColorBox.Text) + 1).ToString();
                    break;
                case "▼":
                    numColorBox.Text = (int.Parse(numColorBox.Text) - 1).ToString();
                    if (int.Parse(numColorBox.Text) < 16) numColorBox.Text = "16";
                    break;
                default: break;
            }
        }

        private void AvoidText(object sender, KeyEventArgs e)
        {
            int i;
            for (i = 0; i < numColorBox.Text.Length; i++)
            {
                if ((numColorBox.Text[i] != '0') && (numColorBox.Text[i] != '1') && (numColorBox.Text[i] != '2') && (numColorBox.Text[i] != '3') && (numColorBox.Text[i] != '4') && (numColorBox.Text[i] != '5') && (numColorBox.Text[i] != '6') && (numColorBox.Text[i] != '7') && (numColorBox.Text[i] != '8') && (numColorBox.Text[i] != '9'))
                {
                    numColorBox.Text = numColorBox.Text.Remove(i, 1);
                }
            }
            if (numColorBox.Text == "") numColorBox.Text = "16";
            if (int.Parse(numColorBox.Text) >= 256) numColorBox.Text = "255";
            if (int.Parse(numColorBox.Text) < 16) numColorBox.Text = "16";
        }
    }
}
