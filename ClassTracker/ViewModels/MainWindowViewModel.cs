using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassTracker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Windows;
using ClassTracker.ExtensionMethods;

namespace ClassTracker.ViewModels
{
    class MainWindowViewModel
    {
        private CoursesEntities databaseConnection;
        public ObservableCollection<class_tracker> Classes { get; set; }
        public List<string> EnrolledClasses { get; set; }
        public List<string> ImportanceList { get; set; }
            
        public MainWindowViewModel()
        {
            this.createCommand = new DelegateCommand(CreateMethod, () => (ClassName != null && DateDue != null && Importance != null));
            this.deleteCommand = new DelegateCommand(DeleteClass, () => (SelectedClass != null));
            this.updateCommand = new DelegateCommand(UpdateClass, () => true);
            this.filterCommand = new DelegateCommand(FilterListExecute, () => (SelectedFilterClass != null));
            
            InitializeComponents();
        }

        public void InitializeComponents()
        {
            using (databaseConnection = new CoursesEntities())
            {
                var classList = databaseConnection.class_tracker.OrderBy(c => c.course).ToList();
                Classes = new ObservableCollection<class_tracker>(classList);
            }

            var list = FillEnrollClasses();
            EnrolledClasses = new List<string>(list);

            ImportanceList = new List<string>()
            {
                "Low",
                "Medium",
                "High"
            };
        }

        private class_tracker selectedClass;
        public class_tracker SelectedClass
        {
            get
            {
                return selectedClass;
            }
            set
            {
                selectedClass = value;
                this.deleteCommand.RaiseCanExecuteChanged();
            }
        }

        #region Class Variables
        
        private string className;
        public string ClassName
        {
            get
            {
                return className;
            }
            set
            {
                className = value;
                CheckIfCanAdd();
            }
        }

        private string dateDue;
        public string DateDue
        {
            get
            {
                return dateDue;
            }
            set
            {
                dateDue = value;
                CheckIfCanAdd();
            }
        }

        private string importance;
        public string Importance
        {
            get
            {
                return importance;
            }
            set
            {
                importance = value;
                CheckIfCanAdd();
            }
        }

        private string selectedFilterClass;
        public string SelectedFilterClass
        {
            get
            {
                return selectedFilterClass;
            }
            set
            {
                selectedFilterClass = value;
                filterCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        private DelegateCommand filterCommand;
        public ICommand FilterCommand => filterCommand;
        private void FilterListExecute()
        {
            
            if (SelectedFilterClass == null)
                throw new InvalidOperationException($"{nameof(SelectedFilterClass)} is null");

            Classes.Clear();

            using (databaseConnection = new CoursesEntities())
            {
                var filterList = databaseConnection.class_tracker
                                    .Where(x => x.course == SelectedFilterClass)
                                        .ToList();

                Classes.AddRange<class_tracker>(filterList);
            }
        }

        #region Create Method
        private readonly DelegateCommand createCommand;
        public ICommand CreateCommand => createCommand;
        public void CreateMethod()
        {
            if (ClassName == null && DateDue == null && Importance == null)
                throw new InvalidOperationException($"{nameof(ClassName)}, {nameof(DateDue)} or {nameof(Importance)} is null");

            var charArray = ClassName.ToArray();
            var getSemester = charArray.Last();
            string semester;
            switch (getSemester)
            {
                case 'a':
                    semester = "1st";
                    break;

                case 'b':
                    semester = "2nd";
                    break;

                default:
                    return;
            }

            var addClass = new class_tracker()
            {
                course = ClassName,
                semester = semester,
                date_due = DateDue,
                importance = Importance
            };

            using (databaseConnection = new CoursesEntities())
            {
                databaseConnection.class_tracker.Add(addClass);
                databaseConnection.SaveChanges();
            }

            Refresh();
        } 
        #endregion

        private DelegateCommand updateCommand;
        public ICommand UpdateCommand => updateCommand;
        public void UpdateClass()
        {
            databaseConnection.class_tracker.Attach(SelectedClass);
            databaseConnection.SaveChanges();
        }

        #region DeleteCommand
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand;
        public void DeleteClass()
        {
            if (SelectedClass == null) { throw new InvalidOperationException($"{nameof(SelectedClass)} is null"); }

            using (databaseConnection = new CoursesEntities())
            {
                databaseConnection.class_tracker.Attach(SelectedClass);
                databaseConnection.class_tracker.Remove(SelectedClass);
                databaseConnection.SaveChanges();
            }
            Refresh();

        }

        #endregion

        private void Refresh()
        {
            Classes.Clear();
            using (databaseConnection = new CoursesEntities())
            {
                var classList = databaseConnection.class_tracker
                                    .OrderBy(c => c.course);

                Classes.AddRange<class_tracker>(classList);
            }
        }

        private void CheckIfCanAdd()
        {
            if (ClassName != null && DateDue != null && Importance != null)
            {
                this.createCommand.RaiseCanExecuteChanged();
            }
        }

        private List<string> FillEnrollClasses()
        {
            var list = new List<string>
            {
                "ECE 4436a",
                "SE 3309a",
                "SE 3313a",
                "SE 3316a",
                "SE 3352a",
                "ECE 3375b",
                "SE 3310b",
                "SE 3314b",
                "SE 3351b",
                "SE 3353b",
                "SE 3350b"
            };

            return list;
        }
    }
}
