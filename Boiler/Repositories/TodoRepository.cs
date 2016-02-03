using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Boiler.Models;
using ServiceStack.Data;

namespace Boiler.Repositories
{
    public interface ITodoRepository : IRepository<Todo>
    {
        
    }

    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {

        }
    }
}