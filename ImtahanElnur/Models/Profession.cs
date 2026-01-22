using System.ComponentModel.DataAnnotations;

namespace ImtahanElnur.Models
{
    public class Profession : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Portfolio> Portfolios { get; set; } = [];
    }
}
