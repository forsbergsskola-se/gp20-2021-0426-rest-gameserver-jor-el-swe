using System.Threading.Tasks;
using MongoDB.Driver;

namespace LameScooter {
    public class MongoDBLameScooterRental: ILameScooterRental {
        public async Task<int> GetScooterCountInStation(string stationName) {
            var client = new MongoClient();
            
            //Mongo-Collection named lamescooters.
            return 0;
        }
    }
}