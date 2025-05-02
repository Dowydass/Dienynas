using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dienynas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Pagrindinio lango sąveikos logika
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();
        private System.Threading.Timer _searchTimer;
        private readonly object _lockObject = new object();
        private Dictionary<string, UIElement> _panels;
        private ViewMode _currentViewMode = ViewMode.Default;
        private bool _sortAscending = true;
        private int _currentSortColumnIndex = 0;
        private bool _quickSortAscending = true;
        private int _quickSortColumnIndex = 0;
        private bool _isSearchSortPanelVisible = true;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize panels dictionary
            // Inicializuoti panelių žodyną
            InitializePanelsDict();

            // Initialize the database connection and fetch users
            // Inicializuoti duomenų bazės jungtį ir gauti vartotojus
            _dbManager.ConnectAndFetchUsers();
            DataContext = this;

            // Add keyboard shortcut for toggling search/sort panel
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.F && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    ToggleSearchSortPanel_Click(this, new RoutedEventArgs());
                }
            };

            // Initialize the window and load data
            // Inicializuoti langą ir užkrauti duomenis
            Window_Loaded();
        }

        /// <summary>
        /// Initializes the dictionary of UI panels for easier visibility management
        /// Inicializuoja UI panelių žodyną lengvesniam matomumo valdymui
        /// </summary>
        private void InitializePanelsDict()
        {
            _panels = new Dictionary<string, UIElement>
            {
                { "StudentGradesDataGrid", StudentGradesDataGrid },
                { "AddStudentPanel", AddStudentPanel },
                { "AddModulePanel", AddModulePanel },
                { "DeleteStudentFromModulePanel", DeleteStudentFromModulePanel },
                { "DeleteStudentEntirelyPanel", DeleteStudentEntirelyPanel },
                { "EditGradePanel", EditGradePanel },
                { "SearchSortPanel", SearchSortPanel },
                { "SortingPanel", SortingPanel }
            };
        }

        /// <summary>
        /// Changes the current view mode and updates panel visibility
        /// Pakeičia dabartinį rodymo režimą ir atnaujina panelių matomumą
        /// </summary>
        /// <param name="viewMode">The view mode to switch to / Rodymo režimas į kurį perjungti</param>
        private void SwitchView(ViewMode viewMode)
        {
            try
            {
                _currentViewMode = viewMode;
                VisibilityManager.SwitchToView(viewMode, _panels);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching view: {ex.Message}", "View Error");
            }
        }

        /// <summary>
        /// Loads data into the window and configures the DataGrid
        /// Užkrauna duomenis į langą ir konfigūruoja duomenų tinklelį
        /// </summary>
        private void Window_Loaded()
        {
            try
            {
                // Load data into the DataGrid
                // Užkrauti duomenis į duomenų tinklelį
                var students = InOutUtils.GetStudents();
                var modules = InOutUtils.GetModules();
                var grades = InOutUtils.GetGrades();

                InOutUtils.ConfigureStudentGradesDataGrid(StudentGradesDataGrid);
                InOutUtils.LoadStudentGradesMatrix(StudentGradesDataGrid);

                // Update sort columns for both sorting panels
                UpdateSortColumnComboBox();
                UpdateQuickSortComboBox();

                // Return to default view
                // Grįžti į numatytąjį rodinį
                VisibilityManager.Show(StudentGradesDataGrid);

                // Keep the search and sort panel visibility state
                SearchSortPanel.Visibility = _isSearchSortPanelVisible ? Visibility.Visible : Visibility.Collapsed;
                ToggleButtonIcon.Text = _isSearchSortPanelVisible ? "⮝" : "⮟";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Data Error");
            }
        }

        /// <summary>
        /// Updates the sort column combo box with module names
        /// </summary>
        private void UpdateSortColumnComboBox()
        {
            try
            {
                var modules = InOutUtils.GetModules();
                if (!TaskUtils.UpdateComboBoxWithModules(SortColumnComboBox, modules))
                {
                    MessageBox.Show("Nepavyko atnaujinti rūšiavimo stulpelių.", "Rūšiavimo klaida", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Klaida atnaujinant rūšiavimo stulpelius: {ex.Message}", "Rūšiavimo klaida");
            }
        }

        /// <summary>
        /// Updates the quick sort combo box with available modules
        /// </summary>
        private void UpdateQuickSortComboBox()
        {
            try
            {
                // Keep the first two items (name and average)
                while (QuickSortComboBox.Items.Count > 2)
                {
                    QuickSortComboBox.Items.RemoveAt(2);
                }

                // Add module columns
                var modules = InOutUtils.GetModules();
                for (int i = 0; i < modules.Count; i++)
                {
                    QuickSortComboBox.Items.Add(new ComboBoxItem 
                    { 
                        Content = modules[i].ModuleName, 
                        Tag = i + 1 // +1 because the first column is the student name
                    });
                }

                // Select the first item by default if not already selected
                if (QuickSortComboBox.SelectedIndex < 0 && QuickSortComboBox.Items.Count > 0)
                {
                    QuickSortComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating quick sort combo box: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows a message dialog with the specified text and title.
        /// Rodo pranešimą su nurodytu tekstu ir pavadinimu.
        /// </summary>
        public static void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Closes the application.
        /// Uždaro programą.
        /// </summary>
        public static void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles DataGrid selection changes
        /// Apdoroja duomenų tinklelio pasirinkimo pokyčius
        /// </summary>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reserved for future implementation
            // Rezervuota būsimam įgyvendinimui
        }

        /// <summary>
        /// Handles adding a new module.
        /// Apdoroja naujo modulio pridėjimą.
        /// </summary>
        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            // Ensure text box is empty before showing panel
            // Įsitikinti, kad teksto laukas yra tuščias prieš rodant panelį
            ModuleNameTextBox.Text = string.Empty;
            SwitchView(ViewMode.AddModule);
        }
        
        /// <summary>
        /// Submits a new module to the database
        /// Išsaugo naują modulį duomenų bazėje
        /// </summary>
        private void SubmitNewModule_Click(object sender, RoutedEventArgs e)
        {
            string moduleName = ModuleNameTextBox.Text;
            
            // Basic validation for empty fields
            // Pagrindinė patikra dėl tuščių laukų
            if (string.IsNullOrEmpty(moduleName))
            {
                MessageBox.Show("Įveskite modulio pavadinimą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            try
            {
                // Let Module class validate the module name
                // Leisti Module klasei validuoti modulio pavadinimą
                bool isModuleNameValid = Module.ValidateWithPopup(moduleName, "Modulis");
                
                // Only proceed if validation passes
                // Tęsti tik jei validacija sėkminga
                if (isModuleNameValid)
                {
                    // Add module to database
                    // Pridėti modulį į duomenų bazę
                    InOutUtils.AddModule(moduleName);
                    
                    // Show success message
                    // Parodyti sėkmės pranešimą
                    MessageBox.Show("Modulis sėkmingai pridėtas!", "Pridėti modulį", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Reload the DataGrid to show the updated data
                    // Atnaujinti duomenų tinklelį, kad rodytų atnaujintus duomenis
                    Window_Loaded();
                    SwitchView(ViewMode.Default);
                }
            }
            catch (ArgumentException ex)
            {
                // This catch block is here for any validation exceptions that might not be caught by ValidateWithPopup
                // Šis catch blokas skirtas bet kokioms validacijos išimtims, kurių ValidateWithPopup negalėjo sugauti
                MessageBox.Show(ex.Message, "Validacijos klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the "Add Student" button click
        /// Apdoroja "Pridėti studentą" mygtuko paspaudimą
        /// </summary>
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear text boxes before showing panel
            // Išvalyti teksto laukus prieš rodant panelį
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            SwitchView(ViewMode.AddStudent);
        }

        /// <summary>
        /// Submits a new student to the database
        /// Išsaugo naują studentą duomenų bazėje
        /// </summary>
        private void SubmitNewStudent_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;

            // Basic validation for empty fields
            // Pagrindinė patikra dėl tuščių laukų
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Prašome užpildyti abu laukus: vardą ir pavardę.", 
                    "Įvesties klaida", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Let Student class validate the name and lastname
                // Leisti Student klasei validuoti vardą ir pavardę
                bool isFirstNameValid = Student.ValidateWithPopup(firstName, "Vardas");
                bool isLastNameValid = Student.ValidateWithPopup(lastName, "Pavardė");

                // Only proceed if both validations pass
                // Tęsti tik jei abi validacijos sėkmingos
                if (isFirstNameValid && isLastNameValid)
                {
                    // Add student to database
                    // Pridėti studentą į duomenų bazę
                    InOutUtils.AddStudent(firstName, lastName);
                    
                    // Show success message
                    // Parodyti sėkmės pranešimą
                    MessageBox.Show("Studentas sėkmingai pridėtas!", "Pridėti studentą", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Refresh the DataGrid and switch view
                    // Atnaujinti duomenų tinklelį ir pakeisti rodinį
                    Window_Loaded();
                    SwitchView(ViewMode.Default);
                }
            }
            catch (ArgumentException ex)
            {
                // This catch block is here for any validation exceptions that might not be caught by ValidateWithPopup
                // Šis catch blokas skirtas bet kokioms validacijos išimtims, kurių ValidateWithPopup negalėjo sugauti
                MessageBox.Show(ex.Message, "Validacijos klaida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the "Delete Student from Module" button click
        /// Apdoroja "Ištrinti studentą iš modulio" mygtuko paspaudimą
        /// </summary>
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // First switch to the view to ensure panel is visible
            SwitchView(ViewMode.DeleteStudentFromModule);
           
            try
            {
                // Populate the Module ComboBox
                // Užpildyti modulių išskleidžiamąjį sąrašą
                var modules = InOutUtils.GetModules();
                DeleteModuleComboBox.ItemsSource = modules;
                DeleteModuleComboBox.DisplayMemberPath = "ModuleName";
                DeleteModuleComboBox.SelectedValuePath = "Id";

                // Remove old handler before adding a new one to prevent duplicates
                DeleteModuleComboBox.SelectionChanged -= DeleteModuleComboBox_SelectionChanged;
                
                // Add handler for module selection change
                // Pridėti apdorojimo logiką modulio pasirinkimo pakeitimui
                DeleteModuleComboBox.SelectionChanged += DeleteModuleComboBox_SelectionChanged;

                // Initial population of student ComboBox
                // Pradinis studentų išskleidžiamojo sąrašo užpildymas
                DeleteStudentComboBox.ItemsSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing delete student form: {ex.Message}", "Form Error");
            }
        }

        /// <summary>
        /// Handles module selection change in the delete student from module panel
        /// Apdoroja modulio pasirinkimo pakeitimą "ištrinti studentą iš modulio" panelyje
        /// </summary>
        private void DeleteModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStudentComboBox();
        }

        /// <summary>
        /// Updates the student ComboBox based on the selected module
        /// Atnaujina studentų išskleidžiamąjį sąrašą pagal pasirinktą modulį
        /// </summary>
        private void UpdateStudentComboBox()
        {
            if (DeleteModuleComboBox.SelectedValue == null)
                return;

            try
            {
                var selectedModuleId = (int)DeleteModuleComboBox.SelectedValue;
            
                // Get students who have grades in the selected module
                // Gauti studentus, kurie turi pažymius pasirinktame modulyje
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student list: {ex.Message}", "Data Error");
            }
        }

        /// <summary>
        /// Handles the deletion of a student from a module
        /// Apdoroja studento pašalinimą iš modulio
        /// </summary>
        private void SubmitDeleteStudentFromModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = InOutUtils.HandleStudentModuleDeletion(
                    DeleteModuleComboBox.SelectedValue, 
                    DeleteStudentComboBox.SelectedValue
                );
            
                if (success)
                {
                    Window_Loaded();
                    SwitchView(ViewMode.Default);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student from module: {ex.Message}", "Delete Error");
            }
        }

        /// <summary>
        /// Handles the reset of the database
        /// Apdoroja duomenų bazės atstatymą
        /// </summary>
        private void DeleteDataBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ask for confirmation
                MessageBoxResult result = MessageBox.Show(
                    "Ar tikrai norite ištrinti visus duomenis? Šio veiksmo nebus galima atšaukti.",
                    "Patvirtinkite ištrynimą",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.Yes)
                {
                    DatabaseManager db = new DatabaseManager();
                    db.ResetDatabase();
                    Window_Loaded();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting database: {ex.Message}", "Database Error");
            }
        }

        /// <summary>
        /// Handles editing a student's grade
        /// Apdoroja studento pažymių redagavimą
        /// </summary>
        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First switch to the view to ensure panel is visible
                SwitchView(ViewMode.EditGrade);

                // Update the UI text to indicate editing a grade
                // Atnaujinti UI tekstą, nurodant, kad redaguojamas pažymys
                GradeActionTextBlock.Text = "Edit Grade";
                SubmitGradeButton.Content = "Update Grade";
            
                // Populate the Module ComboBox
                // Užpildyti modulių išskleidžiamąjį sąrašą
                var modules = InOutUtils.GetModules();
                EditModuleComboBox.ItemsSource = modules;
                EditModuleComboBox.DisplayMemberPath = "ModuleName";
                EditModuleComboBox.SelectedValuePath = "Id";

                // Clear previous selections
                // Išvalyti ankstesnius pasirinkimus
                EditStudentComboBox.ItemsSource = null;
                NewGradeTextBox.Text = string.Empty;
            
                // Remove old handler before adding a new one to prevent duplicates
                EditModuleComboBox.SelectionChanged -= EditModuleComboBox_SelectionChanged;
            
                // Add the selection changed event for filtering students
                // Pridėti pasirinkimo pakeitimo įvykį studentų filtravimui
                EditModuleComboBox.SelectionChanged += EditModuleComboBox_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing edit grade form: {ex.Message}", "Form Error");
            }
        }

        /// <summary>
        /// Handles module selection change in the edit grade panel
        /// Apdoroja modulio pasirinkimo pakeitimą pažymių redagavimo panelyje
        /// </summary>
        private void EditModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditModuleComboBox.SelectedValue == null)
                return;

            try
            {
                var selectedModuleId = (int)EditModuleComboBox.SelectedValue;
            
                // Get students who have grades in the selected module
                // Gauti studentus, kurie turi pažymius pasirinktame modulyje
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student list: {ex.Message}", "Data Error");
            }
        }

        /// <summary>
        /// Handles the submission of an edited grade
        /// Apdoroja redaguoto pažymio išsaugojimą
        /// </summary>
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

            try
            {
                var moduleId = (int)EditModuleComboBox.SelectedValue;
                var studentId = (int)EditStudentComboBox.SelectedValue;

                // Update the grade in the database
                // Atnaujinti pažymį duomenų bazėje
                bool success = _dbManager.UpdateGrade(moduleId, studentId, newGrade);

                if (success)
                {
                    MessageBox.Show("Pažymys sėkmingai atnaujintas!", "Sėkmė", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Window_Loaded(); // Refresh the data grid / Atnaujinti duomenų tinklelį
                    SwitchView(ViewMode.Default);
                }
                else
                {
                    MessageBox.Show("Nepavyko atnaujinti pažymio", "Klaida", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}", "Update Error");
            }
        }

        /// <summary>
        /// Handles adding a new grade
        /// Apdoroja naujo pažymio pridėjimą
        /// </summary>
        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First switch to the view to ensure panel is visible
                SwitchView(ViewMode.AddGrade);
            
                // Update the UI text to indicate adding a grade
                // Atnaujinti UI tekstą, nurodant, kad pridedamas pažymys
                GradeActionTextBlock.Text = "Add Grade";
                SubmitGradeButton.Content = "Add Grade";
            
                // Populate the Module ComboBox with all modules
                // Užpildyti modulių išskleidžiamąjį sąrašą visais moduliais
                var modules = InOutUtils.GetModules();
                EditModuleComboBox.ItemsSource = modules;
                EditModuleComboBox.DisplayMemberPath = "ModuleName";
                EditModuleComboBox.SelectedValuePath = "Id";
            
                // Populate the Student ComboBox with all students
                // Užpildyti studentų išskleidžiamąjį sąrašą visais studentais
                var students = InOutUtils.GetStudents();

                EditStudentComboBox.ItemsSource = students;
                EditStudentComboBox.DisplayMemberPath = "FullName";
                EditStudentComboBox.SelectedValuePath = "Id";
            
                // Clear the grade text box
                // Išvalyti pažymio teksto lauką
                NewGradeTextBox.Text = string.Empty;
            
                // Remove the selection changed event if it exists
                // Pašalinti pasirinkimo pakeitimo įvykį, jei jis egzistuoja
                EditModuleComboBox.SelectionChanged -= EditModuleComboBox_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing add grade form: {ex.Message}", "Form Error");
            }
        }

        /// <summary>
        /// Handles the submission of a grade (either new or edited)
        /// Apdoroja pažymio pateikimą (naujo arba redaguoto)
        /// </summary>
        private void SubmitGradeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GradeActionTextBlock.Text == "Add Grade")
                {
                    bool success = InOutUtils.HandleAddGrade(
                        EditModuleComboBox.SelectedValue,
                        EditStudentComboBox.SelectedValue,
                        NewGradeTextBox.Text
                    );
                
                    if (success)
                    {
                        Window_Loaded();
                        SwitchView(ViewMode.Default);
                    }
                }
                else // Edit Grade
                {
                    SubmitEditGrade_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting grade: {ex.Message}", "Submit Error");
            }
        }

        /// <summary>
        /// Handles the sort student button click
        /// </summary>
        private void SortStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show the sorting panel and ensure the DataGrid is visible
                Window_Loaded();
                SwitchView(ViewMode.Default);
                VisibilityManager.Show(SortingPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Klaida inicializuojant rūšiavimą: {ex.Message}", "Rūšiavimo klaida");
            }
        }

        /// <summary>
        /// Handles the selection change of the sort column combo box
        /// </summary>
        private void SortColumnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortColumnComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                _currentSortColumnIndex = Convert.ToInt32(selectedItem.Tag);
            }
        }

        /// <summary>
        /// Handles the radio button selection change for sort direction
        /// </summary>
        private void SortDirection_Changed(object sender, RoutedEventArgs e)
        {
            _sortAscending = SortAscendingRadio.IsChecked == true;
        }

        /// <summary>
        /// Handles the sort button click
        /// </summary>
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentSortColumnIndex == -1) // Special value for average
                {
                    // Sort by average grade
                    InOutUtils.SortStudentsByAverage(StudentGradesDataGrid, _sortAscending);
                }
                else if (_currentSortColumnIndex == 0)
                {
                    // Sort by student name
                    InOutUtils.SortStudentsByName(StudentGradesDataGrid, _sortAscending);
                }
                else
                {
                    // Sort by specific module grade
                    InOutUtils.SortStudentsByModuleGrade(StudentGradesDataGrid, _currentSortColumnIndex - 1, _sortAscending);
                }
                
                MessageBox.Show("Duomenys sėkmingai surūšiuoti!", "Rūšiavimas", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Klaida rūšiuojant duomenis: {ex.Message}", "Rūšiavimo klaida");
            }
        }

        /// <summary>
        /// Handles the selection change of the quick sort combo box
        /// </summary>
        private void QuickSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuickSortComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                _quickSortColumnIndex = Convert.ToInt32(selectedItem.Tag);
            }
        }

        /// <summary>
        /// Handles the radio button selection change for quick sort direction
        /// </summary>
        private void QuickSortDirection_Changed(object sender, RoutedEventArgs e)
        {
            _quickSortAscending = QuickSortAscending.IsChecked == true;
        }

        /// <summary>
        /// Handles the quick sort button click
        /// </summary>
        private void QuickSortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_quickSortColumnIndex == -1) // Special value for average
                {
                    // Sort by average grade
                    InOutUtils.SortStudentsByAverage(StudentGradesDataGrid, _quickSortAscending);
                }
                else if (_quickSortColumnIndex == 0)
                {
                    // Sort by student name
                    InOutUtils.SortStudentsByName(StudentGradesDataGrid, _quickSortAscending);
                }
                else
                {
                    // Sort by specific module grade
                    InOutUtils.SortStudentsByModuleGrade(StudentGradesDataGrid, _quickSortColumnIndex - 1, _quickSortAscending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Klaida rūšiuojant duomenis: {ex.Message}", "Rūšiavimo klaida");
            }
        }

        /// <summary>
        /// Handles changes in the last name textbox
        /// Apdoroja pavardės teksto lauko pakeitimus
        /// </summary>
        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Reserved for future implementation
            // Rezervuota būsimam įgyvendinimui
        }

        /// <summary>
        /// Handles text changes in the search textbox with debounce
        /// Apdoroja paieškos teksto lauko pakeitimus su delsa
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lock (_lockObject)
                {
                    // Reset the timer on each keystroke
                    // Atstatyti laikmatį su kiekvienu klavišo paspaudimu
                    _searchTimer?.Dispose();
                
                    // Create a new timer that waits 300ms before executing the search
                    // Sukurti naują laikmatį, kuris laukia 300ms prieš vykdant paiešką
                    _searchTimer = new System.Threading.Timer(_ => 
                    {
                        // We need to use the dispatcher to update UI from a different thread
                        // Turime naudoti dispatcher'į, kad atnaujintume UI iš kito gijo
                        Dispatcher.Invoke(() =>
                        {
                            string query = SearchBar_TextBox.Text;
                        
                            if (!string.IsNullOrEmpty(query))
                            {
                                // Use the matrix-based approach for displaying search results
                                // Naudoti matricomis pagrįstą metodą paieškos rezultatų rodymui
                                var filteredRows = InOutUtils.SearchStudentsInMatrix(query);
                                Console.WriteLine($"Search completed: {filteredRows.Count} results found");
                            
                                Debug.WriteLine($"Search completed: {filteredRows.Count} results found");
                                // Update the DataGrid with the filtered matrix data
                                // Atnaujinti duomenų tinklelį su filtruotais matricos duomenimis
                                StudentGradesDataGrid.ItemsSource = filteredRows;
                            }
                            else
                            {
                                // If the search box is empty, reload the full matrix
                                // Jei paieškos laukas tuščias, perkrauti pilną matricą
                                InOutUtils.LoadStudentGradesMatrix(StudentGradesDataGrid);
                            }
                        });
                    }, null, 300, System.Threading.Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Search error: {ex.Message}");
                // We don't show message box for search errors to avoid interrupting the user
            }
        }
        
        /// <summary>
        /// Handles the toggle button click for showing/hiding the search and sort panel
        /// </summary>
        private void ToggleSearchSortPanel_Click(object sender, RoutedEventArgs e)
        {
            _isSearchSortPanelVisible = !_isSearchSortPanelVisible;
            
            // Update panel visibility
            SearchSortPanel.Visibility = _isSearchSortPanelVisible ? Visibility.Visible : Visibility.Collapsed;
            
            // Update toggle button icon
            ToggleButtonIcon.Text = _isSearchSortPanelVisible ? "⮝" : "⮟";
            
            // Update tooltip
            ToggleSearchSortButton.ToolTip = _isSearchSortPanelVisible 
                ? "Slėpti paieškos ir rūšiavimo skydelį" 
                : "Rodyti paieškos ir rūšiavimo skydelį";
        }

        /// <summary>
        /// Generic button click handler
        /// Bendras mygtuko paspaudimo apdorojimas
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Reserved for future implementation
            // Rezervuota būsimam įgyvendinimui
        }

        /// <summary>
        /// Generic menu item click handler
        /// Bendras meniu elemento paspaudimo apdorojimas
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Reserved for future implementation
            // Rezervuota būsimam įgyvendinimui
        }

        /// <summary>
        /// Handles the search student button click
        /// Apdoroja studentų paieškos mygtuko paspaudimą
        /// </summary>
        private void SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window_Loaded();
                SwitchView(ViewMode.Default);
                
                // Show search sort panel and focus on search box
                SearchSortPanel.Visibility = Visibility.Visible;
                _isSearchSortPanelVisible = true;
                ToggleButtonIcon.Text = "⮝";
                SearchBar_TextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing search: {ex.Message}", "Search Error");
            }
        }

        /// <summary>
        /// Handles the delete student entirely button click
        /// Apdoroja visiško studento ištrynimo mygtuko paspaudimą
        /// </summary>
        private void DeleteStudentEntirely_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First switch to the view to ensure panel is visible
                SwitchView(ViewMode.DeleteStudent);

                // Populate the Student ComboBox with all students
                // Užpildyti studentų išskleidžiamąjį sąrašą visais studentais
                var students = InOutUtils.GetStudents();
                DeleteStudentEntirelyComboBox.ItemsSource = students;
                DeleteStudentEntirelyComboBox.DisplayMemberPath = "Name";
                DeleteStudentEntirelyComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing delete student form: {ex.Message}", "Form Error");
            }
        }

        /// <summary>
        /// Handles the submission of a student deletion request
        /// Apdoroja studento ištrynimo užklausos pateikimą
        /// </summary>
        private void SubmitDeleteStudentEntirely_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = InOutUtils.HandleStudentDeletion(
                    DeleteStudentEntirelyComboBox.SelectedValue
                );
            
                if (success)
                {
                    // Refresh the UI
                    // Atnaujinti vartotojo sąsają
                    Window_Loaded();
                    SwitchView(ViewMode.Default);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}", "Delete Error");
            }
        }
    }
}
