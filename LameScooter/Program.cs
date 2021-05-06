using System;
using System.Threading.Tasks;

namespace LameScooter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(args[0]);
            ILameScooterRental rental = null;

            var count = await rental.GetScooterCountInStation(null);
            Console.Write($"Number of scooters at this station: {count}");
        }
    }
}
