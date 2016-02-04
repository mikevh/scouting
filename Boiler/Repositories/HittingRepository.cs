using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface IHittingRepository : IRepository<Hitting>
    {

    }

    public class HittingRepository : Repository<Hitting>, IHittingRepository
    {
        public HittingRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {
            
        }
    }
}