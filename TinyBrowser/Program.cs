namespace TinyBrowser
{
    static class Program {
        
        static void Main(string[] args) {
            Networking.Init();
            RunMainLoop();
        }

        static void RunMainLoop() {
            while (true) {
                Networking.InitClient();
                
                Networking.SendGetRequest();
                
                var stringToParse = Networking.GetResponseFromWebSite();

                Networking.FindAllLinks(stringToParse);

                UserInput.PrintAllLinks();

                UserInput.AskUserForLink();
                
                Networking.CloseStreamAndTcpClient();
            }
        }
    }
}
