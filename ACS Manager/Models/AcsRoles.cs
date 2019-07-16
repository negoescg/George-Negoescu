using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_Role")]
    public class AcsRoles
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
    }
}