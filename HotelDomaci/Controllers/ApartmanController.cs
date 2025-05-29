using HotelDomaci.Data;
using HotelDomaci.Models;
using HotelDomaci.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HotelDomaci.Controllers
{
    public class ApartmanController : Controller
    {
        private readonly ApartmanService _apartmanService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApartmanController(ApartmanService apartmanService, IWebHostEnvironment webHostEnvironment)
        {
            _apartmanService = apartmanService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> DodajTestApartmane()
        {
            await _apartmanService.UbaciTestApartmane();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index(string? SearchTerm, string? SortOrder)
        {
            var apartmani = await _apartmanService.GetAsync();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                apartmani = apartmani
                 .Where(a => !string.IsNullOrEmpty(a.NazivApartmana) && a.NazivApartmana.Contains(SearchTerm))
                 .ToList();
            }
            apartmani = SortOrder switch
            {
                "naziv_desc" => apartmani.OrderByDescending(a => a.NazivApartmana).ToList(),
                "cena_asc" => apartmani.OrderBy(a => a.CenaPoNocenju).ToList(),
                "cena_desc" => apartmani.OrderByDescending(a => a.CenaPoNocenju).ToList(),
                _ => apartmani.OrderBy(a => a.NazivApartmana).ToList()
            };

            var viewModel = new Filter
            {
                SortOrder = SortOrder,
                SearchTerm = SearchTerm,
                Apartmani = apartmani
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Apartman model, List<IFormFile> NoveSlike)
        {
            model.Slike ??= new List<string>();


            var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            if (NoveSlike != null && NoveSlike.Any())
            {
                foreach (var slika in NoveSlike)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(slika.FileName);
                    var fullPath = Path.Combine(imagesFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await slika.CopyToAsync(stream);
                    }

                    model.Slike.Add(fileName);
                }
            }

            await _apartmanService.CreateAsync(model);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(string id)
        {
            var apartman = await _apartmanService.GetAsync(id);
            if (apartman == null) return NotFound();
            return View(apartman);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var apartman = await _apartmanService.GetAsync(id);
            if (apartman == null) return NotFound();

            await _apartmanService.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var apartman = await _apartmanService.GetAsync(id);
            if (apartman == null)
                return NotFound();

            return View(apartman);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Apartman apartmanIzForme, List<string> SlikeZaBrisanje, List<IFormFile> NoveSlike)
        {
            var apartman = await _apartmanService.GetAsync(id);
            if (apartman == null)
            {
                return NotFound();
            }
            apartman.NazivApartmana = apartmanIzForme.NazivApartmana;
            apartman.OpisApartmana = apartmanIzForme.OpisApartmana;
            apartman.Drzava = apartmanIzForme.Drzava;
            apartman.Grad = apartmanIzForme.Grad;
            apartman.UdaljenostOdCentra = apartmanIzForme.UdaljenostOdCentra;
            apartman.BrojMesta = apartmanIzForme.BrojMesta;
            apartman.CenaPoNocenju = apartmanIzForme.CenaPoNocenju;
            apartman.ServisneUsluge = apartmanIzForme.ServisneUsluge;

            if (SlikeZaBrisanje != null && SlikeZaBrisanje.Any())
            {
                apartman.Slike = apartman.Slike.Where(s => !SlikeZaBrisanje.Contains(s)).ToList();

                foreach (var slikaZaBrisanje in SlikeZaBrisanje)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", slikaZaBrisanje);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            if (NoveSlike != null && NoveSlike.Any())
            {
                foreach (var slika in NoveSlike)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(slika.FileName);
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await slika.CopyToAsync(stream);
                    }

                    apartman.Slike.Add(fileName);
                }
            }
            await _apartmanService.UpdateAsync(id, apartman);

            return RedirectToAction("Index");
        }

    }
}
