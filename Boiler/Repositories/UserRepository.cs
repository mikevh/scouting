using System.Collections.Generic;
using System.Data;
using Boiler.Models;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Boiler.Repositories
{
    public interface IUserRepository
    {
        List<UserAuth> All();
        UserAuth GetById(int id);
        void Delete(int id);
        void Insert(UserAuth user, string password);
        void Update(UserAuth user, string passowrd = null);
    }

    public class UserRepository : IUserRepository
    {
        public IUserAuthRepository userauth_repository { get; set; }
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        public List<UserAuth> All() {
            using (var c = Open()) {
                return c.Select<UserAuth>();
            }
        }

        public UserAuth GetById(int id) {
            using (var c = Open()) {
                return c.SingleById<UserAuth>(id);
            }
        }

        public void Delete(int id) {
            userauth_repository.DeleteUserAuth(id);
        }

        public void Insert(UserAuth user, string password) {
            var userauth = userauth_repository.CreateUserAuth(new UserAuth
            {
                UserName = user.UserName,
                Email = user.Email
            }, password);

            userauth_repository.SaveUserAuth(userauth);
        }

        public void Update(UserAuth user, string password = null) {
            var update = userauth_repository.GetUserAuthByUserName(user.UserName);
            update.Email = user.Email;
            if (password != null) {
                userauth_repository.UpdateUserAuth(update, update, password);
            }
            else {
                userauth_repository.UpdateUserAuth(update, update);
            }
        }

        private IDbConnection Open() {
            return _connectionFactory.OpenDbConnection();
        }
    }
}