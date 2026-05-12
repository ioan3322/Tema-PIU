using System;
using System;

namespace LibrarieModele
{
    public enum CategorieMedicament
    {
        Analgezic,
        Antibiotic,
        Antiinflamator,
        Supliment,
        Diverse
    }

    [Flags]
    public enum OptiuniMedicament
    {
        Niciuna = 0,
        RetetaNecesara = 1,
        Compensat = 2,
        Generic = 4
    }
}
