using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LameScooter
{
    public class DeprecatedLameScooterRental: ILameScooterRental
    {
        /*Create a class called DeprecatedLameScooterRental which also implements the ILameScooterRental-Interface.
         Download scooters.txt and also put it into your project and again make sure, that it gets copied when Building. 
         Find a way to read the required data from the file.
         How much code can you share? How much code do you have to duplicate?
         */
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            if (stationName.Any(char.IsDigit))
            {
                throw new ArgumentException("the station name must not contain a number");
            }
            var reader = new StreamReader("scooters.txt");
            
            var stationsString = await reader.ReadToEndAsync();
            var scooterStations = GetScooterStationList(stationsString);
            
            //5. Create and throw your own Exception
            //Create your own Exception called NotFoundException. Throw it, if the station can not be found.
            //Catch it in the calling code and print "Could not find: " and the Message Property of the exception.
            if(scooterStations?.Find(station=>station.Name == stationName) == null)
            {
                throw new NotFoundException($"Could not find station: {stationName}");
            }
            return scooterStations.Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault();
        }

        private static List<LameScooterStationList> GetScooterStationList(string stationsString)
        {
            var scooterStationLists = new List<LameScooterStationList>();
            foreach (var myString in stationsString.Split(new string[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries))
            {
                var substring = myString.Split(':', StringSplitOptions.TrimEntries);
                var station = new LameScooterStationList {Name = substring[0], BikesAvailable = int.Parse(substring[1])};
                scooterStationLists.Add(station);
            }


            return scooterStationLists;
        }
    }
}