using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MMORPG {
    [ApiController]
    [Route("[controller]")]
    public class PlayersController:ControllerBase {
        IRepository _repository;
        public PlayersController(IRepository repository) {
            _repository = repository;
        }
        
        [HttpGet("player/GetPlayer")]
        public Task<Player> Get(Guid id) {
            return _repository.Get(id);
        }

        [HttpGet("player/GetAllPlayers")]
        public Task<Player[]> GetAll() {
            return _repository.GetAll();
        }
        [HttpPost("player/CreateNewPlayer")]
        public Task<Player> Create(NewPlayer player) {
            var newPlayer = new Player {Name = player.Name, CreationTime = DateTime.Now, Id = Guid.NewGuid()};
            return _repository.Create(newPlayer);
        }
        [HttpPut("player/ModifyPlayer")]
        public Task<Player> Modify(Guid id, ModifiedPlayer player) {
            return _repository.Modify(id, player);
        }
        [HttpDelete("player/DeletePlayer")]
        public Task<Player> Delete(Guid id) {
            return _repository.Delete(id);
        }
    }
}