using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    [DataContract]
    public class JobPostingDTO
    {
        [DataMember]
        public Guid JobID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string JobDescription { get; set; }
        [DataMember]
        public byte EmploymentTypeID { get; set; }
        [DataMember]
        public byte MinimumExperience { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public List<int> SelectedAttachmentTypes { get; set; }

        [DataMember]
        public List<int> SelectedSkills { get; set; }

        //
        [DataMember]
        [Ignore]
        public int TotalCandidates { get; set; }

        [DataMember]
        [Ignore]
        public string ActiveDetails { get; set; }

        [DataMember]
        [Ignore]
        public string ClosedDetails { get; set; }


    }
}