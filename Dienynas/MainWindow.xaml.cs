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
            DataContext = this;
            // Initialize the window and load data
            Window_Loaded();


        }

        private void Window_Loaded()
        {
            // Load data into the DataGrid
            var students = InOutUtils.GetStudents();
            var modules = InOutUtils.GetModules();
            var grades = InOutUtils.GetGrades();

            InOutUtils.ConfigureStudentGradesDataGrid(StudentGradesDataGrid);
            InOutUtils.LoadStudentGradesMatrix(StudentGradesDataGrid);
            
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
            VisibilityManager.Show(AddModulePanel);
            VisibilityManager.Hide(AddStudentPanel);
            VisibilityManager.Hide(StudentGradesDataGrid);

            VisibilityManager.Hide(DeleteStudentFromModulePanel);
        }
        private void SubmitNewModule_Click(object sender, RoutedEventArgs e)
        {
            string moduleName = ModuleNameTextBox.Text;

            if (!string.IsNullOrEmpty(moduleName))
            {
                InOutUtils.AddModule(moduleName); // Call the AddModule function
                AddModulePanel.Visibility = Visibility.Hidden;
                // Refresh the DataGrid to show the new module

                MessageBox.Show("Modulis sėkmingai pridėtas!", "Pridėti modulį", MessageBoxButton.OK, MessageBoxImage.Information);
                VisibilityManager.Show(StudentGradesDataGrid);
            }
            else
            {
                MessageBox.Show("Įveskite modulio pavadinimą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
          /// Add student logic here
            VisibilityManager.Show(AddStudentPanel);
            VisibilityManager.Hide(AddModulePanel);
            VisibilityManager.Hide(DeleteStudentFromModulePanel);
            VisibilityManager.Hide(StudentGradesDataGrid);
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

                MessageBox.Show("Studentas sėkmingai pridėtas!", "Pridėti studentą", MessageBoxButton.OK, MessageBoxImage.Information);
                // Refresh the DataGrid to show the new student  

                StudentGradesDataGrid.ItemsSource = InOutUtils.GetStudents();
                Window_Loaded();
                VisibilityManager.Show(StudentGradesDataGrid);

              
            }
            else
            {
                MessageBox.Show("Please enter both first name and last name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            DeleteStudentFromModulePanel.Visibility = Visibility.Visible;
            AddStudentPanel.Visibility = Visibility.Hidden;
            AddModulePanel.Visibility = Visibility.Hidden;

            // Populate the ComboBoxes
            DeleteModuleComboBox.ItemsSource = InOutUtils.GetModules();
            DeleteModuleComboBox.DisplayMemberPath = "ModuleName";
            DeleteModuleComboBox.SelectedValuePath = "Id";

            DeleteStudentComboBox.ItemsSource = InOutUtils.GetStudents();
            DeleteStudentComboBox.DisplayMemberPath = "Name";
            DeleteStudentComboBox.SelectedValuePath = "Id";
        }

        private void SubmitDeleteStudentFromModule_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteModuleComboBox.SelectedValue is int moduleId && DeleteStudentComboBox.SelectedValue is int studentId)
            {
                InOutUtils.DeleteStudentFromModule(studentId, moduleId);

                // Refresh the DataGrid
                var students = InOutUtils.GetStudents();

                DeleteStudentFromModulePanel.Visibility = Visibility.Hidden;

                MessageBox.Show("Studentas sėkmingai ištrintas iš modulio!", "Ištrinti studentą iš modulio", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Pasirinkite modulį ir studentą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteDataBase_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager db = new DatabaseManager();
            db.ResetDatabase();
            MessageBox.Show("Duomenų bazė sėkmingai ištrinta!", "Ištrinti duomenų bazę", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            // Edit student logic here
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            // Add grade logic here
        }

      

        private void SortStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentPanel.Visibility = Visibility.Hidden;
            AddModulePanel.Visibility = Visibility.Hidden;
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
       
            string query = SearchBar_TextBox.Text;
            
            if (!string.IsNullOrEmpty(query))
            {
                // Get students from the database
                var students = InOutUtils.SearchStudents(query);

                // Sort the students by name (ascending)
                var sortedStudents = TaskUtils.SortStudentsByName(students);

                // Update the DataGrid with the sorted students
                StudentGradesDataGrid.ItemsSource = sortedStudents;
            }
            else
            {
                // If the search bar is empty, reload all students
                var allStudents = InOutUtils.GetStudents();
                StudentGradesDataGrid.ItemsSource = allStudents;
            }
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBar_TextBox.Text;

            if (!string.IsNullOrEmpty(query))
            {
                // Get students from the database
                var students = InOutUtils.SearchStudents(query);

                // Sort the students by name (ascending)
                var sortedStudents = TaskUtils.SortStudentsByName(students);

                // Update the DataGrid with the sorted students
                StudentGradesDataGrid.ItemsSource = sortedStudents;
            }
            else
            {
                MessageBox.Show("Įveskite paieškos užklausą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
