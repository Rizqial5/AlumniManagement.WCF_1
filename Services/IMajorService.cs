using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMajorService" in both code and config file together.
    [ServiceContract]
    public interface IMajorService
    {
        [OperationContract]
        IEnumerable<MajorDTO> GetAll();

        [OperationContract]
        MajorDTO GetMajor(int majorId);

        [OperationContract]
        MajorDTO GetMajorIdByName(string name);

        [OperationContract]
       IEnumerable<MajorDTO> GetMajorIdByFacultyId(int facultyId);

        [OperationContract]
        void InsertMajor(MajorDTO major);

        [OperationContract]
        void UpdateMajor(MajorDTO major);

        [OperationContract]
        void DeleteMajor(int majorId);
    }
}
