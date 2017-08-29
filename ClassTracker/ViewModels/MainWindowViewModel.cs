using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassTracker.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ClassTracker.ExtensionMethods;
using ClassTracker.Interfaces;

namespace ClassTracker.ViewModels
{
    class MainWindowViewModel : IViewModel
    {
        private CourseEntities databaseConnection;
        public ObservableCollection<DueItem> Classes { get; set; }
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
            using (databaseConnection = new CourseEntities())
            {
                var classList = databaseConnection.DueItems
                                    .OrderBy(c => c.Date_Due)
                                        .ToList();

                Classes = new ObservableCollection<DueItem>(classList);
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

        private DueItem selectedClass;
        public DueItem SelectedClass
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

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
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

        #region Filter Method

        private DelegateCommand filterCommand;
        public ICommand FilterCommand => filterCommand;
        private void FilterListExecute()
        {

            if (SelectedFilterClass == null)
                throw new InvalidOperationException($"{nameof(SelectedFilterClass)} is null");

            Classes.Clear();

            using (databaseConnection = new CourseEntities())
            {
                var filterList = databaseConnection.DueItems
                                    .Where(x => x.Class == SelectedFilterClass)
                                        .ToList();

                Classes.AddRange<DueItem>(filterList);
            }
        } 
        #endregion

        #region Create Method
        private readonly DelegateCommand createCommand;
        public ICommand CreateCommand => createCommand;
        public void CreateMethod()
        {
            if (ClassName == null && DateDue == null && Importance == null)
                throw new InvalidOperationException($"{nameof(ClassName)}, {nameof(DateDue)} or {nameof(Importance)} is null");

            DateTime formatDateDue;
            try
            {
                formatDateDue = DateTime.Parse(DateDue);
            }
            catch(Exception ex)
            {
                throw new FormatException("The Date Due was in a bad format", ex);
            }

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

            var addClass = new DueItem()
            {
                Class = ClassName,
                Semester = semester,
                Date_Due = formatDateDue,
                Importance = Importance,
                Description = Description
            };

            using (databaseConnection = new CourseEntities())
            {
                databaseConnection.DueItems.Add(addClass);
                databaseConnection.SaveChanges();
            }

            Refresh();
        }
        #endregion

        #region Update Method

        private DelegateCommand updateCommand;
        public ICommand UpdateCommand => updateCommand;
        public void UpdateClass()
        {
            using (databaseConnection = new CourseEntities())
            {
                var updateClass = databaseConnection.DueItems
                                       .Where(x => x.id == SelectedClass.id)
                                            .FirstOrDefault();

                if (updateClass.Date_Due != SelectedClass.Date_Due)
                    updateClass.Date_Due = SelectedClass.Date_Due;

                if (updateClass.Description != SelectedClass.Description)
                    updateClass.Description = SelectedClass.Description;

                databaseConnection.SaveChanges();
            }
        }

        #endregion

        #region DeleteCommand
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand;
        public void DeleteClass()
        {
            if (SelectedClass == null) { throw new InvalidOperationException($"{nameof(SelectedClass)} is null"); }

            using (databaseConnection = new CourseEntities())
            {
                databaseConnection.DueItems.Attach(SelectedClass);
                databaseConnection.DueItems.Remove(SelectedClass);
                databaseConnection.SaveChanges();
            }
            Refresh();

        }

        #endregion

        private void Refresh()
        {
            Classes.Clear();
            using (databaseConnection = new CourseEntities())
            {
                var classList = databaseConnection.DueItems
                                    .OrderBy(c => c.Date_Due);

                Classes.AddRange<DueItem>(classList);
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
