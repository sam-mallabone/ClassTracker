using System;
using System.Collections.Generic;

namespace APIToClassDatabase.Models
{
    public partial class ClassTracker
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }
        public string DateDue { get; set; }
        public string Importance { get; set; }
    }
}
