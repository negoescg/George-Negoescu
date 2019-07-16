using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_System")]
    public class AcsSystem
    {
        [Key]
        public int SystemID { get; set; }
        public string SystemCode { get; set; }
        public string Description { get; set; }
    }
}