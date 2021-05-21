using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameRestAPI
{
    class Program {
        const int NumberOfAPIs = 0;
        static readonly HttpClient Client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            
            CheckUserInput(out var choice, "Choose a REST API to explore: ", "0: GitHub",NumberOfAPIs );
            
            switch (choice) {
                case 0 :
                    await DoGitHubAPI();
                    break;
                default:
                    Console.WriteLine("no such API");
                    break;
            }
        }

        static void CheckUserInput(out int o, string instructions, string choices, int maxNumChoices) {
            var userInputFalse = true;
            var num = 0;
            while (userInputFalse) {
                Console.WriteLine(choices);
                Console.Write(instructions);
                var userInput = Console.ReadLine();
                userInputFalse = !int.TryParse(userInput, out num) || num > maxNumChoices;
            }
            o = num;
        }

        static async Task DoGitHubAPI() {
            var userName = string.Empty;
            while (true) {
                Console.Write("Enter a user you would like to inspect\n" +
                              " (for example jor-el-swe or marczaku. 'q' for quit):");
                userName= Console.ReadLine();
                if (userName == "q" || userName == "Q")
                    break;
                CheckUserInput(out var choice, "What would you like to see next: ", "0: Followers\n1: Organizations\n2: Repositories", 2);
                switch (choice) {
                    case 0:
                        await CheckFollowers(userName);
                        break;
                    case 1:
                        await CheckOrganizations(userName);
                        break;
                    case 2:
                        await CheckRepositories(userName);
                        break;
                    default:
                        Console.WriteLine("no such API");
                        break;
                }
            }
        }

        static async Task CheckOrganizations(string userName) {
            var organizations = await ProcessAPICall(userName, "orgs");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{userName}:s organizations");
            Console.WriteLine("**************************");
            foreach (var org in organizations) {
                var holder = org as Organization;
                Console.WriteLine(holder);
                Console.WriteLine();
            }
        }

        static async Task CheckFollowers(string userName) {
            throw new NotImplementedException();
        }

        static async Task CheckRepositories(string userName) {
            var repositories = await ProcessAPICall(userName, "repos");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{userName}:s repos");
            Console.WriteLine("**************************");
            foreach (var repo in repositories) {
                var holder = repo as Repository;
                Console.WriteLine(holder.Name);
                Console.WriteLine(holder.Description);
                Console.WriteLine(holder.GitHubHomeUrl);
                Console.WriteLine(holder.Homepage);
                Console.WriteLine(holder.Watchers);
                Console.WriteLine(holder.LastPush);
                Console.WriteLine();
            }
        }

        static async Task<IList> ProcessAPICall(string user, string operation) {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            Client.DefaultRequestHeaders.Add("User-Agent", "my repo finder");

            var streamTask = Client.GetStreamAsync($"https://api.github.com/users/{user}/{operation}");

            switch (operation) {
                case "repos":
                    return await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
                    break;
                case "orgs":
                    return  await JsonSerializer.DeserializeAsync<List<Organization>>(await streamTask);
                    break;
                case "followers":
                    return await JsonSerializer.DeserializeAsync<List<Follower>>(await streamTask);
                    break;
                default:
                    return null;
            }
        }
    }
    
}
