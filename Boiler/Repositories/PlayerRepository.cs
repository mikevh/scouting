using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {

    }

    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {
            
        }
    }
}