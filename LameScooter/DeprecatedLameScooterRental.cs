using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LameScooter
{
    public class DeprecatedLameScooterRental: ILameScooterRental
    {
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException("the station name must not contain a number");
            }
            var reader = new StreamReader("scooters.txt");
            
            var stationsString = await reader.ReadToEndAsync();
            var scooterStations = GetScooterStationList(stationsString);
            
            if(scooterStations?.Find(station=>station.Name == stationName) == null)
            {
                throw new NotFoundException($"Could not find station: {stationName}");
            }
            return scooterStations.Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault();
        }

        private static List<LameScooterStationList> GetScooterStationList(string stationsString)
        {
            return stationsString.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Select(textRow => textRow.Split(':', StringSplitOptions.TrimEntries))
                .Select(substring => new LameScooterStationList {Name = substring[0], BikesAvailable = int.Parse(substring[1])})
                .ToList();
        }
    }
}