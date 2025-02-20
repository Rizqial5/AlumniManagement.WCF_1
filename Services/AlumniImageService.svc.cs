using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlumniImageService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AlumniImageService.svc or AlumniImageService.svc.cs at the Solution Explorer and start debugging.
    public class AlumniImageService : IAlumniImageService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public AlumniImageService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<ImageDTO> GetAllImage(int alumniId)
        {
            var data = _context.AlumniImages.ToList().Where(a=> a.AlumniID == alumniId);

            var result = data.Select(x => Mapping.Mapper.Map<ImageDTO>(x));



            return result.OrderByDescending(f => f.UploadDate);
        }

        public ImageDTO GetImageById(int imageId, int alumniId)
        {
            var selectedData = (from i in _context.AlumniImages
                                where i.AlumniID == alumniId
                                select i).FirstOrDefault(i=> i.ImageID == imageId);

            return Mapping.Mapper.Map<ImageDTO>(selectedData);
        }
        public async Task AddImageAsync(IEnumerable<ImageDTO> imageDTO, int alumniId)
        {


            var inserData = imageDTO.Select(x => Mapping.Mapper.Map<AlumniImage>(x));

            foreach (var item in inserData)
            {
                item.AlumniID = alumniId;
                item.UploadDate = DateTime.Now;
            }

            _context.AlumniImages.InsertAllOnSubmit(inserData);

            await Task.Run(() =>
            {
                _context.SubmitChanges();
            });   

            
        }

        public async Task DeleteImageByIdAsync(int imageId, int alumniId)
        {
            var selectedData = _context.AlumniImages
                .Where(i => i.AlumniID == alumniId)
                .FirstOrDefault(i => i.ImageID == imageId);



            _context.AlumniImages.DeleteOnSubmit(selectedData);

            await Task.Run(() =>
            {
                _context.SubmitChanges();
            });

        }
    }
}
