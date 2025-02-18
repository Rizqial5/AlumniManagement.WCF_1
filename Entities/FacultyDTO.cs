using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class FacultyDTO
    {

        public int FacultyID { get; set; }

        public string FacultyName { get; set; }

        public string Description { get; set; }

        public System.DateTime ModifiedDate { get; set; }

    }
}