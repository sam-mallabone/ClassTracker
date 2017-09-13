using System;
using System.Collections.Generic;
using System.Linq;
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
        private CoursesEntities1 databaseConnection;
        public ObservableCollection<DueItem> Classes { get; set; }
        public List<string> EnrolledClasses { get; set; }
        public List<string> ImportanceList { get; set; }
            
        public MainWindowViewModel()
        {
            this.createCommand = new DelegateCommand(CreateMethod, () => (ClassName != null && DateDue != null && Importance != null));
            this.deleteCommand = new DelegateCommand(DeleteClass, () => (SelectedClass != null));
            this.updateCommand = new DelegateCommand(UpdateClass);
            this.filterCommand = new DelegateCommand(FilterListExecute, () => (SelectedFilterClass != null));
            
            InitializeComponents();
        }
        
        public void InitializeComponents()
        {
            using (databaseConnection = new CoursesEntities1())
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

        /// <summary>
        /// Filter the list of classes that are displayed in the DataGridView
        /// </summary>
        #region Filter Method

        private DelegateCommand filterCommand;
        public ICommand FilterCommand => filterCommand;
        private void FilterListExecute()
        {

            if (SelectedFilterClass == null)
                throw new InvalidOperationException($"{nameof(SelectedFilterClass)} is null");

            Classes.Clear();

            using (databaseConnection = new CoursesEntities1())
            {
                var filterList = databaseConnection.DueItems
                                    .Where(x => x.Class == SelectedFilterClass)
                                        .OrderBy(x => x.Date_Due)
                                            .ToList();

                Classes.AddRange<DueItem>(filterList);
            }
        } 
        #endregion

        /// <summary>
        /// Create a new DueItem to be inserted into the database
        /// </summary>
        #region Create Method
        private readonly DelegateCommand createCommand;
        public ICommand CreateCommand => createCommand;
        public void CreateMethod()
        {
            //If any of the properties are null, there was a problem loading the variables
            if (ClassName == null && DateDue == null && Importance == null)
                throw new InvalidOperationException($"{nameof(ClassName)}, {nameof(DateDue)} or {nameof(Importance)} is null");

            DateTime formatDateDue;

            try
            {
                //Convert the date String into type DateTime
                formatDateDue = DateTime.Parse(DateDue);
            }
            catch(Exception ex)
            {
                //String was in a incorrect format
                throw new FormatException("The Date Due was in a bad format", ex);
            }

            //Retrieves the letter from the end of the Course, this determines which semester the class is in
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
                    semester = "NA";
                    break;
            }

            var addClass = new DueItem()
            {
                Class = ClassName,
                Semester = semester,
                Date_Due = formatDateDue,
                Importance = Importance,
                Description = Description
            };

            using (databaseConnection = new CoursesEntities1())
            {
                databaseConnection.DueItems.Add(addClass);
                databaseConnection.SaveChanges();
            }

            Refresh();
        }
        #endregion

        /// <summary>
        /// Update an existing DueItem, only the date due or the description can be changed
        /// </summary>
        #region Update Method

        private DelegateCommand updateCommand;
        public ICommand UpdateCommand => updateCommand;
        public void UpdateClass()
        {
            using (databaseConnection = new CoursesEntities1())
            {
                var updateClass = databaseConnection.DueItems
                                       .Where(x => x.id == SelectedClass.id)
                                            .FirstOrDefault();

                if (updateClass.Date_Due != SelectedClass.Date_Due)
                    updateClass.Date_Due = SelectedClass.Date_Due;

                if (updateClass.Description != SelectedClass.Description)
                    updateClass.Description = SelectedClass.Description;

                databaseConnection.SaveChanges();

                Refresh();
            }
        }

        #endregion

        /// <summary>
        /// Delete a DueItem in the database
        /// </summary>
        #region DeleteCommand
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand;
        public void DeleteClass()
        {
            if (SelectedClass == null) { throw new InvalidOperationException($"{nameof(SelectedClass)} is null"); }

            using (databaseConnection = new CoursesEntities1())
            {
                databaseConnection.DueItems.Attach(SelectedClass);
                databaseConnection.DueItems.Remove(SelectedClass);
                databaseConnection.SaveChanges();
            }
            Refresh();

        }

        #endregion

        /// <summary>
        /// Refresh the list that is bound to the DataGridView
        /// </summary>
        private void Refresh()
        {
            Classes.Clear();
            using (databaseConnection = new CoursesEntities1())
            {
                var classList = databaseConnection.DueItems
                                    .OrderBy(c => c.Date_Due);

                Classes.AddRange<DueItem>(classList);
            }
        }

        /// <summary>
        /// Predicate Function that returns whether the conditions have been met to add a new class
        /// </summary>
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
                "SE 3350b",
                "Other"
            };

            return list;
        }
    }
}
