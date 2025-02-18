using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFacultyService" in both code and config file together.
    [ServiceContract]
    public interface IFacultyService
    {
        [OperationContract]
        IEnumerable<FacultyDTO> GetAll();

        [OperationContract]
        FacultyDTO GetFaculty(int FacultyId);

        [OperationContract]
        FacultyDTO GetFacultyIdByName(string name);

        [OperationContract]
        void InsertFaculty(FacultyDTO Faculty);

        [OperationContract]
        void UpdateFaculty(FacultyDTO Faculty);

        [OperationContract]
        void DeleteFaculty(int FacultyId);


    }
}
