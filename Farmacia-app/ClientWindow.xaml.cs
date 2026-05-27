using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Farmacia_app
{
    public partial class ClientWindow : Window
    {
        private string _currentSearch = string.Empty;

        public ClientWindow()
        {
            InitializeComponent();
            AppState.DataChanged += RefreshGrid;
            RefreshGrid();
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            _currentSearch = SearchBox.Text.Trim();
            RefreshGrid();
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            _currentSearch = string.Empty;
            RefreshGrid();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = GetSelected();
            SelectedText.Text = selected == null
                ? string.Empty
                : $"{selected.Nume}";
        }

        private void OnBuy(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Selecteaza un medicament.");
                return;
            }

            if (!int.TryParse(QuantityBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity))
            {
                MessageBox.Show("Cantitate invalida.");
                return;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("Cantitatea trebuie sa fie pozitiva.");
                return;
            }

            if (quantity > selected.Stoc)
            {
                MessageBox.Show("Stoc insuficient.");
                return;
            }

            selected.Stoc -= quantity;
            MessageBox.Show("Cumparare efectuata.");
            QuantityBox.Clear();
            AppState.NotifyChanged();
        }

        private Medicament? GetSelected()
        {
            return MedicamenteGrid.SelectedItem as Medicament;
        }

        private void RefreshGrid()
        {
            var data = AppState.Medicamente.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(_currentSearch))
            {
                data = data.Where(m => m.Nume.Contains(_currentSearch, StringComparison.OrdinalIgnoreCase));
            }

            MedicamenteGrid.ItemsSource = data.ToList();
        }
    }
}
