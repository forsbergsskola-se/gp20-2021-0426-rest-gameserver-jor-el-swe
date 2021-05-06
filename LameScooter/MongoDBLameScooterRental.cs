using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace LameScooter {
    public class MongoDBLameScooterRental: ILameScooterRental {
        public async Task<int> GetScooterCountInStation(string stationName) {
            
            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException("the station name must not contain a number");
            }
            
            var mongoClient = new MongoClient();
            var database = mongoClient.GetDatabase("lamescooters");
            var collection = database.GetCollection<BsonDocument>("lamescooters");
            AddConventionPacks();
            var filter = Builders<BsonDocument>.Filter.Eq("name", stationName);
            var document = collection.Find(filter).First();
            var result = BsonSerializer.Deserialize<LameScooterStationList>(document);
            return result.BikesAvailable;
        }

        void AddConventionPacks() {
            var conventionPack = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            
            conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
        }
    }
}