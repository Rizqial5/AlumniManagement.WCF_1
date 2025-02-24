using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PostingJobService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PostingJobService.svc or PostingJobService.svc.cs at the Solution Explorer and start debugging.
    public class PostingJobService : IPostingJobService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public PostingJobService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<EmploymentTypeDTO> GetEmploymentTypes()
        {
            var data = _context.EmploymentTypes.ToList();

            var result = data.Select(x => Mapping.Mapper.Map<EmploymentTypeDTO>(x));



            return result;
        }


        public IEnumerable<JobPostingDTO> GetJobPostings()
        {
            var data = _context.JobPostings.ToList();

            var result = data.Select(x => new JobPostingDTO
            {
                JobID = x.JobID,
                Title = x.Title,
                Company = x.Company,
                JobDescription = x.JobDescription,
                EmploymentTypeID = x.EmploymentTypeID,
                MinimumExperience = x.MinimumExperience,
                ModifiedDate = x.ModifiedDate,
                IsActive = x.IsActive,
                IsClosed = x.IsClosed,
                TotalCandidates = x.JobCandidates.Count(),
                SelectedAttachmentTypes = x.JobAttachmentTypes.Select(jat => (int)jat.AttachmentTypeID).ToList(),
                SelectedSkills = x.JobSkills.Select(s => (int)s.SkillID).ToList(),
                ActiveDetails = x.IsActive ? "Active" : "Inactive",
                ClosedDetails = x.IsClosed ? "Open" : "Closed"
            });

    

            return result.OrderByDescending(f => f.ModifiedDate);
        }

        public IEnumerable<SkillDTO> GetSkills()
        {
            var data = _context.Skills.ToList();

            var result = data.Select(x => Mapping.Mapper.Map<SkillDTO>(x));



            return result;
        }

        public IEnumerable<AttachmentTypeDTO> GetAttachmentTypes()
        {
            var data = _context.AttachmentTypes.ToList();

            var result = data.Select(x => Mapping.Mapper.Map<AttachmentTypeDTO>(x));



            return result;
        }

        public JobPostingDTO GetJobPosting(Guid jobId)
        {
            var selectedData = _context.JobPostings.FirstOrDefault(j => j.JobID == jobId);

            var result = new JobPostingDTO
            {
                JobID = selectedData.JobID,
                Title = selectedData.Title,
                Company = selectedData.Company,
                JobDescription = selectedData.JobDescription,
                EmploymentTypeID = selectedData.EmploymentTypeID,
                MinimumExperience = selectedData.MinimumExperience,
                ModifiedDate = selectedData.ModifiedDate,
                IsActive = selectedData.IsActive,
                IsClosed = selectedData.IsClosed,
                TotalCandidates = selectedData.JobCandidates.Count(),
                SelectedAttachmentTypes = selectedData.JobAttachmentTypes.Select(jat => (int)jat.AttachmentTypeID).ToList(),
                SelectedSkills = selectedData.JobSkills.Select(s => (int)s.SkillID).ToList(),
                ActiveDetails = selectedData.IsActive ? "Active" : "Inactive",
                ClosedDetails = selectedData.IsClosed ? "Open" : "Closed"
            };


            return result;
        }

        public void InsertJobPosting(JobPostingDTO jobPostingDTO)
        {
            var inserData = Mapping.Mapper.Map<JobPosting>(jobPostingDTO);

            inserData.ModifiedDate = DateTime.Now;

            _context.JobPostings.InsertOnSubmit(inserData);

            _context.SubmitChanges();


            // insert job skill

            var newJobSkills = new List<JobSkill>();

            foreach (var item in jobPostingDTO.SelectedSkills)
            {
                var jobSkill = new JobSkill
                {
                    JobID = inserData.JobID,
                    SkillID = (byte)item
                };

                newJobSkills.Add(jobSkill);
            }

            _context.JobSkills.InsertAllOnSubmit(newJobSkills);
            _context.SubmitChanges();

            // insert job attachmenet

            var newAttachmentjobs = new List<JobAttachmentType>();

            foreach (var item in jobPostingDTO.SelectedAttachmentTypes)
            {
                var attachmenJob = new JobAttachmentType
                {
                    JobID = inserData.JobID,
                    AttachmentTypeID = (byte)item
                };

                newAttachmentjobs.Add(attachmenJob);
            }

            _context.JobAttachmentTypes.InsertAllOnSubmit(newAttachmentjobs);
            _context.SubmitChanges();

        }

        public void UpdateJobPosting(JobPostingDTO jobPostingDTO)
        {
            var selectedData = _context.JobPostings.FirstOrDefault(m => m.JobID == jobPostingDTO.JobID);

            var updatedData = Mapping.Mapper.Map(jobPostingDTO, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();

            // edit job skills

            var exisitingJobSkills = _context.JobSkills
                .Where(js => js.JobID == jobPostingDTO.JobID)
                .Select(js => (int) js.SkillID).ToList();

            var jobSkillsToAdd = jobPostingDTO.SelectedSkills.Except(exisitingJobSkills).ToList();
            var jobSkillsToRemove = exisitingJobSkills.Except(jobPostingDTO.SelectedSkills.ToList());

            foreach (var item in jobSkillsToAdd)
            {
                var jobSkill = new JobSkill
                {
                    JobID = jobPostingDTO.JobID,
                    SkillID = (byte)item
                };

                _context.JobSkills.InsertOnSubmit(jobSkill);
            }

            foreach (var item in jobSkillsToRemove)
            {
                var jobSKill = _context.JobSkills
                    .FirstOrDefault(ah => ah.JobID == jobPostingDTO.JobID && ah.SkillID == item);

                if (jobSKill != null)
                {
                    _context.JobSkills.DeleteOnSubmit(jobSKill);
                }
            }

            _context.SubmitChanges();


            // edit attachtment skills

            var existingAttachment = _context.JobAttachmentTypes
                .Where(js => js.JobID == jobPostingDTO.JobID)
                .Select(js => (int)js.AttachmentTypeID).ToList();

            var attachmentJobToAdd = jobPostingDTO.SelectedAttachmentTypes.Except(existingAttachment).ToList();
            var attachmentJobToRemove = existingAttachment.Except(jobPostingDTO.SelectedAttachmentTypes.ToList());

            foreach (var item in attachmentJobToAdd)
            {
                var attachmenJob = new JobAttachmentType
                {
                    JobID = jobPostingDTO.JobID,
                    AttachmentTypeID = (byte)item
                };

                _context.JobAttachmentTypes.InsertOnSubmit(attachmenJob);
            }

            foreach (var item in attachmentJobToRemove)
            {
                var attachmentJobType = _context.JobAttachmentTypes
                    .FirstOrDefault(ah => ah.JobID == jobPostingDTO.JobID && ah.AttachmentTypeID == item);

                if (attachmentJobType != null)
                {
                    _context.JobAttachmentTypes.DeleteOnSubmit(attachmentJobType);
                }
            }

            _context.SubmitChanges();
        }

        public void DeletingJobPosting(Guid jobId)
        {
            var selectedJObSkills = _context.JobPostings.FirstOrDefault(js => js.JobID == jobId).JobSkills;
            var selectedAttachMEntTypes = _context.JobPostings.FirstOrDefault(js => js.JobID == jobId).JobAttachmentTypes;


            if (selectedJObSkills != null)
            {
                _context.JobSkills.DeleteAllOnSubmit(selectedJObSkills);
                _context.SubmitChanges();
            }

            if (selectedJObSkills != null)
            {
                _context.JobAttachmentTypes.DeleteAllOnSubmit(selectedAttachMEntTypes);
                _context.SubmitChanges();
            }

            var selectedData = _context.JobPostings.FirstOrDefault(m => m.JobID == jobId);
            _context.JobPostings.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();

        }

        public void InsertApplyJob(JobAttachmentDTO jobAttachmentDTO)
        {
            var data = Mapping.Mapper.Map<JobAttachment>(jobAttachmentDTO);

            //insert alumni attachment to the job
            _context.JobAttachments.InsertOnSubmit(data);

            _context.SubmitChanges();

            // insert alumni data to candidates data
            var candidatesData = new JobCandidate
            {
                JobID = jobAttachmentDTO.JobID,
                AlumniID = jobAttachmentDTO.AlumniID,
                ApplyDate = DateTime.Now
            };

            _context.JobCandidates.InsertOnSubmit(candidatesData);

            _context.SubmitChanges();

        }

        public IEnumerable<JobCandidateDTO> GetAllCandidateBYJObId(Guid jobID)
        {
            var data = from c in _context.JobCandidates
                       where c.JobID == jobID
                       select c;

            var result = data.ToList().Select(d => new JobCandidateDTO
            {
                JobID = d.JobID,
                ApplyDate = d.ApplyDate,
                FullName = d.Alumni.FirstName + " " + (d.Alumni.MiddleName ?? "") + " " + d.Alumni.LastName,
                JobAttachments = d.JobPosting.JobAttachments.Where(ja=> ja.AlumniID == d.AlumniID).Select(ja => new JobAttachmentDTO
                {
                    FilePath = ja.FilePath,
                    FileName = ja.FileName,
                }  
                ).ToList(),
                AlumniId = d.AlumniID
            });
                       
    
            return result;
        }

        public IEnumerable<SkillDTO> GetSkillsByJobId(Guid jobId)
        {
            var data = _context.JobPostings.FirstOrDefault(j => j.JobID == jobId)
                .JobSkills.Select(js=> js.Skill);

            var result = Mapping.Mapper.Map<List<SkillDTO>>(data);

            return result;
        }

        public void UpsertJobPosting(JobPostingDTO jobPostingDTO)
        {
            try
            {
                var jobSkills = new DataTable();
                jobSkills.Columns.Add("JobID", typeof(Guid));
                jobSkills.Columns.Add("SkillID", typeof(byte));

                foreach (var skill in jobPostingDTO.SelectedSkills)
                {
                    jobSkills.Rows.Add(jobPostingDTO.JobID, skill);
                }

                var jobAttachments = new DataTable();
                jobAttachments.Columns.Add("JobID", typeof(Guid));
                jobAttachments.Columns.Add("AttachmentTypeID", typeof(byte));

                foreach (var attachment in jobPostingDTO.SelectedAttachmentTypes)
                {
                    jobAttachments.Rows.Add(jobPostingDTO.JobID, attachment);
                }

                using (var connection = new SqlConnection(_context.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertJobPosting", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@JobID", SqlDbType.UniqueIdentifier)
                        {
                            Value = jobPostingDTO.JobID == Guid.Empty ? (object)DBNull.Value : jobPostingDTO.JobID
                        });
                        command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 255) { Value = jobPostingDTO.Title });
                        command.Parameters.Add(new SqlParameter("@Company", SqlDbType.NVarChar, 255) { Value = jobPostingDTO.Company });
                        command.Parameters.Add(new SqlParameter("@JobDescription", SqlDbType.NVarChar, -1) { Value = jobPostingDTO.JobDescription });
                        command.Parameters.Add(new SqlParameter("@EmploymentTypeID", SqlDbType.TinyInt) { Value = jobPostingDTO.EmploymentTypeID });
                        command.Parameters.Add(new SqlParameter("@MinimumExperience", SqlDbType.TinyInt) { Value = jobPostingDTO.MinimumExperience });
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });
                        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = jobPostingDTO.IsActive });
                        command.Parameters.Add(new SqlParameter("@IsClosed", SqlDbType.Bit) { Value = jobPostingDTO.IsClosed });

                        // Parameter untuk JobSkills
                        command.Parameters.Add(new SqlParameter("@JobSkills", SqlDbType.Structured)
                        {
                            TypeName = "dbo.JobSkillType",
                            Value = jobSkills
                        });

                        // Parameter untuk JobAttachments
                        command.Parameters.Add(new SqlParameter("@JobAttachments", SqlDbType.Structured)
                        {
                            TypeName = "dbo.JobAttachmentType",
                            Value = jobAttachments
                        });

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
