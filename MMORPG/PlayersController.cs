using System;
using System.Threading.Tasks;

namespace MMORPG {
    public class PlayersController {
        IRepository _repository;
        public PlayersController(IRepository repository) {
            _repository = repository;
        }
        
        public Task<Player> Get(Guid id) {
            return _repository.Get(id);
        }

        public Task<Player[]> GetAll() {
            return _repository.GetAll();
        }

        public Task<Player> Create(NewPlayer player) {
            var newPlayer = new Player {Name = player.Name, CreationTime = DateTime.Now};
            return _repository.Create(newPlayer);
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player) {
            return _repository.Modify(id, player);
        }

        public Task<Player> Delete(Guid id) {
            return _repository.Delete(id);
        }
    }
}