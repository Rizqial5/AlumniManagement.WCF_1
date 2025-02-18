using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJobHistorySerice" in both code and config file together.
    [ServiceContract]
    public interface IJobHistorySerice
    {
        [OperationContract]
        IEnumerable<JobDTO> GetAll(int alumniId);

        [OperationContract]
        JobDTO GetJob(int jobId, int alumniId);
        [OperationContract]
        void InsertJob(JobDTO jobDTO, int alumniId);

        [OperationContract]
        void UpdateJob(JobDTO jobDTO, int alumniId);

        [OperationContract]
        void DeleteJob(int jobId, int alumniId);
    }
}
