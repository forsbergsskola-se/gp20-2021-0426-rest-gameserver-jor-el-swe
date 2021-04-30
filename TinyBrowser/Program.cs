using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program {
        static TcpClient tcpClient;
        static NetworkStream netStream;
        static List<string> linkNames = new List<string>();
        static List<string> hyperLinks = new List<string>();
        const int MAX_LINKNAME_LENGHT = 50;


        const string hostName = "www.acme.com";
        static string pathName = "/";
        //const string hostName = "www.marc-zaku.de";
        const int tcpPort = 80;
        
        static void Main(string[] args) {
            RunMainLoop();
        }

        static void RunMainLoop() {
            while (true) {
                InitClient();
                
                SendGetRequest();
                
                var stringToParse = GetResponseFromWebSite();

                FindAllLinks(stringToParse);

                PrintAllLinks();

                AskUserForLink();
                
                ShutDownProgram();
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
            Console.WriteLine("you want : " + num + ": " + hyperLinks[num]);
            Console.WriteLine("press any key to follow that link");
            Console.ReadLine();
            pathName = hyperLinks[num];
            pathName = pathName.TrimStart('/');
            pathName = "/" + pathName;
        }

        static void PrintAllLinks() {
            for (var i = 0; i < linkNames.Count; i++) {
                var iteratorString = i + ": ";
                Console.Write(iteratorString);
                Console.Write(linkNames[i]);
  
                var spaces = "";
                for (var j = 0; j < (MAX_LINKNAME_LENGHT-linkNames[i].Length-iteratorString.Length); j++) {
                    spaces += " ";
                }
                Console.Write(spaces);
                Console.WriteLine("(" +hyperLinks[i] + ")");
            }
        }

        static void SendGetRequest() {
            //- Write a valid HTTP-Request to the Stream.
            var request = "GET " + pathName +" HTTP/1.1\r\n";
            request += "Host:"+ hostName+"\r\n";
            request += "\r\n";
            netStream.Write(Encoding.ASCII.GetBytes(request));
        }

        static void FindAllLinks(string stringToParse) {
            hyperLinks.Clear();
            linkNames.Clear();
            //Console.WriteLine(stringToParse);
            Console.WriteLine("Opened: " + hostName);
            
            /*- Print that string (between `<title>` and `</title>`) to the console.*/
            var foundString = FindStringBetweenTwoStrings(stringToParse, "<title>", "</title>", 0);
            Console.WriteLine("Title: " + foundString);
            
            //Find all the hyperlinks
            var currentPosition = 0;
            while (true) {
                //find the hyperlink
                currentPosition = stringToParse.IndexOf("<a href=\"", currentPosition, StringComparison.Ordinal);
                if (currentPosition == -1)
                    break;
                currentPosition += "<a href=\"".Length;

                var endOfHref = stringToParse.IndexOf("\">",currentPosition, StringComparison.Ordinal);
                var hyperlink = stringToParse.Substring(currentPosition, endOfHref - currentPosition);
                
                //find the link display name
                var startOfLinkName = endOfHref + "\">".Length;
                var findLinkName = endOfHref;
                var endOfLinkName = stringToParse.IndexOf("</a>",findLinkName, StringComparison.Ordinal);
                var linkName = stringToParse.Substring(startOfLinkName, endOfLinkName - startOfLinkName);
                if (linkName.Length > 50) continue;
                
                //store in lists here
                linkNames.Add(linkName);
                hyperLinks.Add(hyperlink);
            }
        }

        static string FindStringBetweenTwoStrings(string sourceString, string startString, string endString, int startAtPosition) {
            var pFrom = sourceString.IndexOf(startString, startAtPosition, StringComparison.OrdinalIgnoreCase) + startString.Length;
            var pTo = sourceString.IndexOf(endString, StringComparison.OrdinalIgnoreCase);
            var result = sourceString.Substring(pFrom, pTo - pFrom);
            return result;
        }

        static string GetResponseFromWebSite() {
            if(netStream.CanRead){
                var streamReader = new StreamReader(netStream);
                var completeMessage = streamReader.ReadToEnd();
                
                return completeMessage;
            }
            Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
            return "";
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
