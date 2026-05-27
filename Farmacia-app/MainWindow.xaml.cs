using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Farmacia_app
{
    public partial class MainWindow : Window
    {
        private int _nextId = 1;
        private string _currentSearch = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            AppState.DataChanged += RefreshGrid;
            RefreshGrid();
        }

        private void OnOpenClient(object sender, RoutedEventArgs e)
        {
            var clientWindow = new ClientWindow();
            clientWindow.Show();
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            if (!TryReadInputs(out var name, out var price, out var stock))
            {
                return;
            }

            AppState.Medicamente.Add(new Medicament
            {
                Id = _nextId++,
                Nume = name,
                Pret = price,
                Stoc = stock
            });

            AppState.NotifyChanged();
            ClearInputs();
        }

        private void OnUpdate(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Selecteaza un medicament din lista.");
                return;
            }

            if (!TryReadInputs(out var name, out var price, out var stock))
            {
                return;
            }

            selected.Nume = name;
            selected.Pret = price;
            selected.Stoc = stock;

            AppState.NotifyChanged();
            ClearInputs();
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Selecteaza un medicament din lista.");
                return;
            }

            AppState.Medicamente.Remove(selected);
            AppState.NotifyChanged();
            ClearInputs();
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
            if (selected == null)
            {
                return;
            }

            NameBox.Text = selected.Nume;
            PriceBox.Text = selected.Pret.ToString(CultureInfo.InvariantCulture);
            StockBox.Text = selected.Stoc.ToString(CultureInfo.InvariantCulture);
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

            var list = data.ToList();
            MedicamenteGrid.ItemsSource = list;
            UpdateStockSummary(list);
        }

        private void UpdateStockSummary(System.Collections.Generic.List<Medicament> list)
        {
            var totalItems = list.Count;
            var totalStock = list.Sum(m => m.Stoc);
            StockSummaryText.Text = $"Total medicamente: {totalItems} | Stoc total: {totalStock}";
        }

        private bool TryReadInputs(out string name, out decimal price, out int stock)
        {
            name = NameBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Numele este obligatoriu.");
                price = 0;
                stock = 0;
                return false;
            }

            if (!decimal.TryParse(PriceBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out price))
            {
                MessageBox.Show("Pret invalid. Foloseste formatul cu punct pentru zecimale.");
                stock = 0;
                return false;
            }

            if (!int.TryParse(StockBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out stock))
            {
                MessageBox.Show("Stoc invalid.");
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            NameBox.Clear();
            PriceBox.Clear();
            StockBox.Clear();
        }
    }
}
