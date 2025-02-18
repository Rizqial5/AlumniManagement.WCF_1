using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JobHistorySerice" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JobHistorySerice.svc or JobHistorySerice.svc.cs at the Solution Explorer and start debugging.
    public class JobHistorySerice : IJobHistorySerice
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public JobHistorySerice()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<JobDTO> GetAll(int alumniId)
        {
            var data = _context.JobHistories.Where(m => m.AlumniID == alumniId).ToList();

            var result = data.Select(x => Mapping.Mapper.Map<JobDTO>(x));

            return result.OrderByDescending(d=> d.ModifiedDate);
        }

        public JobDTO GetJob(int jobId, int alumniId)
        {
            var selectedData = (from m in _context.JobHistories
                                where m.AlumniID == alumniId
                                select m).FirstOrDefault(m => m.JobHistoryID == jobId);

            return Mapping.Mapper.Map<JobDTO>(selectedData);
        }

        public void InsertJob(JobDTO jobDTO, int alumniId)
        {
            var inserData = Mapping.Mapper.Map<JobHistory>(jobDTO);

            inserData.AlumniID = alumniId;
            inserData.ModifiedDate = DateTime.Now;

            _context.JobHistories.InsertOnSubmit(inserData);

            _context.SubmitChanges();
        }

        public void UpdateJob(JobDTO jobDTO, int alumniId)
        {
            var selectedData = (from m in _context.JobHistories
                               where m.AlumniID == alumniId
                               select m).FirstOrDefault(m=> m.JobHistoryID == jobDTO.JobHistoryID);
                

            var updatedData = Mapping.Mapper.Map(jobDTO, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public void DeleteJob(int jobId, int alumniId)
        {
            var selectedData = _context.JobHistories
                .Where(m => m.AlumniID == alumniId)
                .FirstOrDefault(m => m.JobHistoryID == jobId);
                
            

            _context.JobHistories.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();

        }
    }
}
