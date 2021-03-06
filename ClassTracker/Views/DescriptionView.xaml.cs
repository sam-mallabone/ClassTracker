﻿using ClassTracker.Interfaces;
using ClassTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClassTracker.Views
{
    /// <summary>
    /// Interaction logic for DescriptionView.xaml
    /// </summary>
    public partial class DescriptionView : UserControl
    {
        private IViewModel viewModel;

        public DescriptionView()
        {
            InitializeComponent();
            this.viewModel = new DescriptionViewModel();
            this.DataContext = viewModel;
        }
    }
}
