using System;
using System.ComponentModel;

namespace Dienynas
{
    // Represents an assessment with a grade
    public class Assessment : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleID { get; set; }

        private double grade;
        public double Grade
        {
            get => grade;
            set { grade = value; OnPropertyChanged(nameof(Grade)); }
        }

        private DateTime date;
        public DateTime Date
        {
            get => date;
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }

        public Assessment(int id, string name, double grade)
        {
            Id = id;
            Name = name;
            Grade = grade;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}