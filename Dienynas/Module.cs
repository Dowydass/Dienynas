        using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Dienynas
{
    //  Modulio objekto klase
    public class Module
    {
        private string _moduleName;
        
        public int Id { get; set; }
        
        public string ModuleName
        {
            get => _moduleName;
            set
            {
                if (!ValidateWithPopup(value, "Modulis"))
                    throw new ArgumentException("Modulio pavadinimas yra neteisingas. Pavadinimas turi prasidėti didžiąja raide ir gali turėti tik raides, skaičius, tarpus ir specialius simbolius (pvz., -, .).");
                _moduleName = value;
            }
        }

        /// Modulio konstruktorius
        public Module(int id, string moduleName)
        {
            Id = id;
            try
            {
                ModuleName = moduleName; // Will validate through property setter
            }
            catch (ArgumentException ex)
            {
                // Exception will be shown through popup in ValidateWithPopup method
                throw;
            }
        }
        
        public Module()
        {
            // Tuscias konstruktorius dėl duomenų užpildymo
        }
        
        /// <summary>
        /// Patikrina, ar modulio pavadinimas yra teisingas
        /// </summary>
        /// <param name="name">Modulio pavadinimas, kurį reikia patikrinti</param>
        /// <returns>true jei pavadinimas teisingas, kitaip - false</returns>
        public static bool IsValidModuleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
                
            // Pattern for valid module names - allows letters, digits, spaces, hyphens and periods
            string pattern = @"^[\p{L}\d\s\-\.]+$";
            
            // Check if name matches the pattern
            if (!Regex.IsMatch(name, pattern))
                return false;
            
            // Check if name starts with an uppercase letter
            if (!char.IsUpper(name[0]))
                return false;
                
            // Check if name does not contain consecutive spaces, hyphens, or periods
            if (name.Contains("  ") || name.Contains("--") || name.Contains(".."))
                return false;
                
            // Ensure the name is not too long or too short
            if (name.Length < 2 || name.Length > 100)
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Patikrina modulio pavadinimą ir parodo popup su klaidos pranešimu jei reikia
        /// </summary>
        /// <param name="value">Tikrinama reikšmė</param>
        /// <param name="fieldName">Lauko pavadinimas</param>
        /// <returns>true jei reikšmė teisinga, kitaip - false</returns>
        public static bool ValidateWithPopup(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show($"{fieldName} pavadinimas negali būti tuščias.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (!Regex.IsMatch(value, @"^[\p{L}\d\s\-\.]+$"))
            {
                MessageBox.Show($"{fieldName} pavadinime leidžiami tik raidės, skaičiai, tarpai, brūkšneliai ir taškai.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (!char.IsUpper(value[0]))
            {
                MessageBox.Show($"{fieldName} pavadinimas turi prasidėti didžiąja raide.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (value.Contains("  ") || value.Contains("--") || value.Contains(".."))
            {
                MessageBox.Show($"{fieldName} pavadinime negali būti kelių tarpų, brūkšnelių ar taškų iš eilės.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            if (value.Length < 2 || value.Length > 60)
            {
                MessageBox.Show($"{fieldName} pavadinimas turi būti nuo 2 iki 60 simbolių ilgio.", 
                    "Neteisingi duomenys", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            
            return true;
        }
    }
}
