using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_UserRole")]
    public class AcsUserRoles
    {
        public int SystemID { get; set; }
        public int DatabaseID { get; set; }
        public int UserID { get; set; }
        [Key]
        public int RoleID { get; set; }
    }
}