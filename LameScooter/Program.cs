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
            //ILameScooterRental rental = new OfflineLameScooterRental();
            ILameScooterRental rental = new DeprecatedLameScooterRental();
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
