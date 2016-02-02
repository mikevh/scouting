using ServiceStack;

namespace Boiler.Models
{
    [Authenticate]
    public abstract class SecureBaseService : IService
    {
        
    }
}