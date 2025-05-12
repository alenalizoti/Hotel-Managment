using HotelDomaci.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelDomaci.Data
{
    public class ApartmanService
    {
        private readonly IMongoCollection<Apartman> _apartmani;

        public ApartmanService(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _apartmani = database.GetCollection<Apartman>(mongoSettings.Value.ApartmaniCollection);
        }

        public async Task<List<Apartman>> GetAsync()
        {
            var result = await _apartmani.FindAsync(ap => true);
            return await result.ToListAsync();
        }
        public async Task<Apartman?> GetAsync(string id)
        {
            var result = await _apartmani.FindAsync(ap => ap.Id == id);
            return await result.FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Apartman ap)
        {
            await _apartmani.InsertOneAsync(ap);
        }

        public async Task UpdateAsync(string id, Apartman ap)
        {
            await _apartmani.ReplaceOneAsync(a => a.Id == id, ap);
        }

        public async Task DeleteAsync(string id)
        {
            await _apartmani.DeleteOneAsync(a => a.Id == id);
        }
        public async Task UbaciTestApartmane()
        {
            if (_apartmani.Find(ap => true).Any())
            {
                return;
            }

            var apartmani = new List<Apartman>
            {
                new Apartman
                {
                    NazivApartmana = "Sunčani Apartman",
                    Drzava = "Srbija",
                    Grad = "Beograd",
                    UdaljenostOdCentra = 2.5,
                    CenaPoNocenju = 45,
                    OpisApartmana = "Moderno opremljen apartman u centru.",
                    BrojMesta = 2,
                    ServisneUsluge = new ServisneUsluge
                    {
                        Klima = true,
                        Wifi = true,
                        Frizider = false,
                        Sef = true
                    },
                    Slike = new List<string> { "slika1.jpg", "slika3.jpg" }
                },
                new Apartman
                {
                    NazivApartmana = "Planinski Raj",
                    Drzava = "Srbija",
                    Grad = "Zlatibor",
                    UdaljenostOdCentra = 1.0,
                    CenaPoNocenju = 60,
                    OpisApartmana = "Prelep pogled na planine.",
                    BrojMesta = 4,
                    ServisneUsluge = new ServisneUsluge
                    {
                        Klima = true,
                        Wifi = true,
                        Frizider = false,
                        Sef = true
                    },
                    Slike = new List<string> { "slika3.jpg" }
                },
                new Apartman
                {
                    NazivApartmana = "Apartman Lux",
                    Drzava = "Hrvatska",
                    Grad = "Split",
                    UdaljenostOdCentra = 0.5,
                    CenaPoNocenju = 80,
                    OpisApartmana = "Luksuzan apartman na obali.",
                    BrojMesta = 3,
                    ServisneUsluge = new ServisneUsluge
                    {
                        Klima = true,
                        Wifi = true,
                        Frizider = false,
                        Sef = true
                    },
                    Slike = new List<string> { "slika4.jpg" }
                }
            };

            await _apartmani.InsertManyAsync(apartmani);
        }
    }
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ApartmaniCollection { get; set; }
    }
}