using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public class Medicament : INotifyPropertyChanged
    {
        private int _idMedicament;
        private string _nume = string.Empty;
        private string _producator = string.Empty;
        private decimal _pret;
        private DateTime _dataExpirare = DateTime.Now;
        private CategorieMedicament _categorie;
        private OptiuniMedicament _optiuni;

        public int IdMedicament
        {
            get => _idMedicament;
            set { _idMedicament = value; OnPropertyChanged(); }
        }

        public string Nume
        {
            get => _nume;
            set { _nume = value; OnPropertyChanged(); }
        }

        public string Producator
        {
            get => _producator;
            set { _producator = value; OnPropertyChanged(); }
        }

        public decimal Pret
        {
            get => _pret;
            set { _pret = value; OnPropertyChanged(); }
        }

        public DateTime DataExpirare
        {
            get => _dataExpirare;
            set { _dataExpirare = value; OnPropertyChanged(); }
        }

        public CategorieMedicament Categorie
        {
            get => _categorie;
            set { _categorie = value; OnPropertyChanged(); }
        }

        public OptiuniMedicament Optiuni
        {
            get => _optiuni;
            set { _optiuni = value; OnPropertyChanged(); }
        }

        public Medicament() { }

        // Constructor care parseaza o linie din fisier
        public Medicament(string linieDinFisier)
        {
            var date = linieDinFisier.Split(';');
            if (date.Length >= 7)
            {
                IdMedicament = int.Parse(date[0]);
                Nume = date[1];
                Producator = date[2];
                // Handling decimal parsing variations if needed, keeping simple
                Pret = decimal.Parse(date[3]);
                DataExpirare = DateTime.Parse(date[4]);
                Categorie = (CategorieMedicament)Enum.Parse(typeof(CategorieMedicament), date[5]);
                Optiuni = (OptiuniMedicament)Enum.Parse(typeof(OptiuniMedicament), date[6]);
            }
        }

        // Metoda ceruta pentru conversia la sir
        public string ConversieLaSirPentruFisier()
        {
            return $"{IdMedicament};{Nume};{Producator};{Pret};{DataExpirare:O};{Categorie};{Optiuni}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
