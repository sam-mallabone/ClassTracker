using ClassTracker.Interfaces;
using ClassTracker.ViewModels;
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

namespace ClassTracker.Views
{
    /// <summary>
    /// Interaction logic for DueListView.xaml
    /// </summary>
    public partial class DueListView : UserControl
    {
        private IViewModel viewModel;
        public DueListView()
        {
            InitializeComponent();
            this.viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }
    }
}
