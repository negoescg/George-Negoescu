using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table ("GI_User")]
    public class AcsUser
    {
        [Key]
        public int UserID { get; set; }
        public int OrganisationID { get; set; }
        public int DepartmentID { get; set; }
        public string WindowsUserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string FormsPassword { get; set; }
    }
}