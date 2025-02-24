using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class JobSkillDTO
    {
        public Guid JobID { get; set; }
        public byte SkillID { get; set; }
    }
}