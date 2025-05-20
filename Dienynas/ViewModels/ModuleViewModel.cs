using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Dienynas.ViewModels
{
    public class ModuleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Module> Modules { get; set; }

        public ModuleViewModel()
        {
            Modules = InOutUtils.GetModulesWithAssessments();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
