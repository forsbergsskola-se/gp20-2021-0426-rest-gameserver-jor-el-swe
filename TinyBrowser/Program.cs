using System;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program {
        static TcpClient tcpClient;
        static NetworkStream netStream;

        const string hostName = "www.acme.com";
        const int tcpPort = 80;
        
        static void Main(string[] args) {
            InitClient();

            RunMainLoop();
            
            ShutDownProgram();
        }

        static void RunMainLoop() {
            while (true) {
                /*
                 *  - `Write` allows you to send Bytes over the socket.
                    - `Read` allows you to read Bytes over the socket.
                    - `Close` needs to be called when you are done sending bytes over the stream.
                    - `Encoding.ASCII.GetBytes` Can convert a `string` to ASCII-`byte[]` for you.
                    - `Encoding.ASCII.GetString` Can convert a `byte[]` to a `string`. 
                    
                 */
                
                
                //- Write a valid HTTP-Request to the Stream.
                var request = "GET / HTTP/1.1\r\n";
                request += "Host:"+ hostName+"\r\n";
                request += "\r\n";
                netStream.Write(Encoding.ASCII.GetBytes(request));
                
                //- Fetch the response from the Website
                var stringToParse = GetResponseFromWebSite();

                GenerateConsoleOutput(stringToParse);
               
                
                
                /*
                 * - Search the response for all occurences of `<a href ="`
  - One sample: `<a href="auxprogs.html">auxiliary programs</a>`
  - Without going into too much detail:
    - `<a>` is the start tag of an HTML `hyperlink`-Element used for clickable links in browsers
    - `href="..."` is an HTML url-Attribute used to give the URL to the Hyperlink
    - `</a>` is the end tag of an HTML `hyperlink`-Element
    - Everything inbetween is the HTML-Content of the Element
    - And in this case, describes the Display Text of the Hyperlink
                 */
                
                
                /*
                 *
                 * - For each occurence:
  - Find all letters until the next `"`-symbol.
  - These letters define the local URL to the destination
  - Remember this, so you can navigate to that URL, if the User decides to follow this link
  - Navigate to the next `>`-symbol, so you find the end of the start tag.
  - Every letter until the next occurence of `</a>` are part of the display text.
- Now, when you have all the information (display text & url for each link)
- Print them all to the console
  - Recommendation: Use an iterator i, starting at 0.
  - Iterate over a list of all information that you have stored before.
  - Print: `%INDEX%: %DISPLAYNAME% (%URL%)`, e.g.: `3: auxiliary programs (auxprogs.html)`
- Ask the user for Input
  - it should be a Number between 0 and the number of options
  - Follow the link that the user wants to follow and start at the beginning of the application again
  - (Send a TCP Request to acme.com...)
                 */
                
                break;
            }
        }

        static void GenerateConsoleOutput(string stringToParse) {
            Console.WriteLine("Opened: " + hostName);
            
            /*- Print that string (between `<title>` and `</title>`) to the console.*/
            int pFrom = stringToParse.IndexOf("<title>", StringComparison.Ordinal) + "<title>".Length;
            int pTo = stringToParse.IndexOf("</title>", StringComparison.Ordinal);

            String result = stringToParse.Substring(pFrom, pTo - pFrom);
            Console.WriteLine("Title: " + result);
            
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
