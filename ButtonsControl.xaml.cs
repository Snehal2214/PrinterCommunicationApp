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
using System.Windows.Shapes;
using VideojetApp.ViewModel;

namespace VideojetApp
{
    /// <summary>
    /// Interaction logic for ButtonsControl.xaml
    /// </summary>
    public partial class ButtonsControl : Window
    {

        public ButtonsControl()
        {
            InitializeComponent();
            //this.DataContext = new DashboardViewModel();
            this.DataContext = new ButtonsControlViewModel();
         

        }

    }
}
