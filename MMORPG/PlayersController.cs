using System;
using System.Threading.Tasks;

namespace MMORPG {
    public class PlayersController {
        readonly Player _player;
        IRepository _repository;
        Player[] _players;
        public PlayersController(IRepository repository) {
            _repository = repository;
        }
        
        public async Task<Player> Get(Guid id) {
            return _player;
        }

        public async Task<Player[]> GetAll() {
            return _players;
        }

        public async Task<Player> Create(NewPlayer player) {
            return _player;
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player) {
            return _player;
        }

        public async Task<Player> Delete(Guid id) {
            return _player;
        }
    }
}