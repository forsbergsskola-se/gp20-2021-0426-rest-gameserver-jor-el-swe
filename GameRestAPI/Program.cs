using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameRestAPI
{
    class Program {
        const int NumberOfApIs = 0;
        static readonly HttpClient Client = new HttpClient();
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
                userInputFalse = !int.TryParse(userInput, out num) || num > NumberOfApIs;
            }
            o = num;
        }

        static async Task DoGitHubAPI() {
            Console.Write("Enter a user you would like to inspect\n" +
                          " (for example jor-el-swe or marczaku):");
            var userName= Console.ReadLine();
            var repositories = await ProcessRepositories(userName);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{userName}:s repos");
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

        static async Task<List<Repository>> ProcessRepositories(string user)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            Client.DefaultRequestHeaders.Add("User-Agent", "my repo finder");

            var streamTask = Client.GetStreamAsync($"https://api.github.com/users/{user}/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            return repositories;
        }
    }
}
