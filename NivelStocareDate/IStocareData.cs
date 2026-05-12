using LibrarieModele;
using System.Collections.Generic;

namespace NivelStocareDate
{
    public interface IStocareData
    {
        void AddMedicament(Medicament m);
        List<Medicament> GetMedicamente();
        void UpdateMedicament(Medicament m);
    }
}
