using LibrarieModele;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NivelUIWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Medicament _currentMedicament;
        private string _pretText = string.Empty;

        public ObservableCollection<Medicament> MedicamenteList { get; } = new();

        public Medicament CurrentMedicament
        {
            get => _currentMedicament;
            set
            {
                _currentMedicament = value;
                OnPropertyChanged();
            }
        }

        public string PretText
        {
            get => _pretText;
            set
            {
                _pretText = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _currentMedicament = new Medicament { DataExpirare = DateTime.Today };
        }

        public void ResetCurrentMedicament()
        {
            CurrentMedicament = new Medicament { DataExpirare = DateTime.Today };
            PretText = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
