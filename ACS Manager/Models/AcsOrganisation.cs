using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_Organisation")]
    public class AcsOrganisation
    {
        [Key]
        public int OrganisationID { get; set; }
        public string OrgName { get; set; }
    }
}