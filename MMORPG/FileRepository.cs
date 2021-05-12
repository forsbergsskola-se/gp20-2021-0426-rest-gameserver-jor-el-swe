using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMORPG {
    public class FileRepository: IRepository {
        class PlayersContainer {
            public List<Player> playersList;
        }
        
        const string TextFilePath = "game-dev.txt";
        public async Task<Player> Get(Guid id) {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            return playersContainer?.playersList.FirstOrDefault(player => player.Id == id);
        }

        public async Task<Player[]> GetAll() {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            return playersContainer?.playersList.ToArray();
        }

        public async Task<Player> Create(Player player) {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            playersContainer.playersList.Add(player);

            var result = JsonSerializer.Serialize(playersContainer);
            await File.WriteAllTextAsync(TextFilePath, result);
            return player;
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player) {
            var modifiedPlayer = new Player();
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            foreach (var playerFound in playersContainer.playersList.Where(playerItem => playerItem.Id == id)) {
                playerFound.Score = player.Score;
                modifiedPlayer = playerFound;

            }
                
            var result = JsonSerializer.Serialize(playersContainer);
            await File.WriteAllTextAsync(TextFilePath, result);
            return modifiedPlayer;
        }

        public async Task<Player> Delete(Guid id) {
            Player playerToDelete = null;
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            foreach (var playerFound in playersContainer.playersList.Where(playerItem => playerItem.Id == id)) {
                playerToDelete = playerFound;
            }

            if(playerToDelete!=null) 
                playersContainer.playersList.Remove(playerToDelete);
                
            var result = JsonSerializer.Serialize(playersContainer);
            await File.WriteAllTextAsync(TextFilePath, result);
            return playerToDelete;
        }
    }
}