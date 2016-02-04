using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface IFieldingRepository : IRepository<Fielding>
    {

    }

    public class FieldingRepository : Repository<Fielding>, IFieldingRepository
    {
        public FieldingRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {
            
        }
    }
}