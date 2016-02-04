using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        
    }

    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {
        public PriorityRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {
            
        }
    }
}