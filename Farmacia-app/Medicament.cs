namespace Farmacia_app
{
    internal sealed class Medicament
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public decimal Pret { get; set; }
        public int Stoc { get; set; }
    }
}
