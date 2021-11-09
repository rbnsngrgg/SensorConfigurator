using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SensorConfigurator.Objects;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppLogger appLogger = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DisplayAndLogError(string caption, string message)
        {
            _ = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            appLogger.Write($"{caption}: {message}");
        }
    }
}
