using ImtahanElnur.Data;
using ImtahanElnur.Models;
using ImtahanElnur.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ImtahanElnur.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public PortfolioController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath,"assets/images");
        }

        public async Task<IActionResult> Index()
        {
            var portfolios =await _context.Portfolios.Select(p=> new PortfolioVM()
            {
                Id = p.Id,
                ImagePath = p.ImagePath,
                FullName = p.FullName,
                ProfessionName = p.Profession.Name


            }).ToListAsync();
            return View(portfolios);
        }

    

        public async Task<IActionResult> Create()
        {
            await SendWithViewBag();
            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Create(PortfolioCreateVM vm)
        {
            await SendWithViewBag();
            if (!ModelState.IsValid)
            
                return View(vm);

             var isExistProfession = await _context.Professions.AnyAsync(p => p.Id == vm.ProfessionId);

          

            if (!isExistProfession)
            {
                ModelState.AddModelError("ProfessionId", "This Profession is not found");
                return View(vm);
            }

            if (vm.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Faylin maximum olcusu 2 mb olmalidir");
                return View(vm);
            }




            if (!vm.Image.ContentType.Contains("image"))
            {

                ModelState.AddModelError("Image", "Yalniz sekil formatinda data yukleyin");
                return View(vm);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;


            string path = Path.Combine(_folderPath, uniqueFileName);

            using FileStream stream = new(path, FileMode.Create);

            await vm.Image.CopyToAsync(stream);



            Portfolio portfolio = new()
            {
                FullName = vm.FullName,
                ImagePath = uniqueFileName,
                ProfessionId = vm.ProfessionId
            };


            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");



        }

        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null) return NotFound();
            string path = Path.Combine(_folderPath, portfolio.ImagePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var portfolio = await _context.Portfolios.Include(p => p.Profession).FirstOrDefaultAsync(p => p.Id == id);
            if (portfolio == null) return NotFound();
            PortfolioDetailVM vm = new()
            {
                Id = portfolio.Id,
                FullName = portfolio.FullName,
                ImagePath = portfolio.ImagePath,
                ProfessionName = portfolio.Profession.Name
            };
            return View(vm);
        }

        public async Task<IActionResult> Update(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null) return NotFound();
            await SendWithViewBag();
            PortfolioUpdateVM vm = new()
            {
                Id = portfolio.Id,
                FullName = portfolio.FullName,
                ProfessionId = portfolio.ProfessionId,
                
            };
            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(PortfolioUpdateVM vm)
        {
            await SendWithViewBag();
            if (!ModelState.IsValid)
                return View(vm);
            var portfolio = await _context.Portfolios.FindAsync(vm.Id);
            if (portfolio == null) return NotFound();
            var isExistProfession = await _context.Professions.AnyAsync(p => p.Id == vm.ProfessionId);
            if (!isExistProfession)
            {
                ModelState.AddModelError("ProfessionId", "This Profession is not found");
                return View(vm);
            }
            if (vm.Image != null)
            {
                if (vm.Image.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Faylin maximum olcusu 2 mb olmalidir");
                    return View(vm);
                }
                if (!vm.Image.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("Image", "Yalniz sekil formatinda data yukleyin");
                    return View(vm);
                }
            }
            if (vm.Image != null)
            {
                string oldPath = Path.Combine(_folderPath, portfolio.ImagePath);
                if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
                
                string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;

                string newPath = Path.Combine(_folderPath, uniqueFileName);

                using FileStream stream = new(newPath, FileMode.Create);

                await vm.Image.CopyToAsync(stream);

                portfolio.ImagePath = uniqueFileName;
            }
        
            portfolio.FullName = vm.FullName;
            portfolio.ProfessionId = vm.ProfessionId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }





        private async Task SendWithViewBag()
        {
            var portfolios = await _context.Professions.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToListAsync();
            ViewBag.Professions = portfolios;
        }

    }
}
