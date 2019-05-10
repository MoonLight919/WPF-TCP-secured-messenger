﻿using ClassesForNP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for AllUsersWindow.xaml
    /// </summary>
	public partial class AllUsersWindow : Window
    {
        AllUsersViewModel vm;


        public AllUsersWindow(AllUsersViewModel allUsersWindow)
        {
            InitializeComponent();
            vm = allUsersWindow;
            DataContext = vm;
            allContactsLb.ItemsSource = vm.Contacts;
        }
    }
}
