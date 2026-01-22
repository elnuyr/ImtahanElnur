using System.ComponentModel.DataAnnotations;

namespace ImtahanElnur.Models
{
    public class Portfolio : BaseEntity
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string ImagePath { get; set; } = string.Empty;

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; } = null!;
    }
}
