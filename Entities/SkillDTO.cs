using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class SkillDTO
    {
        public byte SkillID { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}