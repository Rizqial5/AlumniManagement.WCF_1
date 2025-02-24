using AlumniManagement.WCF.Entities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Configuration;
using System.Data.Linq;

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
                             //join m in _context.Majors on a.MajorID equals m.MajorID
                             //join f in _context.Faculties on m.FacultyID equals f.FacultyID
                             //join d in _context.Districts on a.DistrictID equals d.DistrictID
                             //join s in _context.States on d.StateID equals s.StateID
                             //join ah in _context.AlumniHobbies on a.AlumniID equals ah.AlumniID 
                             ////from ah in alumniHobbies.DefaultIfEmpty()
                             //join h in _context.Hobbies on ah.HobbyID equals h.HobbyID into 
                             //from h in hobbies.DefaultIfEmpty()
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
                             FullAddress = a.Address + ", " + a.District.DistrictName + ", " + a.District.State.StateName,
                             FacultyName = a.Major.Faculty.FacultyName,
                             MajorName = a.Major.MajorName,
                             FacultyID = a.Major.Faculty.FacultyID,
                             StateID = a.District.State.StateID,
                             StateName = a.District.State.StateName,
                             DistrictName = a.District.DistrictName,
                             Hobbies = a.AlumniHobbies.Select(h => h.HobbyID).ToList(),
                             HobbiesListName = String.Join(",", a.AlumniHobbies
                             .Select(ah => ah.Hobby)
                             .Select(h => h.Name)),
                             PhotoName = a.PhotoName,
                             PhotoPath = a.PhotoPath

                         };






            foreach (var item in result)
            {
                var hobies = GetTotalHobbies(item);

                item.Hobbies = hobies.Select(h => h.HobbyID);
                item.HobbiesListName = String.Join(",", hobies.Select(h => h.Name));
            }


            return result.OrderByDescending(a => a.ModifiedDate).ToList();
        }

        private List<HobbyDTO> GetTotalHobbies(AlumniDTO alumni)
        {
            var result = new List<HobbyDTO>();

            var hobbiesResult = from ah in _context.AlumniHobbies
                                join h in _context.Hobbies on ah.HobbyID equals h.HobbyID
                                select new
                                {
                                    AlumniID = ah.AlumniID,
                                    HobbyID = h.HobbyID,
                                    HobbyName = h.Name
                                };

            foreach (var item in hobbiesResult)
            {
                if (item.AlumniID == alumni.AlumniID)
                {
                    var newHobbyDTO = new HobbyDTO
                    {
                        HobbyID = item.HobbyID,
                        Name = item.HobbyName
                    };

                    result.Add(newHobbyDTO);
                }
            }

            return result;
        }


        public AlumniDTO GetAlumni(int alumniId)
        {
            var result = GetAll().FirstOrDefault(r => r.AlumniID == alumniId);

            return Mapping.Mapper.Map<AlumniDTO>(result);
        }

        public int GetDistrictIdByName(string districtName)
        {
            var selectedData = _context.Districts.FirstOrDefault(m => m.DistrictName == districtName);

            int districtId = selectedData.DistrictID;

            return districtId;
        }

        public int GetStateIdByName(string stateName)
        {
            var selectedData = _context.States.FirstOrDefault(m => m.StateName == stateName);

            int stateID = selectedData.StateID;

            return stateID;
        }

        #region Insert Behaviour
        //Insert Behaviour

        //
        // Summary:
        //     Insert alumni data 
        public void InsertAlumni(AlumniDTO alumni)
        {

            if (alumni.AlumniID != 0)
            {
                alumni.AlumniID = 0;
            }

            InsertAlumniIntoDatabase(alumni);

            _context.SubmitChanges();
        }

        private Alumni InsertAlumniIntoDatabase(AlumniDTO alumni)
        {
            var inserData = Mapping.Mapper.Map<Alumni>(alumni);


            inserData.ModifiedDate = DateTime.Now;

            _context.Alumnis.InsertOnSubmit(inserData);

            return inserData;
        }


        //
        // Summary:
        //     Insert alumni data with list of hobbies
        public void InsertAlumniWithHobbies(AlumniDTO alumni)
        {
            if (alumni.AlumniID != 0)
            {
                alumni.AlumniID = 0;
            }

            var insertedAlumni = InsertAlumniIntoDatabase(alumni);

            // submitting new alumni
            _context.SubmitChanges();


            var newAlumniHobbies = new List<AlumniHobby>();

            foreach (var item in alumni.Hobbies)
            {
                var alumnyHobby = new AlumniHobby
                {
                    AlumniID = insertedAlumni.AlumniID,
                    HobbyID = item
                };

                newAlumniHobbies.Add(alumnyHobby);
            }

            _context.AlumniHobbies.InsertAllOnSubmit(newAlumniHobbies);
            _context.SubmitChanges();
        }

        #endregion


        #region Update Behaviour
        public void UpdateAlumni(AlumniDTO alumni)
        {
            UpdateAlumniIntoDatabase(alumni);

            _context.SubmitChanges();
        }

        private void UpdateAlumniIntoDatabase(AlumniDTO alumni)
        {
            var selectedData = _context.Alumnis.FirstOrDefault(m => m.AlumniID == alumni.AlumniID);

            var updatedData = Mapping.Mapper.Map(alumni, selectedData);

            updatedData.ModifiedDate = DateTime.Now;
        }

        public void UpdateAlumniWithHobbies(AlumniDTO alumni)
        {
            UpdateAlumniIntoDatabase(alumni);

            _context.SubmitChanges();

            var existingHobbies = _context.AlumniHobbies
                .Where(ah => ah.AlumniID == alumni.AlumniID)
                .Select(ah => ah.HobbyID).ToList();


            //Klasifikasi hobbies yang di add dan remove
            var hobbiesToAdd = alumni.Hobbies.Except(existingHobbies).ToList();
            var hobbiesToRemove = existingHobbies.Except(alumni.Hobbies.ToList());

            foreach (var item in hobbiesToAdd)
            {
                var alumniHobby = new AlumniHobby
                {
                    AlumniID = alumni.AlumniID,
                    HobbyID = item
                };

                _context.AlumniHobbies.InsertOnSubmit(alumniHobby);
            }

            foreach (var item in hobbiesToRemove)
            {
                var alumniHobby = _context.AlumniHobbies
                    .FirstOrDefault(ah => ah.AlumniID == alumni.AlumniID && ah.HobbyID == item);
                if (alumniHobby != null)
                {
                    _context.AlumniHobbies.DeleteOnSubmit(alumniHobby);
                }
            }

            _context.SubmitChanges();
        }

        #endregion

        public void DeleteAlumni(int alumniId)
        {

            //delete hobbies

            var selectedAh = _context.AlumniHobbies.FirstOrDefault(ah => ah.AlumniID == alumniId);

            if (selectedAh != null)
            {
                _context.AlumniHobbies.DeleteOnSubmit(selectedAh);
                _context.SubmitChanges();
            }

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

            var result = districts.Select(d => Mapping.Mapper.Map<DistrictDTO>(d));

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
            if (CheckAlumniId(alumniDTO.AlumniID))
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
                if (item.FirstName == alumniDTO.FirstName && item.LastName == alumniDTO.LastName)
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

            return majorFacultiesName.Select(s => s.MajorFaculties).ToList();
        }

        public IEnumerable<HobbyDTO> GetAllHobbies()
        {
            var data = _context.Hobbies.ToList();

            var result = Mapping.Mapper.Map<IEnumerable<HobbyDTO>>(data);

            return result;
        }

        public IEnumerable<HobbyDTO> GetAllHobbiesByAlumniId(int alumniId)
        {
            var data = from a in _context.Alumnis
                       join ah in _context.AlumniHobbies on a.AlumniID equals ah.AlumniID
                       join h in _context.Hobbies on ah.HobbyID equals h.HobbyID
                       where a.AlumniID == alumniId
                       select h;


            var result = Mapping.Mapper.Map<IEnumerable<HobbyDTO>>(data.ToList());

            return result;
        }

        public void UpsertAlumni(AlumniDTO alumni)
        {
            try
            {
                var alumniHobbies = new DataTable();
                alumniHobbies.Columns.Add("AlumniID", typeof(int));
                alumniHobbies.Columns.Add("HobbyID", typeof(int));

                foreach (var item in alumni.Hobbies)
                {
                    alumniHobbies.Rows.Add(alumni.AlumniID, item);
                }

                using (var connection = new SqlConnection(_context.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertAlumni", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@AlumniID", SqlDbType.Int) { Value = (object)alumni.AlumniID ?? 0 });
                        command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = alumni.FirstName });
                        command.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50) { Value = (object)alumni.MiddleName ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = alumni.LastName });
                        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = alumni.Email });
                        command.Parameters.Add(new SqlParameter("@MobileNumber", SqlDbType.NVarChar, 15) { Value = alumni.MobileNumber });
                        command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = alumni.Address });
                        command.Parameters.Add(new SqlParameter("@DistrictID", SqlDbType.Int) { Value = alumni.DistrictID });
                        command.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = (object)alumni.DateOfBirth ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@GraduationYear", SqlDbType.Int) { Value = alumni.GraduationYear });
                        command.Parameters.Add(new SqlParameter("@Degree", SqlDbType.NVarChar, 100) { Value = (object)alumni.Degree ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@MajorID", SqlDbType.Int) { Value = alumni.MajorID });
                        command.Parameters.Add(new SqlParameter("@LinkedInProfile", SqlDbType.NVarChar, 255) { Value = (object)alumni.LinkedInProfile ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@PhotoPath", SqlDbType.NVarChar, 255) { Value = (object)alumni.PhotoPath ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@PhotoName", SqlDbType.NVarChar, 100) { Value = (object)alumni.PhotoName ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });
                        command.Parameters.Add(new SqlParameter("@AlumniHobbies", SqlDbType.Structured) { TypeName = "dbo.AlumniHobbiesType", Value = alumniHobbies });

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
