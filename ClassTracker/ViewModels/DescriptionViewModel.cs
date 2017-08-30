using ClassTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTracker.ViewModels
{
    class DescriptionViewModel : IViewModel
    {
        public List<string> classList;
        public DescriptionViewModel()
        {
            InitializeClassList();
        }

        private void InitializeClassList()
        {
            classList = new List<string>()
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
        }
    }
}
