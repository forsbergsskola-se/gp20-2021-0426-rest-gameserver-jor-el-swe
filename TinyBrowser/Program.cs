using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program {
        static TcpClient tcpClient;
        static NetworkStream netStream;
        static List<string> linkNames = new List<string>();
        static List<string> hyperLinks = new List<string>();


        const string hostName = "www.acme.com";
        //const string hostName = "www.marc-zaku.de";
        const int tcpPort = 80;
        
        static void Main(string[] args) {
            InitClient();

            RunMainLoop();
            
            ShutDownProgram();
        }

        static void RunMainLoop() {
            while (true) {
                SendGetRequest();
                
                var stringToParse = GetResponseFromWebSite();

                FindAllLinks(stringToParse);

                PrintAllLinks();

                AskUserForLink();
                
                break;
            }
        }

        static void AskUserForLink() {
            /*- Ask the user for Input
                - it should be a Number between 0 and the number of options
                - Follow the link that the user wants to follow and start at the beginning of the application again
                - (Send a TCP Request to acme.com...)
            */
           
            var test = false;
            var num = 0;
            while (!test)
            {
                Console.Write("What link do you want to follow: ");
                test = int.TryParse(Console.ReadLine(), out num);
                if (num > hyperLinks.Count) {
                    Console.WriteLine("number too large. press any key to continue");
                    Console.ReadLine();
                    PrintAllLinks();
                    test = false;
                }
            }
            Console.WriteLine("you want : " + num);
        }

        static void PrintAllLinks() {
            for (var i = 0; i < linkNames.Count; i++) {
                var iteratorString = i + ": ";
                Console.Write(iteratorString);
                Console.Write(linkNames[i]);
  
                var spaces = "";
                for (var j = 0; j < (50-linkNames[i].Length-iteratorString.Length); j++) {
                    spaces += " ";
                }
                Console.Write(spaces);
                Console.WriteLine("(" +hyperLinks[i] + ")");
            }
        }

        static void SendGetRequest() {
            //- Write a valid HTTP-Request to the Stream.
            var request = "GET / HTTP/1.1\r\n";
            request += "Host:"+ hostName+"\r\n";
            request += "\r\n";
            netStream.Write(Encoding.ASCII.GetBytes(request));
        }

        static void FindAllLinks(string stringToParse) {
            hyperLinks.Clear();
            linkNames.Clear();
            Console.WriteLine(stringToParse);
            Console.WriteLine("Opened: " + hostName);
            
            /*- Print that string (between `<title>` and `</title>`) to the console.*/
            var foundString = FindStringBetweenTwoStrings(stringToParse, "<title>", "</title>", 0);
            Console.WriteLine("Title: " + foundString);
            
            //Find all the hyperlinks
            var currentPosition = 0;
            //checks for wrap-around
            var largestPosition = 0;
            while (true) {
                currentPosition = stringToParse.IndexOf("<a href=\"",currentPosition, StringComparison.Ordinal) + "<a href=\"".Length;
                largestPosition = currentPosition > largestPosition ? currentPosition : largestPosition;

                if (largestPosition > currentPosition) break;
                
                var readTo = stringToParse.IndexOf("\">",currentPosition, StringComparison.Ordinal);
                string hyperlink = stringToParse.Substring(currentPosition, readTo - currentPosition);

                var findLinkName = readTo;
                var endOfLinkName = stringToParse.IndexOf("</a>",findLinkName, StringComparison.Ordinal);
                var startOfLinkName = stringToParse.IndexOf(">", findLinkName, StringComparison.Ordinal) + ">".Length;
                startOfLinkName = readTo + "\">".Length;
                string linkName = stringToParse.Substring(startOfLinkName, endOfLinkName - startOfLinkName);
                if (linkName.Length > 50) continue;
                
                //store in lists here
                linkNames.Add(linkName);
                hyperLinks.Add(hyperlink);
            }
        }

        static object FindStringBetweenTwoStrings(string sourceString, string startString, string endString, int startAtPosition) {
            var pFrom = sourceString.IndexOf(startString, startAtPosition, StringComparison.Ordinal) + startString.Length;
            var pTo = sourceString.IndexOf(endString, StringComparison.Ordinal);
            var result = sourceString.Substring(pFrom, pTo - pFrom);
            return result;
        }

        static string GetResponseFromWebSite() {
            if(netStream.CanRead){
                var myReadBuffer = new byte[1024];
                var myCompleteMessage = new StringBuilder();
                var numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size.
                do{
                    numberOfBytesRead = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                }
                while(netStream.DataAvailable);
                return myCompleteMessage.ToString();
            }
            else{
                Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
                return "";
            }
        }

        static void ShutDownProgram() {
            Console.WriteLine("shutting down...");
            netStream.Close();
            tcpClient.Close();
            
        }

        static void InitClient() {
            tcpClient = new TcpClient(hostName, tcpPort);
            netStream = tcpClient.GetStream();
            Console.WriteLine("connected to: " + netStream.Socket.RemoteEndPoint);
        }
    }
}
