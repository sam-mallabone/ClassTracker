using ClassTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassTracker.Models;

namespace ClassTracker.ViewModels
{
    class DescriptionViewModel : IViewModel
    {
        public List<string> ClassList { get; set; }
        private CoursesEntities1 databaseConnection;
        public DescriptionViewModel()
        {
            InitializeClassList();
        }

        private void InitializeClassList()
        {
            ClassList = new List<string>()
            {
               "ECE 4436a",
                "SE 3309a",
                "SE 3313a",
                "SE 3316a",
                "SE 3352a",
            };
        }

        #region Variables

        private string selectedClass;
        public string SelectedClass
        {
            get
            {
                return selectedClass;
            }
            set
            {
                if(selectedClass != value)
                {
                    selectedClass = value;
                    using(databaseConnection = new CoursesEntities1())
                    {
                        var getClass = databaseConnection.ClassDescriptions.Where(x => x.Course_Number == selectedClass).FirstOrDefault();
                        //Set all the variable bound to the view
                        CourseNumber = getClass.Course_Number;
                        CourseName = getClass.Course_Name;
                        Instructor = getClass.Instructor;
                        OfficeHours = getClass.Office_Hours;
                        Email = getClass.Email;
                        Office = getClass.Office;

                        char[] getLastLetter = CourseNumber.ToCharArray();
                        char lastLetter = getLastLetter[getLastLetter.Length - 1];
                        switch (lastLetter)
                        {
                            case 'a':
                                Semester = "1st";
                                break;

                            case 'b':
                                Semester = "2nd";
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
        
        private string courseNumber;
        public string CourseNumber
        {
            get
            {
                return courseNumber;
            }
            set
            {
                courseNumber = value;
            }
        }

        private string courseName;
        public string CourseName
        {
            get
            {
                return courseName;
            }
            set
            {
                courseName = value;
            }
        }

        private string instructor;
        public string Instructor
        {
            get
            {
                return instructor;
            }
            set
            {
                instructor = value;
            }
        }

        private string officeHours;
        public string OfficeHours
        {
            get
            {
                return officeHours;
            }
            set
            {
                officeHours = value;
            }
        }

        public string Semester { get; set; }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        private string office;
        public string Office
        {
            get
            {
                return office;
            }
            set
            {
                office = value;
            }
        }
        
        #endregion
    }
}
