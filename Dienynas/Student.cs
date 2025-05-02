using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Dienynas
{
    /// <summary>
    /// Studento objekto klasė
    /// </summary>
    public class Student
    {
        private string _name;
        private string _lastname;
        
        public int Id { get; set; }
        
        public string Name
        {
            get => _name;
            set
            {
                    ValidateName(value, "Vardas");
                _name = value;
            }
        }
        
        public string Lastname
        {
            get => _lastname;
            set
            {
                ValidateName(value, "Pavardė");
                _lastname = value;
            }
        }

        /// <summary>
        /// Studento konstruktorius
        /// </summary>
        public Student(int id, string name, string lastname)
        {
            Id = id;
            Name = name; // Will validate through property setter
            Lastname = lastname; // Will validate through property setter
        }
        
        public Student()
        {
            // Tuscias konstruktorius dėl duomenų užpildymo
        }

        public Student(string name, string lastname)
        {
            Name = name; // Will validate through property setter
            Lastname = lastname; // Will validate through property setter
        }

        
        
        public string FullName => $"{Name} {Lastname}";


        /// <summary>
        /// Patikrina, ar vardas/pavardė yra teisingi
        /// </summary>
        /// <param name="name">Vardas arba pavardė, kurią reikia patikrinti</param>
        /// <returns>true jei vardas/pavardė teisingi, kitaip - false</returns>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
                
            // Pattern for valid Lithuanian names - allows letters (including Lithuanian characters),
            // apostrophes, hyphens and spaces
            // This regex allows international characters as well
            string pattern = @"^[\p{L}\s'\-]+$";
            
            // Check if name matches the pattern
            if (!Regex.IsMatch(name, pattern))
                return false;
            
            // Check if name starts with an uppercase letter
            if (!char.IsUpper(name[0]))
                return false;
                
            // Check if name does not contain consecutive spaces, hyphens, or apostrophes
            if (name.Contains("  ") || name.Contains("--") || name.Contains("''"))
                return false;
                
            // Ensure the name is not too long or too short
            if (name.Length < 2 || name.Length > 50)
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Patikrina duomenis ir parodo popup su klaidos pranešimu jei reikia
        /// </summary>
        /// <param name="value">Tikrinama reikšmė</param>
        /// <param name="fieldName">Lauko pavadinimas (Vardas/Pavardė)</param>
        /// <returns>true jei reikšmė teisinga, kitaip - false</returns>
        public static bool ValidateWithPopup(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show($"{fieldName} negali būti tuščias.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (!Regex.IsMatch(value, @"^[\p{L}\s'\-]+$"))
            {
                MessageBox.Show($"{fieldName} gali turėti tik raides, tarpus, apostrofus ir brūkšnelius.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (!char.IsUpper(value[0]))
            {
                MessageBox.Show($"{fieldName} turi prasidėti didžiąja raide.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            string[] words = value.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    if (!char.IsUpper(word[0]))
                    {
                        MessageBox.Show($"Kiekvienas žodis {fieldName} turi prasidėti didžiąja raide.", 
                            "Neteisingi duomenys", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                        return false;
                    }
                    
                    if (word.Length > 1 && word.Substring(1).Any(c => char.IsUpper(c)))
                    {
                        MessageBox.Show($"Po pirmosios raidės, likusios raidės {fieldName} turi būti mažosios.", 
                            "Neteisingi duomenys", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            
            if (value.Contains("  ") || value.Contains("--") || value.Contains("''"))
            {
                MessageBox.Show($"{fieldName} negali turėti kelių tarpų, brūkšnelių ar apostrofų iš eilės.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (value.Count(c => c == ' ') > 2)
            {
                MessageBox.Show($"{fieldName} negali turėti daugiau nei dviejų tarpų.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (value.Length < 2 || value.Length > 19)
            {
                MessageBox.Show($"{fieldName} turi būti nuo 2 iki 50 simbolių ilgio.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Patikrina, ar vardo ir pavardės formatai teisingi
        /// </summary>
        /// <returns>true jei vardas ir pavardė teisingi, kitaip - false</returns>
        public bool IsValid()
        {
            return IsValidName(Name) && IsValidName(Lastname);
        }
        
        /// <summary>
        /// Gauna pilną studento vardą
        /// </summary>
        /// <returns>Studento pilnas vardas (formatas: Vardas Pavardė)</returns>
        public string GetFullName()
        {
            return $"{Name} {Lastname}";
        }

        private void ValidateName(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} negali būti tuščias.");
                
            if (value.Length < 2 || value.Length > 50)
                throw new ArgumentException($"{fieldName} turi būti nuo 2 iki 50 simbolių ilgio.");
                
            if (!Regex.IsMatch(value, @"^[\p{L}\s'\-]+$"))
                throw new ArgumentException($"{fieldName} gali turėti tik raides, tarpus, apostrofus ir brūkšnelius.");
            
            // Add other validations from IsValidName with specific error messages
        }
    }
}
