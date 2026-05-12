using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NivelStocareDate
{
    public class AdministrareMedicamente_FisierText : IStocareData
    {
        private readonly string fileName;

        public AdministrareMedicamente_FisierText(string fileName)
        {
            this.fileName = fileName;
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
        }

        public void AddMedicament(LibrarieModele.Medicament m)
        {
            // Asignare ID simplist
            var toate = GetMedicamente();
            m.IdMedicament = toate.Any() ? toate.Max(x => x.IdMedicament) + 1 : 1;

            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(m.ConversieLaSirPentruFisier());
            }
        }

        public List<LibrarieModele.Medicament> GetMedicamente()
        {
            List<LibrarieModele.Medicament> medicamente = new List<LibrarieModele.Medicament>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        medicamente.Add(new LibrarieModele.Medicament(line));
                    }
                }
            }
            return medicamente;
        }

        public void UpdateMedicament(LibrarieModele.Medicament m)
        {
            var medicamente = GetMedicamente();
            var index = medicamente.FindIndex(x => x.IdMedicament == m.IdMedicament);
            if (index != -1)
            {
                medicamente[index] = m;
                // Rescriem fisierul
                using (StreamWriter sw = new StreamWriter(fileName, false))
   
