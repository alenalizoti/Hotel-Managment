using HotelDomaci.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelDomaci.Controllers
{
    public class ApartmanController : Controller
    {
        private readonly ApartmanService _apartmanService;

        public ApartmanController(ApartmanService apartmanService)
        {
            _apartmanService = apartmanService;
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
    }
}
