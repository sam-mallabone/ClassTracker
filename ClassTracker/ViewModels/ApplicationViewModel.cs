using ClassTracker.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClassTracker.ViewModels
{
    /// <summary>
    /// This view models purpose is to control all the other view models in the project.
    /// This will help me navigate between the different views within my project.
    /// </summary>
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public ICommand DueCommand { get; set; }
        public ICommand DescriptionCommand { get; set; }
        public ICommand ScheduleCommand { get; set; }
        private IViewModel DueListViewModel;
        private IViewModel DescriptionViewModel;
        private IViewModel ScheduleViewModel;
        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        public ApplicationViewModel()
        {
            DueCommand = new DelegateCommand(OpenDue);
            DescriptionCommand = new DelegateCommand(OpenDescription);
            ScheduleCommand = new DelegateCommand(OpenSchedule);
            DueListViewModel = new MainWindowViewModel();
            DescriptionViewModel = new DescriptionViewModel();
            ScheduleViewModel = new ScheduleViewModel();
            SelectedViewModel = DueListViewModel;
        }

        private void OpenDue()
        {
            SelectedViewModel = DueListViewModel;
        }

        private void OpenDescription()
        {
            SelectedViewModel = DescriptionViewModel;
        }

        private void OpenSchedule()
        {
            SelectedViewModel = ScheduleViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
