using HotelDomaci.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelDomaci.Models.ViewModel
{
    public class Filter
    {
        public List<Apartman> Apartmani { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }
    }
}
