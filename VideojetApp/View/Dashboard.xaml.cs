using System.Windows;
using VideojetApp.ViewModel;

namespace VideojetApp.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();

            this.DataContext = new DashboardViewModel(this);
        }
    }
}
