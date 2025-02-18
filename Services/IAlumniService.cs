using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlumniService" in both code and config file together.
    [ServiceContract]
    public interface IAlumniService
    {
        [OperationContract]
        IEnumerable<AlumniDTO> GetAll();

        [OperationContract]
        AlumniDTO GetAlumni(int alumniId);

        [OperationContract]
        void InsertAlumni(AlumniDTO alumni);

        [OperationContract]
        void UpdateAlumni(AlumniDTO alumni);

        [OperationContract]
        void DeleteAlumni(int alumniId);

        [OperationContract]
        int GetDistrictIdByName(string districtName);

        [OperationContract]
        int GetStateIdByName(string stateName);

        [OperationContract]
        IEnumerable<StateDTO> GetAllStates();

        [OperationContract]
        IEnumerable<DistrictDTO> GetAllDistricts();

        [OperationContract]
        IEnumerable<DistrictDTO> GetDistrictByStateId(int stateId);

        [OperationContract]
        IEnumerable<string> GetStatesDistrictName();
        [OperationContract]
        IEnumerable<string> GetMajorFacultiesName();

        [OperationContract]
        void ImportFromExcel(AlumniDTO alumniDTO);

    }
}
