using System;
using System.Collections.ObjectModel;

namespace Farmacia_app
{
    internal static class AppState
    {
        public static ObservableCollection<Medicament> Medicamente { get; } = new();

        public static event Action? DataChanged;

        public static void NotifyChanged()
        {
            DataChanged?.Invoke();
        }
    }
}
