using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salat.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public double ProteinPct { get; set; }

        [Range(0, 100)]
        public double FatPct { get; set; }

        [Range(0, 100)]
        public double CarbsPct { get; set; }

        [NotMapped]
        public double SumPct => ProteinPct + FatPct + CarbsPct;
    }
}
