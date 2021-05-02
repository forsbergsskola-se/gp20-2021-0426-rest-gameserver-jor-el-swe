using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public struct HostAndPath
    {
        public HostAndPath(string host, string path)
        {
            HostName = host;
            PathName = path;
        }
        public string HostName { get; set; }
        public string PathName { get; set; }
    }
    
    class Program {
        static TcpClient tcpClient;
        static NetworkStream netStream;
        static List<string> linkNames = new List<string>();
        static List<string> hyperLinks = new List<string>();
        const int MAX_LINKNAME_LENGHT = 50;


        const string initialHostName = "www.acme.com";
        static string initialPathName = "/";
        
        private static int currentPathIndex=0;
        static List<HostAndPath> pathHistory = new List<HostAndPath>();
        private static HostAndPath currentHostandPath;
        
        const int tcpPort = 80;
        
        static void Main(string[] args) {
            pathHistory.Add(new HostAndPath(initialHostName, initialPathName));
            currentHostandPath = pathHistory[currentPathIndex];
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
                
                CloseStreamAndTCPClient();
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

            var pathName = hyperLinks[num];
            pathName = pathName.TrimStart('/');
            pathName = "/" + pathName;
            pathHistory.Add(new HostAndPath(initialHostName, pathName));
            currentPathIndex++;
            currentHostandPath = pathHistory[currentPathIndex];
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
            var request = "GET " + currentHostandPath.PathName +" HTTP/1.1\r\n";
            request += "Host:"+ currentHostandPath.HostName+"\r\n";
            request += "\r\n";
            netStream.Write(Encoding.ASCII.GetBytes(request));
        }

        static void FindAllLinks(string stringToParse) {
            hyperLinks.Clear();
            linkNames.Clear();

            Console.WriteLine("Opened: " + currentHostandPath.HostName + currentHostandPath.PathName);
            
            /*- Print that string (between `<title>` and `</title>`) to the console.*/
            var foundString = FindStringBetweenTwoStrings(stringToParse, "<title>", "</title>", 0,out var foundPosition);
            Console.WriteLine("Title: " + foundString);
            
            //Find all the hyperlinks
            var currentPosition = 0;
            while (true) {
                //find the hyperlink
                var hyperlink = FindStringBetweenTwoStrings(stringToParse, "<a href=\"", "\">", currentPosition, out currentPosition);
                if (currentPosition == -1)
                    break;
                
                //find the link display name
                var linkName = FindStringBetweenTwoStrings(stringToParse, "\">", "</a>", currentPosition, out currentPosition);
                if (linkName.Length > 50) continue;
                
                //store in lists here
                linkNames.Add(linkName);
                hyperLinks.Add(hyperlink);
            }
        }

        static string FindStringBetweenTwoStrings(string sourceString, string startString, string endString, int startAtPosition, out int foundPosition)
        {
            foundPosition = sourceString.IndexOf(startString, startAtPosition, StringComparison.OrdinalIgnoreCase);
            if (foundPosition == -1)
            {
                return "";
            }
            
            foundPosition += startString.Length;
            var pTo = sourceString.IndexOf(endString, foundPosition, StringComparison.OrdinalIgnoreCase);
            var result = sourceString[foundPosition..pTo];
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

        static void CloseStreamAndTCPClient() {
            Console.WriteLine("closing stream and TPC client...");
            netStream.Close();
            tcpClient.Close();
            
        }

        static void InitClient() {
            tcpClient = new TcpClient(currentHostandPath.HostName, tcpPort);
            netStream = tcpClient.GetStream();
            Console.WriteLine("connected to: " + netStream.Socket.RemoteEndPoint);
        }
    }
}
