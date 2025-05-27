using HotelDomaci.Data;
using HotelDomaci.Models;
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
        public async Task<IActionResult> Index()
        {
            var apartmani = await _apartmanService.GetAsync();
            return View(apartmani);
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

    }
}
