using System;
using System.Windows;
using System.Windows.Controls;

namespace Dienynas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DatabaseManager db = new DatabaseManager();
            db.ConnectAndFetchUsers();
        }

                
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDataGrid.Visibility = Visibility.Hidden;
            // Implement logic to show a form or input fields for adding a new student
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StudentDataGrid.Visibility = Visibility.Hidden;
            // Implement logic to show a form or input fields for adding a new student
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Add student logic here
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // Delete student logic here
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            // Edit student logic here
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            // Add grade logic here
        }

        private void SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            // Search student logic here
        }

        private void SortStudent_Click(object sender, RoutedEventArgs e)
        {
            // Sort student logic here
        }

       

    }
}
