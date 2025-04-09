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

            // Initialize the database connection and fetch users
            DatabaseManager db = new DatabaseManager();
            db.ConnectAndFetchUsers();

            // Initialize the window and load data
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            // Load data into the DataGrid
            var students = InOutUtils.GetStudents();
            var modules = InOutUtils.GetModules();
            var grades = InOutUtils.GetGrades();
            StudentDataGrid.ItemsSource = students;
        }
        
        /// Parodo pranešimą su nurodytu tekstu ir pavadinimu.

        public static void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        
        /// Uždaro programą.
        public static void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       /// TODO: Įvesti naują modulį.
        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            // Logic to handle adding a module
            AddModulePanel.Visibility = Visibility.Visible;
            StudentDataGrid.Visibility = Visibility.Hidden;
            AddStudentPanel.Visibility = Visibility.Hidden;
        }
        private void SubmitNewModule_Click(object sender, RoutedEventArgs e)
        {
            string moduleName = ModuleNameTextBox.Text;

            if (!string.IsNullOrEmpty(moduleName))
            {
                InOutUtils.AddModule(moduleName); // Call the AddModule function
                AddModulePanel.Visibility = Visibility.Hidden;
                StudentDataGrid.Visibility = Visibility.Visible;

                MessageBox.Show("Modulis sėkmingai pridėtas!", "Pridėti modulį", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Įveskite modulio pavadinimą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
          /// Add student logic here
            StudentDataGrid.Visibility = Visibility.Hidden;
            AddModulePanel.Visibility = Visibility.Hidden;
            AddStudentPanel.Visibility = Visibility.Visible;
        }

  
     
 
        private void SubmitNewStudent_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                if(sbyte.TryParse(FirstNameTextBox.Text, out sbyte result))
                {
                    MessageBox.Show("Please enter a valid first name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                    InOutUtils.AddStudent(firstName, lastName);
                AddStudentPanel.Visibility = Visibility.Hidden;
                StudentDataGrid.Visibility = Visibility.Visible;
                // Refresh the DataGrid to show the new student
               // StudentDataGrid.ItemsSource = dashboard.GetStudents();
            }
            else
            {
                MessageBox.Show("Please enter both first name and last name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            
        }

        private void SortStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentPanel.Visibility = Visibility.Hidden;
            AddModulePanel.Visibility = Visibility.Hidden;
            StudentDataGrid.Visibility = Visibility.Visible;
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
