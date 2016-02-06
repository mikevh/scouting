using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Boiler.Models;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

namespace Boiler.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        void UpdatePassword(string username, string password);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public IUserAuthRepository userauth_repository { get; set; }

        public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) {}

        public void UpdatePassword(string username, string password) {
            var userauth = userauth_repository.GetUserAuthByUserName(username);
            userauth_repository.UpdateUserAuth(userauth, userauth, password);
        }

        public override int Insert(User model, IDbTransaction transaction = null) {
            using (var trans = OpenConnection().OpenTransaction()) {
                var rv = base.Insert(model, transaction);

                var userauth =  userauth_repository.CreateUserAuth(new UserAuth {
                    UserName = model.Username,
                    Email = model.Email
                }, model.Password);

                userauth_repository.SaveUserAuth(userauth);

                trans.Commit();
                return rv;
            }
        }

        public override int Update(User model, IDbTransaction transaction = null) {

            base.Update(model, transaction);

            var userauth = userauth_repository.GetUserAuthByUserName(model.Username);
            userauth.Email = model.Email;
            userauth_repository.UpdateUserAuth(userauth, userauth);

            return model.Id;
        }

        public override void Delete(int id, IDbTransaction transaction = null) {

            var conn = transaction?.Connection ?? OpenConnection();
            var trans = transaction ?? conn.OpenTransaction();

            var model = GetById(id);
            base.Delete(id, transaction);

            var userauth = userauth_repository.GetUserAuthByUserName(model.Username);
            userauth_repository.DeleteUserAuth(userauth.Id);

            trans.Commit();

            if (transaction == null) {
                conn.Dispose();
                trans.Dispose();
            }
        }
    }
}