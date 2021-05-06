using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LameScooter
{
    public class OfflineLameScooterRental: ILameScooterRental
    {
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException("the station name must not contain a number");
            }
            var reader = new StreamReader("scooters.json");
            
            var jsonString = await reader.ReadToEndAsync();
            var scooterStations = JsonSerializer.Deserialize<List<LameScooterStationList>>(jsonString,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            
            //5. Create and throw your own Exception
            //Create your own Exception called NotFoundException. Throw it, if the station can not be found.
            //Catch it in the calling code and print "Could not find: " and the Message Property of the exception.
            if(scooterStations?.Find(station=>station.Name == stationName) == null)
            {
                throw new NotFoundException($"Could not find station: {stationName}");
            }
            return scooterStations.Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault();
        }
    }
}