using System;
using System.Net.Sockets;

namespace TinyBrowser
{
    class Program {
        static TcpClient tcpClient;
        static NetworkStream netStream;

        const string hostName = "www.acme.com";
        const int tcpPort = 80;
        
        static void Main(string[] args) {
            InitClient(ref tcpClient);

            RunMainLoop();
            
            ShutDownProgram(ref tcpClient);
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
                break;
            }
        }

        static void ShutDownProgram(ref TcpClient tcpClient1) {
            tcpClient1.Close();
        }

        static void InitClient(ref TcpClient tcpClient1) {
            tcpClient1 = new TcpClient(hostName, tcpPort);
            netStream = tcpClient1.GetStream();
        }
    }
}
