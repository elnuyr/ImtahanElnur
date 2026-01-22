using ImtahanElnur.Models;
using System.ComponentModel.DataAnnotations;

namespace ImtahanElnur.ViewModels
{
    public class PortfolioCreateVM
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public IFormFile? Image { get; set; } 

        public int ProfessionId { get; set; }
       
    }
}
