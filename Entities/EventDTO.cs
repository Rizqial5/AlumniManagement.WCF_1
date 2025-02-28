using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class EventDTO
    {
        public int EventID{get; set;}

        public string Title{get; set;}

        public string Description{get; set;}

        public string EventImagePath{get; set;}

        public string EventImageName{get; set;}

        public System.DateTime StartDate{get; set;}

        public System.DateTime EndDate{get; set;}

        public bool IsClosed{get; set;}

        public System.DateTime ModifiedDate{get; set;}
    }
}