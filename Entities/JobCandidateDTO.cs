using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class JobCandidateDTO
    {
        
        public Guid JobID { get; set; }
        public DateTime ApplyDate { get; set; }

        public string FullName { get; set; }


        public int AlumniId { get; set; }
        public List<JobAttachmentDTO> JobAttachments{get; set;}
        

    }
}