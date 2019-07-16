using ACS_Manager.Models;
using ACS_Manager.DBContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACS_Manager.Controllers
{
    public class UsersController : Controller
    {
        private readonly AcsDBContext db = new AcsDBContext();
                     
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users);
        }
        [HttpGet]
        public JsonResult GetUsers()
        {
            var acsUsers = db.Users;

            var users = new List<User>();

            foreach(var acsUser in acsUsers)
            {
                var user = new User
                {
                    UserID = acsUser.UserID,
                    OrganisationID = acsUser.OrganisationID,
                    DepartmentID = acsUser.DepartmentID,
                    WindowsUserName = acsUser.WindowsUserName,
                    Email = acsUser.Email,
                    Name = acsUser.Name,
                    Telephone = acsUser.Telephone,
                    FormsPassword = acsUser.FormsPassword
                };
                if(acsUser.FormsPassword == null || !acsUser.FormsPassword.Contains("-"))
                {
                    user.UserStatus = 0;
                    users.Add(user);
                }
                else
                {
                    user.UserStatus = 1;
                    users.Add(user);
                }
            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserRoles(int userId)
        {
            var userRoles = db.UserRoles.Where(x => x.UserID == userId);
            return Json(userRoles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            var roles = db.Roles;
            return Json(roles, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDatabases()
        {
            var databases = db.Databases;
            return Json(databases, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSystems()
        {
            var systems = db.Systems;
            return Json(systems, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrganisations()
        {
            var organisations = db.Organisations;
            return Json(organisations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            var deparments = db.Departments;
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUsers(User user)
        {
            db.UpdateUser(user);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddNewUser(User user)
        {
            db.AddUser(user);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUserRole(AcsUserRoles role)
        {
            db.DeleteUserRole(role);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddNewUserRole(AcsUserRoles role)
        {
            var systemDatabase = db.SystemDatabases.Select(z => new AcsSystemDatabase
            {
                SystemID = z.SystemID,
                DatabaseID = z.DatabaseID
            }).Where(x => x.SystemID == role.SystemID && x.DatabaseID == role.DatabaseID).FirstOrDefault();

            if(systemDatabase == null)
            {
                db.AddUserRole(role, true);
            }
            else
            {
                db.AddUserRole(role, false);
            }
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateUserRole(List<AcsUserRoles> role)
        {
            db.UpdateUserRole(role);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CloneUserRoles(List<AcsUserRoles> roles)
        {
            db.CloneUserRoles(roles);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUserRoles(List<AcsUserRoles> roles)
        {
            db.DeleteUserRoles(roles);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddRolesToSelectedUser(List<AcsUserRoles> roles)
        {
            var targetUserID = roles[roles.Count - 1].UserID;
            var userRoles = db.UserRoles.Where(x => x.UserID == targetUserID).ToList();
            roles.RemoveAt(roles.Count - 1);
            var addRoleToUser = new List<AcsUserRoles>(roles);
            List<int> indexes = new List<int>();
            for (int i = 0; i < roles.Count; i++)
            {
                roles[i].UserID = targetUserID;
                if (userRoles.Where(x => x.SystemID == roles[i].SystemID && x.DatabaseID == roles[i].DatabaseID && x.UserID == roles[i].UserID && x.RoleID == roles[i].RoleID).Any())
                {
                    indexes.Add(i);
                }
            }
            for(int i = 0; i < indexes.Count; i++)
            {
                roles.Remove(addRoleToUser[indexes[i]]);
            }
            db.CloneUserRoles(roles);
            var deparments = "Success";
            return Json(deparments, JsonRequestBehavior.AllowGet);
        }
    }
}