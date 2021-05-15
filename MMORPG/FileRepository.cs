using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMORPG {
    public class FileRepository: IRepository {
        class PlayersContainer {
            public List<Player> PlayersList { get; set; } = new List<Player>();
        }

        const string TextFilePath = "game-dev.txt";
        public async Task<Player> Get(Guid id) {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            return playersContainer?.PlayersList.FirstOrDefault(player => player.Id == id);
        }

        public async Task<Player[]> GetAll() {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            return playersContainer?.PlayersList.ToArray();
        }

        public async Task<Player> Create(Player player) {
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            playersContainer?.PlayersList.Add(player);

            var result = JsonSerializer.Serialize(playersContainer,typeof(PlayersContainer),new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            await File.WriteAllTextAsync(TextFilePath, result);
            return player;
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player) {
            var modifiedPlayer = new Player();
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            foreach (var playerFound in playersContainer.PlayersList.Where(playerItem => playerItem.Id == id)) {
                playerFound.Score = player.Score;
                modifiedPlayer = playerFound;

            }
                
            var result = JsonSerializer.Serialize(playersContainer, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            await File.WriteAllTextAsync(TextFilePath, result);
            return modifiedPlayer;
        }

        public async Task<Player> Delete(Guid id) {
            Player playerToDelete = null;
            var fileContent = await File.ReadAllTextAsync(TextFilePath);
            var playersContainer = JsonSerializer.Deserialize<PlayersContainer>(fileContent,new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            foreach (var playerFound in playersContainer.PlayersList.Where(playerItem => playerItem.Id == id)) {
                playerFound.IsDeleted = true;
                playerToDelete = playerFound;
            }
            
            var result = JsonSerializer.Serialize(playersContainer, typeof(PlayersContainer),new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            await File.WriteAllTextAsync(TextFilePath, result);
            return playerToDelete;
        }

        public Task<Item> GetItem(Guid playerId, string itemName) {
            throw new NotImplementedException();
        }

        public Task<Item[]> GetAllItems(Guid playerId) {
            throw new NotImplementedException();
        }

        public Task<Item> Create(Guid playerId, string itemName) {
            throw new NotImplementedException();
        }

        public Task<Item> Modify(Guid playerId, string itemName, string newItemName) {
            throw new NotImplementedException();
        }

        public Task<Player> Delete(Guid playerId, string itemName) {
            throw new NotImplementedException();
        }
    }
}