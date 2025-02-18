using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class JobDTO
    {
        public int JobHistoryID {get; set;}

        public System.Nullable<int> AlumniID {get; set;}

        public string JobTitle{get; set;}

        public string Company{get; set;}

        public System.Nullable<System.DateTime> StartDate{get; set;}

        public System.Nullable<System.DateTime> EndDate{get; set;}

        public string Description{get; set;}

        public System.DateTime ModifiedDate{get; set;}
    }
}