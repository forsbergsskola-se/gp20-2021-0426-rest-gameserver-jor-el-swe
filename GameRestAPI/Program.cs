using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameRestAPI
{
    class Program {
        const int numberOfAPIs = 1;
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            
            CheckUserInput(out var choice);
            
            switch (choice) {
                case 0 :
                    await DoGitHubAPI();
                    break;
                default:
                    Console.WriteLine("no such API");
                    break;
            }
        }

        static void CheckUserInput(out int o) {
            var userInputFalse = true;
            var num = 0;
            while (userInputFalse) {
                Console.WriteLine("Choose a REST API to explore: ");
                Console.WriteLine("0: GitHub");
                var userInput = Console.ReadLine();
                userInputFalse = !int.TryParse(userInput, out num) || num > numberOfAPIs;
            }
            o = num;
        }

        static async Task DoGitHubAPI() {
            var repositories = await ProcessRepositories();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("jor-el:s repos");
            Console.WriteLine("**************************");
            foreach (var repo in repositories) {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUrl);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine(repo.LastPush);
                Console.WriteLine();
                
            }
        }

        static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "my repo finder");

            var streamTask = client.GetStreamAsync("https://api.github.com/users/jor-el-swe/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            return repositories;
 
        }
    }
}
