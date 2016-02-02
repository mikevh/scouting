using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Boiler.Models;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Data;
using ServiceStack.Web;

namespace Boiler
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Hello", typeof(HelloService).Assembly) {
            
        }

        public override void Configure(Container container) {

            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = DateHandler.ISO8601;
            OrmLiteConfig.DialectProvider = SqlServerOrmLiteDialectProvider.Instance;
            SqlServerDialect.Provider.GetStringConverter().UseUnicode = true;
            SqlServerDialect.Provider.GetDateTimeConverter().DateStyle = DateTimeKind.Utc;

            Plugins.Add(new SessionFeature());
            Plugins.Add(new ValidationFeature());

            container.Register<IAppDbConnectionFactory>(c => new AppDbConnectionFactory());
            container.Register<ICredentialsDbConnectionFactory>(c => new CredentialsDbConnectionFactory());
            container.Register<ICacheClient>(new MemoryCacheClient { FlushOnDispose = false });

            container.RegisterValidators(typeof(HelloService).Assembly);
            ConfigureAuth(container);

            //CreateAuthDb(container);
        }


        private void ConfigureAuth(Container container)
        {
            container.Register(GetUserSession).ReusedWithin(ReuseScope.Request);
            container.Register(GetUserProfile).ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<BoilerAuthRepository, IAuthRepository>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<BoilerAuthRepository, IUserAuthRepository>().ReusedWithin(ReuseScope.Request);

            var auth_providers = new IAuthProvider[] {new CredentialsAuthProvider() };
            var auth_feature = new AuthFeature(() => new UserSession(), auth_providers) {
                HtmlRedirect = "/login.html"
            };
            Plugins.Add(auth_feature);
        }

        private UserSession GetUserSession(Container container)
        {
            var cache_client = container.Resolve<ICacheClient>();
            var session = SessionFeature.GetOrCreateSession<UserSession>(cache_client);
            return session;
        }

        private UserProfile GetUserProfile(Container container)
        {
            var session = container.Resolve<UserSession>();
            return session.ToUserProfile();
        }

        private void CreateAuthDb(Container container) {
            var db = container.Resolve<ICredentialsDbConnectionFactory>();
            var repo = new OrmLiteAuthRepository(db);
            repo.DropAndReCreateTables();
            var admin_user = repo.CreateUserAuth(new UserAuth {
                UserName = "admin",
                Email = "admin@admin.com",
            }, "password");
            repo.SaveUserAuth(admin_user);

        }
    }

    public class BoilerAuthRepository : OrmLiteAuthRepository
    {
        public BoilerAuthRepository(ICredentialsDbConnectionFactory dbFactory) : base(dbFactory) {}
    }

    public class UserSession : AuthUserSession
    {
        public UserProfile UserProfile { get; set; }

        public UserProfile ToUserProfile() {
            if (IsAuthenticated) {
                return new UserProfile {
                    Id = UserProfile?.Id ?? 0,
                    Username = base.UserAuthName,
                    Email = base.Email,
                    IsAdmin = UserProfile?.IsAdmin ?? false
                };
            }
            return new UserProfile {
                Username = "anonymous",
                Email = ""
            };
        }
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    public interface IAppDbConnectionFactory : IDbConnectionFactory { }
    public interface ICredentialsDbConnectionFactory : IDbConnectionFactory { }

    public class AppDbConnectionFactory : OrmLiteConnectionFactory, IAppDbConnectionFactory
    {
        private static string AppConnectionString => ConfigurationManager.ConnectionStrings["App"].ConnectionString;
        public AppDbConnectionFactory() : base(AppConnectionString, SqlServerDialect.Provider) { }
    }

    public class CredentialsDbConnectionFactory : OrmLiteConnectionFactory, ICredentialsDbConnectionFactory
    {
        private static string CredentialsConnectionString => ConfigurationManager.ConnectionStrings["Credentials"].ConnectionString;
        public CredentialsDbConnectionFactory() : base(CredentialsConnectionString, SqlServerDialect.Provider) { }
    }
}