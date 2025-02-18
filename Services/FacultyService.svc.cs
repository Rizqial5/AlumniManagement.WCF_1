using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FacultyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FacultyService.svc or FacultyService.svc.cs at the Solution Explorer and start debugging.
    public class FacultyService : IFacultyService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public FacultyService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<FacultyDTO> GetAll()
        {
            var data = _context.Faculties.ToList();

            var result = data.Select(x => Mapping.Mapper.Map<FacultyDTO>(x));



            return result.OrderByDescending(f=>f.ModifiedDate);
        }

        public FacultyDTO GetFaculty(int FacultyId)
        {
            var selectedData = _context.Faculties.FirstOrDefault(m => m.FacultyID == FacultyId);

            return Mapping.Mapper.Map<FacultyDTO>(selectedData);
        }

        public FacultyDTO GetFacultyIdByName(string name)
        {
            var selectedData = _context.Faculties.FirstOrDefault(m => m.FacultyName == name);

            return Mapping.Mapper.Map<FacultyDTO>(selectedData);
        }

        public void InsertFaculty(FacultyDTO Faculty)
        {

            var inserData = Mapping.Mapper.Map<Faculty>(Faculty);

            inserData.ModifiedDate = DateTime.Now;

            _context.Faculties.InsertOnSubmit(inserData);

            _context.SubmitChanges();
        }

        public void UpdateFaculty(FacultyDTO Faculty)
        {
            var selectedData = _context.Faculties.FirstOrDefault(m => m.FacultyID == Faculty.FacultyID);

            var updatedData = Mapping.Mapper.Map(Faculty, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public void DeleteFaculty(int FacultyId)
        {
            
            var selectedData = _context.Faculties.FirstOrDefault(m => m.FacultyID == FacultyId);

            _context.Faculties.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();
        }


    }
}
