using AlumniManagement.WCF.Entities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlumniService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AlumniService.svc or AlumniService.svc.cs at the Solution Explorer and start debugging.
    public class AlumniService : IAlumniService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public AlumniService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<AlumniDTO> GetAll()
        {
            var data = _context.Majors.ToList();

            var result = from a in _context.Alumnis
                         join m in _context.Majors on a.MajorID equals m.MajorID
                         join f in _context.Faculties on m.FacultyID equals f.FacultyID
                         join d in _context.Districts on a.DistrictID equals d.DistrictID
                         join s in _context.States on d.StateID equals s.StateID
                         select new AlumniDTO
                         {
                             AlumniID = a.AlumniID,
                             FirstName = a.FirstName,
                             MiddleName = a.MiddleName,
                             LastName = a.LastName,
                             Email = a.Email,
                             MobileNumber = a.MobileNumber,
                             Address = a.Address,
                             DistrictID = a.DistrictID,
                             DateOfBirth = a.DateOfBirth,
                             GraduationYear = a.GraduationYear,
                             Degree = a.Degree,
                             MajorID = a.MajorID,
                             LinkedInProfile = a.LinkedInProfile,
                             ModifiedDate = a.ModifiedDate,
                             FullName = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName,
                             FullAddress = a.Address + ", " + d.DistrictName + ", " + s.StateName,
                             FacultyName = f.FacultyName,
                             MajorName = m.MajorName,
                             FacultyID = f.FacultyID,
                             StateID = s.StateID,
                             StateName = s.StateName,
                             DistrictName = d.DistrictName
                         };



            return result.OrderByDescending(a => a.ModifiedDate).ToList();
        }

        public AlumniDTO GetAlumni(int alumniId)
        {
            var result = (from a in _context.Alumnis
                            join m in _context.Majors on a.MajorID equals m.MajorID
                            join f in _context.Faculties on m.FacultyID equals f.FacultyID
                            join d in _context.Districts on a.DistrictID equals d.DistrictID
                            join s in _context.States on d.StateID equals s.StateID
                            select new AlumniDTO
                            {
                                AlumniID = a.AlumniID,
                                FirstName = a.FirstName,
                                MiddleName = a.MiddleName,
                                LastName = a.LastName,
                                Email = a.Email,
                                MobileNumber = a.MobileNumber,
                                Address = a.Address,
                                DistrictID = a.DistrictID,
                                DateOfBirth = a.DateOfBirth,
                                GraduationYear = a.GraduationYear,
                                Degree = a.Degree,
                                MajorID = a.MajorID,
                                LinkedInProfile = a.LinkedInProfile,
                                ModifiedDate = a.ModifiedDate,
                                FullName = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName,
                                FullAddress = a.Address + ", " + d.DistrictName + ", " + s.StateName,
                                FacultyName = f.FacultyName,
                                MajorName = m.MajorName,
                                FacultyID = f.FacultyID,
                                StateID = s.StateID
                            }).FirstOrDefault(r=> r.AlumniID == alumniId);

            return Mapping.Mapper.Map<AlumniDTO>(result);
        }

        public int GetDistrictIdByName(string districtName)
        {
            var selectedData = _context.Districts.FirstOrDefault(m => m.DistrictName == districtName);
            
            int districtId= selectedData.DistrictID;

            return districtId;
        }

        public int GetStateIdByName(string stateName)
        {
            var selectedData = _context.States.FirstOrDefault(m => m.StateName == stateName);

            int stateID = selectedData.StateID;

            return stateID;
        }

        public void InsertAlumni(AlumniDTO alumni)
        {

            if(alumni.AlumniID != 0)
            {
                alumni.AlumniID = 0;
            }

            var inserData = Mapping.Mapper.Map<Alumni>(alumni);

            inserData.ModifiedDate = DateTime.Now;

            _context.Alumnis.InsertOnSubmit(inserData);

            _context.SubmitChanges();
        }

        public void UpdateAlumni(AlumniDTO alumni)
        {
            var selectedData = _context.Alumnis.FirstOrDefault(m => m.AlumniID == alumni.AlumniID);

            var updatedData = Mapping.Mapper.Map(alumni, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public void DeleteAlumni(int alumniId)
        {

            var selectedData = _context.Alumnis.FirstOrDefault(m => m.AlumniID == alumniId);

            _context.Alumnis.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();
        }

        public IEnumerable<StateDTO> GetAllStates()
        {
            var statesData = _context.States.ToList();

            var result = statesData.Select(s => Mapping.Mapper.Map<StateDTO>(s));

            return result.ToList();
        }

        public IEnumerable<DistrictDTO> GetDistrictByStateId(int stateId)
        {
            var districts = from d in _context.Districts
                            join s in _context.States on d.StateID equals s.StateID
                            where d.StateID == stateId
                            select d;

            var result = districts.Select(d=> Mapping.Mapper.Map<DistrictDTO>(d));

            return result.ToList();
        }

        public IEnumerable<DistrictDTO> GetAllDistricts()
        {
            var data = _context.Districts.ToList();

            var result = data.Select(d => Mapping.Mapper.Map<DistrictDTO>(d));

            return result.ToList();
        }

        public void ImportFromExcel(AlumniDTO alumniDTO)
        {
            if(CheckAlumniId(alumniDTO.AlumniID))
            {
                UpdateAlumni(alumniDTO);
            }
            else
            {
                if (CheckAlumniData(alumniDTO)) return;

                InsertAlumni(alumniDTO);
            }
        }

        private bool CheckAlumniData(AlumniDTO alumniDTO)
        {
            var dataList = _context.Alumnis.ToList();

            foreach (var item in dataList)
            {
                if(item.FirstName == alumniDTO.FirstName && item.LastName == alumniDTO.LastName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckAlumniId(int alumniId)
        {
            var selectedData = _context.Alumnis.FirstOrDefault(m => m.AlumniID == alumniId);

            if (selectedData == null) return false;

            return true;

        }

        public IEnumerable<string> GetStatesDistrictName()
        {
           

            var districtsStatesName = from d in _context.Districts
                                      join s in _context.States on d.StateID equals s.StateID
                                      select new
                                      {
                                          StatesDistricts = s.StateName + " - " + d.DistrictName
                                      };

      

            return districtsStatesName.Select(s => s.StatesDistricts).ToList();
        }

        public IEnumerable<string> GetMajorFacultiesName()
        {
            var majorFacultiesName = from m in _context.Majors
                                     join f in _context.Faculties on m.FacultyID equals f.FacultyID
                                     select new
                                     {
                                         MajorFaculties = f.FacultyName + " - " + m.MajorName
                                     };

            return majorFacultiesName.Select(s=> s.MajorFaculties).ToList();
        }
    }
}
