using System;
using System.Windows;

namespace Dienynas
{
    public partial class AddAssessmentWindow : Window
    {
        public double Grade { get; private set; }
        public DateTime AssessmentDate { get; private set; }

        public AddAssessmentWindow()
        {
            InitializeComponent();
            DatePicker.SelectedDate = DateTime.Now;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(GradeTextBox.Text, out double grade) && DatePicker.SelectedDate.HasValue)
            {
                Grade = grade;
                AssessmentDate = DatePicker.SelectedDate.Value;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Įveskite teisingą balą ir datą.", "Klaida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveGrade_Click(object sender, RoutedEventArgs e)
        {
            // Assume you have studentId, moduleId, and grade from UI controls
            int studentId = /* get from UI */;
            int moduleId = /* get from UI */;
            int grade = /* get from UI */;
            var db = new DatabaseManager();
            db.AddOrUpdateGrade(studentId, moduleId, grade);
            MainWindow.ShowMessage("Pažymys išsaugotas.", "Sėkmingai");
            this.Close();
        }
    }
}
