using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameRestAPI
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
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
