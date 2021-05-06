using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LameScooter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*7. Implement more Command Line Arguments
            dotnet run Linnanmäki offline
            dotnet run Sepänkatu deprecated
            dotnet run Pohjolankatu offline*/
            ILameScooterRental rental = null;
            if (args.Length>1)
            {
                switch (args[1])
                {
                    case "offline":
                        rental = new OfflineLameScooterRental();
                        break;
                    case "deprecated":            
                        rental = new DeprecatedLameScooterRental();
                        break;
                    default:
                        rental = new OfflineLameScooterRental();
                        break;
                }
            }
            else
            {
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
