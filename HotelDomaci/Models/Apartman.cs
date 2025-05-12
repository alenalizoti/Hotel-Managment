using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelDomaci.Models
{
    public class Apartman
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("NazivApartmana")]
        public string NazivApartmana { get; set; }

        [BsonElement("Drzava")]
        public string Drzava { get; set; }

        [BsonElement("Grad")]
        public string Grad { get; set; }

        [BsonElement("UdaljenostOdCentra")]
        public double UdaljenostOdCentra { get; set; }

        [BsonElement("CenaPoNocenju")]
        public decimal CenaPoNocenju { get; set; }

        [BsonElement("OpisApartmana")]
        public string OpisApartmana { get; set; }

        [BsonElement("ServisneUsluge")]
        public ServisneUsluge ServisneUsluge { get; set; }

        [BsonElement("BrojMesta")]
        public int BrojMesta { get; set; }

        [BsonElement("Slike")]
        public List<string> Slike { get; set; } = new List<string>();
    }
    public class ServisneUsluge
    {
        public bool Klima { get; set; }
        public bool Wifi { get; set; }
        public bool Frizider { get; set; }
        public bool Sef { get; set; }
    }
}
