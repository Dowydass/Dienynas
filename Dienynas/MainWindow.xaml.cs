using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Dienynas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();
        private System.Threading.Timer _searchTimer;
        private readonly object _lockObject = new object();

        public MainWindow()
        {

            InitializeComponent();

            // Initialize the database connection and fetch users
            _dbManager.ConnectAndFetchUsers();
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

        /// <summary>
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
            VisibilityManager.Hide(EditGradePanel);
            VisibilityManager.Hide(DeleteStudentFromModulePanel);
            VisibilityManager.Hide(SearchBar_TextBox);



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
                // Reload the DataGrid to show the updated data

                Window_Loaded();

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

            // Basic validation for empty fields
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Prašome užpildyti abu laukus: vardą ir pavardę.", 
                    "Įvesties klaida", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Let Student class validate the name and lastname
                bool isFirstNameValid = Student.ValidateWithPopup(firstName, "Vardas");
                bool isLastNameValid = Student.ValidateWithPopup(lastName, "Pavardė");

                // Only proceed if both validations pass
                if (isFirstNameValid && isLastNameValid)
                {
                    // Add student to database
                    InOutUtils.AddStudent(firstName, lastName);
                    
                    // Hide the panel and show success message
                    AddStudentPanel.Visibility = Visibility.Hidden;
                    MessageBox.Show("Studentas sėkmingai pridėtas!", "Pridėti studentą", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Refresh the DataGrid to show the new student
                    StudentGradesDataGrid.ItemsSource = InOutUtils.GetStudents();
                    Window_Loaded();
                    VisibilityManager.Show(StudentGradesDataGrid);
                }
            }
            catch (ArgumentException ex)
            {
                // This catch block is here for any validation exceptions that might not be caught by ValidateWithPopup
                MessageBox.Show(ex.Message, "Validacijos klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            VisibilityManager.Hide(StudentGradesDataGrid);
            DeleteStudentFromModulePanel.Visibility = Visibility.Visible;
            AddStudentPanel.Visibility = Visibility.Hidden;
            AddModulePanel.Visibility = Visibility.Hidden;

            // Populate the Module ComboBox
            var modules = InOutUtils.GetModules();
            DeleteModuleComboBox.ItemsSource = modules;
            DeleteModuleComboBox.DisplayMemberPath = "ModuleName";
            DeleteModuleComboBox.SelectedValuePath = "Id";

            // Add handler for module selection change
            DeleteModuleComboBox.SelectionChanged += DeleteModuleComboBox_SelectionChanged;

            // Initial population of student ComboBox
            UpdateStudentComboBox();
        }

        private void DeleteModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStudentComboBox();
        }

        private void UpdateStudentComboBox()
        {
            if (DeleteModuleComboBox.SelectedValue == null)
                return;

            var selectedModuleId = (int)DeleteModuleComboBox.SelectedValue;
            
            // Get students who have grades in the selected module
            var studentsWithGrades = InOutUtils.GetStudents()
                .Where(student => InOutUtils.GetGrades()
                    .Any(grade => grade.StudentId == student.Id && grade.ModuleId == selectedModuleId))
                .ToList();

            if (!studentsWithGrades.Any())
            {
                MessageBox.Show("Modulyje studentų sąrašas yra tuščias", "Informacija", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DeleteStudentComboBox.ItemsSource = null;
            }
            else
            {
                DeleteStudentComboBox.ItemsSource = studentsWithGrades;
                DeleteStudentComboBox.DisplayMemberPath = "Name";
                DeleteStudentComboBox.SelectedValuePath = "Id";
            }
        }

        private void SubmitDeleteStudentFromModule_Click(object sender, RoutedEventArgs e)
        {
            bool success = InOutUtils.HandleStudentModuleDeletion(
                DeleteModuleComboBox.SelectedValue, 
                DeleteStudentComboBox.SelectedValue
            );
            
            if (success)
            {
                Window_Loaded();
                DeleteStudentFromModulePanel.Visibility = Visibility.Hidden;
                VisibilityManager.Show(StudentGradesDataGrid);

            }
        }

        private void DeleteDataBase_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager db = new DatabaseManager();
            db.ResetDatabase();
           
        }
        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            VisibilityManager.Hide(SearchBar_TextBox);
            VisibilityManager.Hide(StudentGradesDataGrid);
            VisibilityManager.Hide(AddStudentPanel);
            VisibilityManager.Hide(AddModulePanel);
            VisibilityManager.Hide(DeleteStudentFromModulePanel);
            VisibilityManager.Show(EditGradePanel);

            // Populate the Module ComboBox
            var modules = InOutUtils.GetModules();
            EditModuleComboBox.ItemsSource = modules;
            EditModuleComboBox.DisplayMemberPath = "ModuleName";
            EditModuleComboBox.SelectedValuePath = "Id";

            // Clear previous selections
            EditStudentComboBox.ItemsSource = null;
            NewGradeTextBox.Text = string.Empty;
        }

        private void EditModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditModuleComboBox.SelectedValue == null)
                return;

            var selectedModuleId = (int)EditModuleComboBox.SelectedValue;
            
            // Get students who have grades in the selected module
            var studentsWithGrades = InOutUtils.GetStudents()
                .Where(student => InOutUtils.GetGrades()
                    .Any(grade => grade.StudentId == student.Id && 
                                 grade.ModuleId == selectedModuleId))
                .ToList();

            if (!studentsWithGrades.Any())
            {
                MessageBox.Show("Modulyje studentų sąrašas yra tuščias", "Informacija", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                EditStudentComboBox.ItemsSource = null;
            }
            else
            {
                EditStudentComboBox.ItemsSource = studentsWithGrades;
                EditStudentComboBox.DisplayMemberPath = "Name";
                EditStudentComboBox.SelectedValuePath = "Id";
            }
        }

        private void SubmitEditGrade_Click(object sender, RoutedEventArgs e)
        {
            if (EditModuleComboBox.SelectedValue == null || 
                EditStudentComboBox.SelectedValue == null || 
                string.IsNullOrWhiteSpace(NewGradeTextBox.Text))
            {
                MessageBox.Show("Prašome užpildyti visus laukus", "Klaida", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(NewGradeTextBox.Text, out int newGrade) || 
                newGrade < 0 || newGrade > 10)
            {
                MessageBox.Show("Pažymys turi būti skaičius nuo 0 iki 10", "Klaida", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var moduleId = (int)EditModuleComboBox.SelectedValue;
            var studentId = (int)EditStudentComboBox.SelectedValue;

            // Create database manager instance
            DatabaseManager db = new DatabaseManager();
            
            // Update the grade in the database
            bool success = db.UpdateGrade(moduleId, studentId, newGrade);

            if (success)
            {
                MessageBox.Show("Pažymys sėkmingai atnaujintas!", "Sėkmė", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Window_Loaded(); // Refresh the data grid
                VisibilityManager.Hide(EditGradePanel);
                VisibilityManager.Show(StudentGradesDataGrid);
                
            }
            else
            {
                MessageBox.Show("Nepavyko atnaujinti pažymio", "Klaida", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            VisibilityManager.Hide(StudentGradesDataGrid);
            VisibilityManager.Hide(AddStudentPanel);
            VisibilityManager.Hide(AddModulePanel);
            VisibilityManager.Hide(DeleteStudentFromModulePanel);
            VisibilityManager.Show(EditGradePanel);
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
            lock (_lockObject)
            {
                // Reset the timer on each keystroke
                _searchTimer?.Dispose();
                
                // Create a new timer that waits 300ms before executing the search
                _searchTimer = new System.Threading.Timer(_ => 
                {
                    // We need to use the dispatcher to update UI from a different thread
                    Dispatcher.Invoke(() =>
                    {
                        string query = SearchBar_TextBox.Text;
                        
                        if (!string.IsNullOrEmpty(query))
                        {
                            // Use the matrix-based approach for displaying search results
                            var filteredRows = InOutUtils.SearchStudentsInMatrix(query);
                            Console.WriteLine($"Search completed: {filteredRows.Count} results found");
                            
                            Debug.WriteLine($"Search completed: {filteredRows.Count} results found");
                            // Update the DataGrid with the filtered matrix data
                            StudentGradesDataGrid.ItemsSource = filteredRows;
                        }
                        else
                        {
                            // If the search box is empty, reload the full matrix
                            InOutUtils.LoadStudentGradesMatrix(StudentGradesDataGrid);
                        }
                    });
                }, null, 300, System.Threading.Timeout.Infinite);
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
            Window_Loaded();
            VisibilityManager.Show(SearchBar_TextBox);
            VisibilityManager.Show(StudentGradesDataGrid);
            VisibilityManager.Hide(AddStudentPanel);
            VisibilityManager.Hide(AddModulePanel);
            VisibilityManager.Hide(DeleteStudentFromModulePanel);
            VisibilityManager.Hide(EditGradePanel);
            
        }
    }
}
