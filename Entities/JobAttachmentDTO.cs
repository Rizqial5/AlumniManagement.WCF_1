using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class JobAttachmentDTO
    {
        public int AttachmentID { get; set; }
        public Guid JobID { get; set; }
        public int AlumniID { get; set; }
        public byte AttachmentTypeID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}