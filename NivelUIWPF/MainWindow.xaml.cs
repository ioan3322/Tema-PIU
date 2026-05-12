using LibrarieModele;
using NivelStocareDate;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    public partial class MainWindow : Window
    {
        private readonly IStocareData stocare;
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;

            cmbCategorie.ItemsSource = Enum.GetValues(typeof(CategorieMedicament));
            cmbCategorie.SelectedIndex = 0;

            stocare = StocareFactory.GetStocare();
            IncarcaMedicamente();
        }

        private void IncarcaMedicamente()
        {
            viewModel.MedicamenteList.Clear();
            foreach (var medicament in stocare.GetMedicamente())
            {
                viewModel.MedicamenteList.Add(medicament);
            }
        }

        private void BtnAdministrare_Click(object sender, RoutedEventArgs e)
        {
            PanelAdministrare.Visibility = Visibility.Visible;
            PanelCautare.Visibility = Visibility.Collapsed;
        }

        private void BtnCautare_Click(object sender, RoutedEventArgs e)
        {
            PanelAdministrare.Visibility = Visibility.Collapsed;
            PanelCautare.Visibility = Visibility.Visible;
            ActualizeazaRezultateCautare();
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!TryActualizeazaMedicamentDinFormular())
            {
                return;
            }

            stocare.AddMedicament(viewModel.CurrentMedicament);
            viewModel.MedicamenteList.Add(viewModel.CurrentMedicament);
            ReseteazaFormular();
        }

        private void BtnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentMedicament.IdMedicament == 0)
            {
                return;
            }

            if (!TryActualizeazaMedicamentDinFormular())
            {
                return;
            }

            stocare.UpdateMedicament(viewModel.CurrentMedicament);
            dgMedicamente.Items.Refresh();
        }

        private void BtnCurata_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaFormular();
        }

        private void DgMedicamente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMedicamente.SelectedItem is Medicament medicament)
            {
                viewModel.CurrentMedicament = medicament;
                viewModel.PretText = medicament.Pret.ToString("0.##");
                cmbCategorie.SelectedItem = medicament.Categorie;
                dpDataExpirare.SelectedDate = medicament.DataExpirare;
                SeteazaOptiuniUi(medicament.Optiuni);
            }
        }

        private void TxtTermenCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizeazaRezultateCautare();
        }

        private void ActualizeazaRezultateCautare()
        {
            var termen = txtTermenCautare.Text ?? string.Empty;
            var rezultate = viewModel.MedicamenteList
                .Where(m => m.Nume.Contains(termen, StringComparison.OrdinalIgnoreCase)
                         || m.Producator.Contains(termen, StringComparison.OrdinalIgnoreCase))
                .ToList();

            dgRezultateCautare.ItemsSource = rezultate;
        }

        private bool TryActualizeazaMedicamentDinFormular()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(viewModel.CurrentMedicament.Nume))
            {
                lblNume.Foreground = Brushes.Red;
                valid = false;
            }
            else
            {
                lblNume.Foreground = Brushes.Black;
            }

            if (!decimal.TryParse(viewModel.PretText, out var pret))
            {
                lblPret.Foreground = Brushes.Red;
                valid = false;
            }
            else
            {
                lblPret.Foreground = Brushes.Black;
                viewModel.CurrentMedicament.Pret = pret;
            }

            if (!valid)
            {
                return false;
            }

            viewModel.CurrentMedicament.Categorie = (CategorieMedicament)cmbCategorie.SelectedItem;
            viewModel.CurrentMedicament.DataExpirare = dpDataExpirare.SelectedDate ?? DateTime.Today;
            viewModel.CurrentMedicament.Optiuni = GetOptiuniDinUi();
            return true;
        }

        private OptiuniMedicament GetOptiuniDinUi()
        {
            OptiuniMedicament optiuni = OptiuniMedicament.Niciuna;

            if (rbReteta.IsChecked == true)
            {
                optiuni |= OptiuniMedicament.RetetaNecesara;
            }

            if (chkCompensat.IsChecked == true)
            {
                optiuni |= OptiuniMedicament.Compensat;
            }

            if (chkGeneric.IsChecked == true)
            {
                optiuni |= OptiuniMedicament.Generic;
            }

            return optiuni;
        }

        private void SeteazaOptiuniUi(OptiuniMedicament optiuni)
        {
            rbReteta.IsChecked = optiuni.HasFlag(OptiuniMedicament.RetetaNecesara);
            rbFaraReteta.IsChecked = !rbReteta.IsChecked.GetValueOrDefault();
            chkCompensat.IsChecked = optiuni.HasFlag(OptiuniMedicament.Compensat);
            chkGeneric.IsChecked = optiuni.HasFlag(OptiuniMedicament.Generic);
        }

        private void ReseteazaFormular()
        {
            viewModel.ResetCurrentMedicament();
            cmbCategorie.SelectedIndex = 0;
            dpDataExpirare.SelectedDate = viewModel.CurrentMedicament.DataExpirare;
            rbFaraReteta.IsChecked = true;
            chkCompensat.IsChecked = false;
            chkGeneric.IsChecked = false;
            lblNume.Foreground = Brushes.Black;
            lblPret.Foreground = Brushes.Black;
            dgMedicamente.SelectedItem = null;
        }
    }
}
