using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface IPitchingRepository : IRepository<Pitching>
    {

    }

    public class PitchingRepository : Repository<Pitching>, IPitchingRepository
    {
        public PitchingRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {
            
        }
    }
}