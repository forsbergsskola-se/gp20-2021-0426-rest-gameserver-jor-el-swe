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
            return scooterStations?.Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault() ?? 0;
        }
    }
}