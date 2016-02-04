using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Web;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Model;
using ServiceStack.OrmLite;

namespace Boiler.Repositories
{

    public interface IRepository<T>
    {
        List<T> All();
        T GetById(int id);
        void Delete(int id, IDbTransaction transaction = null);
        int Insert(T model, IDbTransaction transaction = null);
        int Update(T model, IDbTransaction transaction = null);

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Where(object anonType);
        T Single(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T>, IDisposable where T : IHasId<int>, new()
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public Repository(IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        private IDbConnection OpenConnection() {
            if (_connectionFactory == null) {
                throw new NullReferenceException("connection_factory null in base repository class");
            }

            return _connectionFactory.OpenDbConnection();
        }

        public List<T> All() {
            using (var c = OpenConnection()) {
                return c.Select<T>();
            }
        }

        public T GetById(int id) {
            using (var c = OpenConnection()) {
                return c.LoadSingleById<T>(id);
            }
        }

        public void Delete(int id, IDbTransaction transaction = null) {
            var c = transaction?.Connection ?? OpenConnection();

            c.DeleteById<T>(id);

            if (transaction == null) {
                c.Dispose();
            }
        }

        public int Insert(T model, IDbTransaction transaction = null) {
            var c = transaction?.Connection ?? OpenConnection();

            var rv = Convert.ToInt32(c.Insert(model, selectIdentity: true));

            if (transaction == null) {
                c.Dispose();
            }

            return rv;
        }

        public int Update(T model, IDbTransaction transaction = null) {
            if (model.Id < 1) {
                throw new InvalidDataException("Cannot update model with no id");
            }

            var c = transaction?.Connection ?? OpenConnection();

            var existing = c.SingleById<T>(model.Id);
            ThrowIfModelNotFound(model, existing);
            c.Update(model);

            if (transaction == null) {
                c.Dispose();
            }

            return model.Id;
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            using (var c = OpenConnection()) {
                return c.Where<T>(predicate);
            }
        }

        public IEnumerable<T> Where(object anonType) {
            using (var c = OpenConnection()) {
                return c.Where<T>(anonType);
            }
        }

        public T Single(Expression<Func<T, bool>> predicate) {
            using (var c = OpenConnection()) {
                return c.Single(predicate);
            }
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            using (var c = OpenConnection()) {
                try {
                    return c.Single(predicate);
                }
                catch {
                    return default(T);
                }
            }
        }

        public virtual void Dispose() {
            
        }

        private void ThrowIfModelNotFound(T model, T existing) {
            var thistype = GetType().Name;
            existing.ThrowIfNull($"Record not found: {thistype} with id {model.Id}");
        }
    }
}