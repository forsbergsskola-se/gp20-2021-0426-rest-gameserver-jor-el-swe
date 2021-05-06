using System;
using System.Threading.Tasks;

namespace LameScooter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine("not enough arguments. exiting....");
                Console.WriteLine("dotnet run Linnanmäki realtime dotnet run Sepänkatu realtime dotnet run Pohjolankatu realtime");
                return;
            }
            
            ILameScooterRental rental = null;
            if (args.Length>1) {
                rental = args[1] switch {
                    "offline" => new OfflineLameScooterRental(),
                    "deprecated" => new DeprecatedLameScooterRental(),
                    "realtime" => new RealTimeLameScooterRental(),
                    "mongodb" => new MongoDBLameScooterRental(),
                    _ => new OfflineLameScooterRental()
                };
            }
            else {
                rental = new OfflineLameScooterRental();
            }

            var count = 0;
            try
            {
                count = await rental.GetScooterCountInStation(args[0]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            Console.Write($"Number of scooters at this station ({args[0]}): {count}");
        }
    }
}
