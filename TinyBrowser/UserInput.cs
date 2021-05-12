using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TinyBrowser {
    public static class UserInput {
        const int MaxLinkNameLenght = 50;
        static string IsUserInputCorrectNumber(out bool test, out int num)
        {
            var userInput = Console.ReadLine();
            test = int.TryParse(userInput, out num);
            if (num + 1 > Networking.NumberOfHyperLinks || num < 0) {
                Console.WriteLine("wrong number. press any key to continue");
                Console.ReadLine();
                PrintAllLinks();
                test = false;
            }

            return userInput;
        }

        public static void PrintAllLinks() {
            for (var i = 0; i < Networking.NumberOfLinkNames; i++) {
                var iteratorString = i + ": ";
                Console.Write(iteratorString);
                Console.Write(Networking.GetLinkName(i));
  
                var spaces = "";
                for (var j = 0; j < (MaxLinkNameLenght-Networking.GetLinkName(i).Length-iteratorString.Length); j++) {
                    spaces += " ";
                }
                Console.Write(spaces);
                Console.WriteLine("(" + Networking.GetHyperLink(i) + ")");
            }
        }

        public static void AskUserForLink() {
            /*- Ask the user for Input
                - it should be a Number between 0 and the number of options
                - Follow the link that the user wants to follow and start at the beginning of the application again
                - (Send a TCP Request to acme.com...)
                
                Also allow for 
                b – back
                f – forward
                r – refresh
                h – history
                g – goto [user enter URL]
            */
           
            var isUserInputValid = false;
            while (!isUserInputValid)
            {
                Console.WriteLine("Enter a link number");
                Console.WriteLine("or");
                Console.WriteLine("R - Refresh current page");
                Console.WriteLine("B - Back one step");
                Console.WriteLine("F - Forward one step");
                Console.WriteLine("H - Show browser history");
                Console.Write("Which link do you want to follow: ");
                var userInput = IsUserInputCorrectNumber(out isUserInputValid, out var linkNumber);
                
                //we received a correct link number
                if (isUserInputValid)
                {
                    Networking.SetCurrentHostAndPath(linkNumber);
                }
                //we did not receive a valid number.
                //let's try with one of the letter options
                else
                {
                    isUserInputValid = true;
                    switch (userInput)
                    {
                        case "r":
                        case "R":
                            //on refresh, we don't need to to anything. 
                            //just keep the current link path for next iteration
                            break;
                        case "f":
                        case "F":
                            //move forward
                            Networking.BrowserForwardButton();
                            break;
                        case "b":
                        case "B":
                            //move backward
                            Networking.BrowserBackButton();
                            break;
                    
                        case "h":
                        case "H":
                            //show browser history
                            var history = Networking.GetHistory();
                            PrintHistory(history);
            
                            break;
                            //goto user defined link
                        case "g":
                        case "G":
                        
                            break;

                        default: 
                            isUserInputValid = false;
                            break;
                    } 
                }
            }//while !valid input
            
            Console.WriteLine("you want :" + Networking.CurrentHostAndPath.PathName);
            Console.Write("press any key to follow that link");
            Console.ReadLine();

            Networking.AddTotalHistory();
        }

        static void PrintHistory(IEnumerable<HostAndPath> history) {
            Console.WriteLine("History:");
            foreach (var item in history) {
                Console.WriteLine($"host: {item.HostName} path: {item.PathName}");
            }
        }
    }
}