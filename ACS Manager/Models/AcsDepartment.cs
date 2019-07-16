using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_Department")]
    public class AcsDepartment
    {
        [Key]
        public int DepartmentID { get; set; }
        public int OrganisationID { get; set; }
        public string Name { get; set; }
    }
}