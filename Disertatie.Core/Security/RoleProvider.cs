using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using NHibernate;
using NHibernate.Linq;
using System.Diagnostics;
using NHibernate.Criterion;
using Disertatie.Core.Models;
using Disertatie.Core.Repositories;

namespace Disertatie.Core.Security
{
    public class RoleProvider : System.Web.Security.RoleProvider 
    {
        private string _appName;
        public override string ApplicationName {
            get { return _appName; }
            set { _appName = value; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
            base.Initialize(name, config);

            if (config == null)
                throw new ArgumentNullException("config");

            _appName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            Debug.WriteLine("AddUsersToRoles");

            if (usernames.Any(r => string.IsNullOrWhiteSpace(r)))
                throw new ArgumentException("Username cannot be null or a whitespace string");

            if (roleNames.Any(r => string.IsNullOrWhiteSpace(r)))
                throw new ArgumentException("Role names canno be null or a whitespace string");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var users = session.CreateCriteria<Utilizator>()
                    .Add(Restrictions.In("username", usernames))
                    .List<Utilizator>();

                var roles = session.CreateCriteria<Rol>()
                    .Add(Restrictions.In("nume_rol", roleNames))
                    .List<Rol>();

                session.SetBatchSize(25);

                foreach (var user in users)
                    foreach (var role in roles) {
                        user.Roluri.Add(role);
                    }

                session.Transaction.Commit();
            }            
        }

        public override void CreateRole(string roleName) {
            Debug.WriteLine("CreateRole");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or white space strings");

            if (roleName.Contains(','))
                throw new ArgumentException("Role names cannot contain commas");

            if (this.RoleExists(roleName))
                throw new ProviderException("Role name already exists");

            using (var session = SessionManager.OpenSessionTransaction()) 
            {
                session.Save(new Rol { NumeRol = roleName, Activ = true });
                session.Transaction.Commit();            
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            Debug.WriteLine("DeleteRole");

            if (this.RoleExists(roleName) == false)
                throw new ProviderException("Role does not exist");

            if (throwOnPopulatedRole && this.GetUsersInRole(roleName).Length > 0) {
                throw new ProviderException("Cannot delete a populated role");
            }

            using (var session = SessionManager.OpenSessionTransaction()) {
                session.Delete(session.Load<Rol>(roleName));
                session.Transaction.Commit();
            }

            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            
            Debug.WriteLine("FindUsersInRole");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or be a whitespace string");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var usernames = session.CreateQuery("select u.username from Utilizatori u inner join UtilizatoriRoluri ur on ur.id_utilizator=u.id_utilizator inner join Roluri r on r.id_rol=ur.id_rol where r.nume_rol = :roleName and u.username like :username")
                            .SetAnsiString("nume_rol", roleName)
                            .SetAnsiString("username", usernameToMatch)
                            .List<string>()
                            .ToArray();
                session.Transaction.Commit();
                return usernames;
            }
        }

        public override string[] GetAllRoles() {
            Debug.WriteLine("GetAllRoles");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var roles = session.CreateQuery("select nume_rol from Roluri").List<string>().ToArray();
                session.Transaction.Commit();
                return roles;
            }
        }

        public override string[] GetRolesForUser(string username) {
            Debug.WriteLine("GetRolesForUser");

            if (username == null)
                throw new ArgumentNullException("username");
                 
            using (var session = SessionManager.OpenSessionTransaction()) {
         
                var user = session.CreateQuery("from Utilizator where Username = :username")
                            .SetAnsiString("username", username).UniqueResult<Utilizator>();

                if (user == null)
                    throw new ArgumentException("This user does not exist");

                var userRoles = user.Roluri.Select(r => r.NumeRol).ToArray();
                
                session.Transaction.Commit();
                return userRoles;
            }
        }

        public override string[] GetUsersInRole(string roleName) {
            Debug.WriteLine("GetUsersInRole");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or a whitespace string");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var role = session.Get<Rol>(roleName);
                
                if (role == null)
                    throw new ProviderException("Role does not exist");

                
                var usernames = role.RoluriUtilizator.Select(r => r.Username).ToArray();
                
                session.Transaction.Commit();
                return usernames;
            }
        }

        public override bool IsUserInRole(string username, string roleName) {
            Debug.WriteLine("IsUserInRole");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var repo = new UtilizatorRepository(session);
                var user = repo.GetUserByName(username, null);

                if (user == null)
                    return false;

                var hasRole = user.Roluri.Any(r => r.NumeRol == roleName);
                
                session.Transaction.Commit();
                return hasRole;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            Debug.WriteLine("RemoveUsersFromRoles");

            if (usernames.Any(r => string.IsNullOrWhiteSpace(r)))
                throw new ArgumentException("Username cannot be null or a whitespace string");

            if (roleNames.Any(r => string.IsNullOrWhiteSpace(r)))
                throw new ArgumentException("Role names canno be null or a whitespace string");

            using (var session = SessionManager.OpenSessionTransaction()) {

                var users = session.CreateCriteria<Utilizator>()
                    .Add(Restrictions.In("Username", usernames))
                    .List<Utilizator>();

                var roles = session.CreateCriteria<Rol>()
                    .Add(Restrictions.In("nume_rol", roleNames))
                    .List<Rol>();

                foreach (var user in users)
                    foreach (var role in roles) {
                        user.Roluri.Remove(role);
                    }

                session.Transaction.Commit();
            }
        }

        public override bool RoleExists(string roleName) {
            Debug.WriteLine("RoleExists");

            using (var session = SessionManager.OpenSessionTransaction()) {
                var role =  session.Get<Rol>(roleName);
                session.Transaction.Commit();
                return role != null;
            }
        }
    }
}
