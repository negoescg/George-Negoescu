using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS_Manager.Models
{
    [Table("GI_Database")]
    public class AcsDatabase
    {
        [Key]
        public int DatabaseID { get; set; }
        public string DatabaseCode { get; set; }
        public string Description { get; set; }
    }
}