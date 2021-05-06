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
            //Throw an ArgumentException (provided in System) if the user calls GetScooterCountInStation with a string which contains numbers. 
            //Catch the exception in the calling code (your Main Method) and print "Invalid Argument: " and the Message-Property of the exception.
    

            ILameScooterRental rental = new OfflineLameScooterRental();
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
