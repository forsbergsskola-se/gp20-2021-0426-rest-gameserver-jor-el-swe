using System;
using System.Threading.Tasks;

namespace LameScooter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(args[0]);
            ILameScooterRental rental = new OfflineLameScooterRental();

            var count = await rental.GetScooterCountInStation(args[0]);
            Console.Write($"Number of scooters at this station: {count}");
        }
    }
}
