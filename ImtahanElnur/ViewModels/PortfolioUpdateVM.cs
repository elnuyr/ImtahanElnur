using System.ComponentModel.DataAnnotations;

namespace ImtahanElnur.ViewModels
{
    public class PortfolioUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        public IFormFile? Image { get; set; }

        public int ProfessionId { get; set; }

    }
}
