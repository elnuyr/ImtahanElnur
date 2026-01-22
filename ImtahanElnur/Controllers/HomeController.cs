using System.Diagnostics;
using ImtahanElnur.Data;
using ImtahanElnur.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImtahanElnur.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var portfolios = await _context.Portfolios.Select(p => new PortfolioVM()
        {
            Id = p.Id,
            ImagePath = p.ImagePath,
            FullName = p.FullName,
            ProfessionName = p.Profession.Name


        }).ToListAsync();
        return View(portfolios);
       
    }

   
}
