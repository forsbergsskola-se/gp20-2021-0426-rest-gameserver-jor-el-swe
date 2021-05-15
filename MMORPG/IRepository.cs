using System;
using System.Threading.Tasks;

namespace MMORPG {
    public interface IRepository {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);
        
        Task<Item> GetItem(Guid playerId, string itemName);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> Create(Guid playerId, string itemName);
        Task<Item> Modify(Guid playerId, string itemName, string newItemName);
        Task<Item> Delete(Guid playerId, string itemName);
    }
}