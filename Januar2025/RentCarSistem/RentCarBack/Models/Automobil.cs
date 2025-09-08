using System.ComponentModel.DataAnnotations;

namespace RentCarSistem.RentCarBack.Models
{
    public class Automobil
    {
        [Key]
        public int AutomobilID { get; set; }

        public required string Model { get; set; }

        public required int PredjenaKilometraza { get; set; }

        public required int BrojSedista { get; set; }

        public required int CenaPoDanu { get; set; }

        public required int Godiste { get; set; }

        public required bool JelIznajmljen { get; set; }
    }
}