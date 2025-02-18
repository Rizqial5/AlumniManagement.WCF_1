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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MajorService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MajorService.svc or MajorService.svc.cs at the Solution Explorer and start debugging.
    public class MajorService : IMajorService
    {

        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }


        public MajorService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<MajorDTO> GetAll()
        {
            var data = _context.Majors.ToList();

            var result = from m in _context.Majors
                         join f in _context.Faculties on m.FacultyID equals f.FacultyID
                         select new MajorDTO
                         {
                             MajorID = m.MajorID,
                             MajorName = m.MajorName,
                             Description = m.Description,
                             FacultyID = m.FacultyID,
                             FacultyName = f.FacultyName,
                             ModifiedDate = m.ModifiedDate,
                         };

            var filterData = result.OrderByDescending(m => m.ModifiedDate).ToList();


            return filterData;
        }

        public MajorDTO GetMajor(int majorId)
        {
            var selectedData = _context.Majors.FirstOrDefault(m => m.MajorID == majorId);

            return Mapping.Mapper.Map<MajorDTO>(selectedData);
        }

        public MajorDTO GetMajorIdByName(string name)
        {
            var selectedData = _context.Majors.FirstOrDefault(m => m.MajorName == name);

            return Mapping.Mapper.Map<MajorDTO>(selectedData);
        }

        public void InsertMajor(MajorDTO major)
        {
            var inserData = Mapping.Mapper.Map<Major>(major);

            inserData.ModifiedDate = DateTime.Now;

            _context.Majors.InsertOnSubmit(inserData);

            _context.SubmitChanges();
        }

        public void UpdateMajor(MajorDTO major)
        {
            var selectedData = _context.Majors.FirstOrDefault(m => m.MajorID == major.MajorID);

            var updatedData = Mapping.Mapper.Map(major, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public void DeleteMajor(int majorId)
        {
            var selectedData = _context.Majors.FirstOrDefault(m => m.MajorID == majorId);

            _context.Majors.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();
        }

        public IEnumerable<MajorDTO> GetMajorIdByFacultyId(int facultyId)
        {
            var data = from m in _context.Majors
                       join f in _context.Faculties on m.FacultyID equals f.FacultyID
                       where m.FacultyID == facultyId
                       select m;

            var result = data.Select(m=> Mapping.Mapper.Map<MajorDTO>(m));

            return result.ToList();
        }
    }
}
