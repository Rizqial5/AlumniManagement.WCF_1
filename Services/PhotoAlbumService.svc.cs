using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PhotoAlbumService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PhotoAlbumService.svc or PhotoAlbumService.svc.cs at the Solution Explorer and start debugging.
    public class PhotoAlbumService : IPhotoAlbumService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public PhotoAlbumService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<PhotoDTO> GetAllPhotoByAlbumId(int AlbumId)
        {
            var data = _context.Photos.Where(p => p.AlbumID == AlbumId).ToList();

            var result = Mapping.Mapper.Map<List<PhotoDTO>>(data);

            return result.OrderBy(p => p.ModifiedDate);
        }


        public IEnumerable<PhotoAlbumDTO> GetPhotoAlbums()
        {
            var photoAlbums = new List<PhotoAlbumDTO>();

          

            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                using (var command = new SqlCommand("sp_GetPhotoAlbums", connection)) // Nama Stored Procedure
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            photoAlbums.Add(new PhotoAlbumDTO
                            {
                                AlbumID = reader.GetInt32(reader.GetOrdinal("AlbumID")),
                                AlbumName = reader.GetString(reader.GetOrdinal("AlbumName")),
                                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                            });
                        }
                    }
                }
            }

            return photoAlbums.ToList();
        }

        public PhotoAlbumDTO GetPhotoAlbumById(int AlbumId)
        {
            var data = _context.PhotoAlbums.FirstOrDefault(a=> a.AlbumID == AlbumId);

            var result = Mapping.Mapper.Map<PhotoAlbumDTO>(data);

            return result;
        }

        public PhotoDTO GetPhotoByAlbumIdAndPhotoId(int AlbumId, int photoId)
        {
            var data = GetAllPhotoByAlbumId(AlbumId)
                .FirstOrDefault(p => p.PhotoID == photoId);

            return data;

             
        }

        public void InsertPhoto(PhotoDTO photo, int albumID)
        {
            var data = Mapping.Mapper.Map<Photo>(photo);

            data.AlbumID = albumID;

            _context.Photos.InsertOnSubmit(data);

            _context.SubmitChanges();
        }

        public void InsertPhotoAlbum(PhotoAlbumDTO photoAlbum)
        {
            var data = Mapping.Mapper.Map<PhotoAlbum>(photoAlbum);

            _context.PhotoAlbums.InsertOnSubmit(data);
            _context.SubmitChanges();
        }

        public void UpdatePhotoAlbum(PhotoAlbumDTO photoAlbum)
        {
            var selectedData = _context.PhotoAlbums.FirstOrDefault(m => m.AlbumID == photoAlbum.AlbumID);

            var updatedData = Mapping.Mapper.Map(photoAlbum, selectedData);

            updatedData.ModifiedDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public void DeletePhoto(int albumID, int photoID)
        {
            var selectedData = _context.Photos.Where(p=>p.AlbumID == albumID)
                .FirstOrDefault(a=> a.PhotoID == photoID);

       
            _context.Photos.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();
        }

        public void DeletePhotoAlbum(int albumID)
        {
            var selectedData = _context.PhotoAlbums.FirstOrDefault(m => m.AlbumID == albumID);

            _context.PhotoAlbums.DeleteOnSubmit(selectedData);

            _context.SubmitChanges();
        }
    }
}
