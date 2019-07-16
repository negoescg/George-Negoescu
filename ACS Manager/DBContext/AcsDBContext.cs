using ACS_Manager.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ACS_Manager.DBContext
{
    public class AcsDBContext
    {
        public List<AcsUser> Users { get; set; }
        public List<AcsUserRoles> UserRoles { get; set; }
        public List<AcsRoles> Roles { get; set; }
        public List<AcsDatabase> Databases { get; set; }
        public List<AcsSystem> Systems { get; set; }
        public List<AcsOrganisation> Organisations { get; set; }
        public List<AcsDepartment> Departments { get; set; }
        public List<AcsSystemDatabase> SystemDatabases { get; set; }
        public AcsDBContext()
        {
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                Users = connection.GetAll<AcsUser>().ToList();
                UserRoles = connection.GetAll<AcsUserRoles>().ToList();
                Roles = connection.GetAll<AcsRoles>().ToList();
                Databases = connection.GetAll<AcsDatabase>().ToList();
                Systems = connection.GetAll<AcsSystem>().ToList();
                Organisations = connection.GetAll<AcsOrganisation>().ToList();
                Departments = connection.GetAll<AcsDepartment>().ToList();
                SystemDatabases = connection.Query<AcsSystemDatabase>(SelectSystemDatabase).ToList();
            }
        }

        public void UpdateUser(User user)
        {
            var acsUser = new AcsUser
            {
                UserID = user.UserID,
                OrganisationID = user.OrganisationID,
                DepartmentID = user.DepartmentID,
                WindowsUserName = user.WindowsUserName,
                Email = user.Email,
                Name = user.Name,
                Telephone = user.Telephone,
                FormsPassword = user.FormsPassword
            };
            if(user.NewPassword != null && user.UserStatus == 1)
            {
                acsUser.FormsPassword = GetPasswordHash(user.NewPassword);
            }
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                if(user.UserStatus == 0)
                {
                    acsUser.FormsPassword = "Disabled";
                }
                connection.Update(acsUser);
            }
        }
        public void DeleteUserRole(AcsUserRoles role)
        {
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                string sqlDelete = "DELETE FROM GI_UserRole WHERE SystemID = @oldSystemID AND DatabaseID = @oldDatabaseID AND UserID = @oldUserID AND RoleID = @oldRoleID";

                connection.Query<AcsUserRoles>(sqlDelete, new { oldSystemID = role.SystemID, oldDatabaseID = role.DatabaseID, oldUserID = role.UserID, oldRoleID = role.RoleID });
            }
        }
        public void DeleteUserRoles(List<AcsUserRoles> roles)
        {
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                foreach(var role in roles)
                {
                    string sqlDelete = "DELETE FROM GI_UserRole WHERE SystemID = @oldSystemID AND DatabaseID = @oldDatabaseID AND UserID = @oldUserID AND RoleID = @oldRoleID";

                    connection.Query<AcsUserRoles>(sqlDelete, new { oldSystemID = role.SystemID, oldDatabaseID = role.DatabaseID, oldUserID = role.UserID, oldRoleID = role.RoleID });
                }
            }
        }
        public void UpdateUserRole(List<AcsUserRoles> role)
        {
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                string sqlUpdate = "UPDATE GI_UserRole SET SystemID = @newSystemID,DatabaseID = @newDatabaseID,UserID = @newUserID,RoleID = @newRoleID WHERE SystemID = @oldSystemID AND DatabaseID = @oldDatabaseID AND UserID = @oldUserID AND RoleID = @oldRoleID";

                connection.Query<AcsUserRoles>(sqlUpdate, new { newSystemID = role[1].SystemID, newDatabaseID = role[1].DatabaseID, newUserID = role[1].UserID, newRoleID = role[1].RoleID, oldSystemID = role[0].SystemID, oldDatabaseID = role[0].DatabaseID, oldUserID = role[0].UserID, oldRoleID = role[0].RoleID });
            }
        }
        public void AddUserRole(AcsUserRoles role, bool insert)
        {
            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                string sqlSystemDatabase = "INSERT INTO GI_SystemDatabase (SystemID, DatabaseID) VALUES(@systemID, @databaseID)";

                if (insert)
                {
                    connection.Query<AcsUserRoles>(sqlSystemDatabase, new { systemID = role.SystemID, databaseID = role.DatabaseID});
                }
                string sqlRole = "INSERT INTO GI_UserRole (SystemID, DatabaseID, UserID, RoleID) VALUES(@systemID, @databaseID, @userID, @roleID)";
                connection.Query<AcsUserRoles>(sqlRole, new { systemID = role.SystemID, databaseID = role.DatabaseID, userID = role.UserID, roleID = role.RoleID });
            }
        }
        public void AddUser(User user)
        {
            var acsUser = new AcsUser
            {
                OrganisationID = user.OrganisationID,
                DepartmentID = user.DepartmentID,
                WindowsUserName = user.WindowsUserName,
                Email = user.Email,
                Name = user.Name,
                Telephone = user.Telephone,
                FormsPassword = GetPasswordHash(user.NewPassword)
            };

            using (var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                if (user.UserStatus == 0)
                {
                    acsUser.FormsPassword = "Disabled";
                }
                connection.Insert(acsUser);
            }
        }

        public void CloneUserRoles(List<AcsUserRoles> roles)
        {
            using(var connection = new SqlConnection(SqlItems.ConnectionString))
            {
                foreach( var role in roles)
                {
                    string sqlRole = "INSERT INTO GI_UserRole (SystemID, DatabaseID, UserID, RoleID) VALUES(@systemID, @databaseID, @userID, @roleID)";
                    connection.Query<AcsUserRoles>(sqlRole, new { systemID = role.SystemID, databaseID = role.DatabaseID, userID = role.UserID, roleID = role.RoleID });
                }
            }
        }

        public string GetPasswordHash(string plainText)
        {

            Byte[] clearBytes = new UnicodeEncoding().GetBytes(plainText);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }

        #region private variables
        //private readonly string SelectUsers = "SELECT UserID, " +
        //    "OrganisationID, " +
        //    "DepartmentID, " +
        //    "WindowsUserName, " +
        //    "Email, " +
        //    "Name, " +
        //    "Telephone, " +
        //    "FormsPassword " +
        //    "FROM GI_User";
        //private readonly string SelectUserRoles = "SELECT SystemID, " +
        //    "DatabaseID, " +
        //    "UserID, " +
        //    "RoleID " +
        //    "FROM GI_UserRole";
        //private readonly string SelectRoles = "SELECT RoleID, " +
        //    "RoleCode, " +
        //    "Description " +
        //    "FROM GI_Role";
        //private readonly string SelectDatabases = "SELECT DatabaseID, " +
        //    "DatabaseCode, " +
        //    "Description " +
        //    "FROM GI_Database";
        //private readonly string SelectSystems = "SELECT SystemID, " +
        //    "SystemCode, " +
        //    "Description " +
        //    "FROM GI_System";
        //private readonly string SelectOrganisations = "SELECT OrganisationID, " +
        //    "OrgName " +
        //    "FROM GI_Organisation";
        //private readonly string SelectDepartments = "SELECT DepartmentID, " +
        //    "OrganisationID, " +
        //    "Name " +
        //    "FROM GI_Department";
        private readonly string SelectSystemDatabase = "SELECT SystemID, " +
            "DatabaseID " +
            "FROM GI_SystemDatabase";
        #endregion
    }
}