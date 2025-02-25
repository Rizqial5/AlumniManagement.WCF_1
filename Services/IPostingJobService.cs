using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPostingJobService" in both code and config file together.
    [ServiceContract]
    public interface IPostingJobService
    {
        [OperationContract]
        IEnumerable<JobPostingDTO> GetJobPostings();

        [OperationContract]
        IEnumerable<EmploymentTypeDTO> GetEmploymentTypes();
        [OperationContract]
        IEnumerable<JobCandidateDTO> GetAllCandidateBYJObId(Guid jobID);

        [OperationContract]
        IEnumerable<SkillDTO> GetSkills();

        [OperationContract]
        IEnumerable<SkillDTO> GetSkillsByJobId(Guid jobId);

        [OperationContract]
        IEnumerable<AttachmentTypeDTO> GetAttachmentTypes();

        [OperationContract]
        JobPostingDTO GetJobPosting(Guid jobId);

        [OperationContract]
        void InsertJobPosting(JobPostingDTO jobPostingDTO);

        [OperationContract]
        void InsertApplyJob(List<JobAttachmentDTO> jobAttachmentDTO, Guid jobId, int alumniId);

        [OperationContract]
        void UpdateJobPosting(JobPostingDTO jobPostingDTO);

        [OperationContract]
        void DeletingJobPosting(Guid jobId);

        [OperationContract]
        void UpsertJobPosting(JobPostingDTO jobPostingDTO);

        [OperationContract]
        void UpsertJobPostingList(List<JobPostingDTO> jobPostingDTO);
    }
}
