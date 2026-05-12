using System.Configuration;

namespace NivelStocareDate
{
    public static class StocareFactory
    {
        public static IStocareData GetStocare()
        {
            var formatSalvare = ConfigurationManager.AppSettings["FormatSalvare"];
            var numeFisier = ConfigurationManager.AppSettings["NumeFisier"] ?? "medicamente.txt";

            // Momentan suportam doar FisierText, se poate extinde pentru baze de date
            if (formatSalvare == "Txt")
            {
                return new AdministrareMedicamente_FisierText(numeFisier);
            }

            return new AdministrareMedicamente_FisierText(numeFisier);
        }
    }
}
