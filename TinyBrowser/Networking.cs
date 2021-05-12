using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser {
    public static class Networking {
        const string InitialHostName = "www.acme.com";
        //const string InitialHostName = "www.milk.com";
        const string InitialPathName = "/";
        
        static TcpClient tcpClient;
        static NetworkStream netStream;
        
        static readonly List<string> LinkNames = new List<string>();
        static readonly List<string> HyperLinks = new List<string>();

        static readonly List<HostAndPath> PathHistory = new List<HostAndPath>();
        static HostAndPath currentHostAndPath;
        
        const int TcpPort = 80;

        public static int NumberOfHyperLinks => HyperLinks.Count;
        public static int NumberOfLinkNames => LinkNames.Count;

        public static string GetLinkName(int value) {
            return LinkNames[value];
        }
        
        public static string GetHyperLink(int value) {
            return HyperLinks[value];
        }
        public static HostAndPath CurrentHostAndPath {
            get => currentHostAndPath;
            private set => currentHostAndPath = value;
        }

        static int CurrentPathIndex { get; set; }

        public static void Init() {
            PathHistory.Add(new HostAndPath(InitialHostName, InitialPathName));
            currentHostAndPath = PathHistory[CurrentPathIndex];
        }

        public static void SendGetRequest() {
            //- Write a valid HTTP-Request to the Stream.
            var request = "GET " + currentHostAndPath.PathName +" HTTP/1.1\r\n";
            request += "Host:"+ currentHostAndPath.HostName+"\r\n";
            request += "\r\n";
            netStream.Write(Encoding.ASCII.GetBytes(request));
        }
        
        public static void CloseStreamAndTcpClient() {
            netStream.Close();
            tcpClient.Close();
            
        }

        public static void InitClient() {
            tcpClient = new TcpClient(currentHostAndPath.HostName, TcpPort);
            netStream = tcpClient.GetStream();
            Console.WriteLine("connected to: " + netStream.Socket.RemoteEndPoint);
        }

        public static string GetResponseFromWebSite() {
            if(netStream.CanRead){
                var streamReader = new StreamReader(netStream);
                var completeMessage = streamReader.ReadToEnd();
                
                return completeMessage;
            }
            Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
            return "";
        }

        public static void SetCurrentHostAndPath(in int linkNumber) {
            CurrentHostAndPath = ParserHelper.FindHostAndPath(InitialHostName,HyperLinks[linkNumber]);
        }

        public static void AddCurrentPath() {
            PathHistory.Add(CurrentHostAndPath);
            CurrentPathIndex++;
        }

        public static void FindAllLinks(string stringToParse) {
            HyperLinks.Clear();
            LinkNames.Clear();
            int endPosition;
            /*- Print that string (between `<title>` and `</title>`) to the console.*/
            var foundString = ParserHelper.FindStringBetweenTwoStrings(stringToParse, "<title>", "</title>", 0,out var foundPosition, out endPosition);
            Console.WriteLine("Opened: " + currentHostAndPath.HostName + currentHostAndPath.PathName);
            Console.WriteLine("Title: " + foundString);
            
            //Find all the hyperlinks
            var currentPosition = 0;
            while (true) {
                //find the hyperlink
                var hyperlink = ParserHelper.FindStringBetweenTwoStrings(stringToParse, "<a href=\"", "\">", currentPosition, out currentPosition, out endPosition);
                if (currentPosition == -1)
                    break;
                
                //find the link display name
                var linkName = ParserHelper.FindStringBetweenTwoStrings(stringToParse, "\">", "</a>", currentPosition, out currentPosition, out endPosition);
                if (linkName.Length > 50) continue;
                
                //store in lists here
                LinkNames.Add(linkName);
                HyperLinks.Add(hyperlink);
            }
        }
    }
}