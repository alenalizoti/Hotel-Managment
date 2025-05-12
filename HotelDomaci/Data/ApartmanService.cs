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

    }
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ApartmaniCollection { get; set; }
    }
}