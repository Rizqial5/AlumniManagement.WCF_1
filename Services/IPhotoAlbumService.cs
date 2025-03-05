using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPhotoAlbumService" in both code and config file together.
    [ServiceContract]
    public interface IPhotoAlbumService
    {
        [OperationContract]
        IEnumerable<PhotoAlbumDTO> GetPhotoAlbums();

        [OperationContract]
        IEnumerable<PhotoDTO> GetAllPhotoByAlbumId(int AlbumId);

        [OperationContract]
        PhotoAlbumDTO GetPhotoAlbumById(int AlbumId);

        [OperationContract]
        PhotoDTO GetPhotoByAlbumIdAndPhotoId(int AlbumId, int photoId);

        [OperationContract]
        void InsertPhotoAlbum(PhotoAlbumDTO photoAlbum);

        [OperationContract]
        void InsertPhoto(List<PhotoDTO> photo, int albumID);

        [OperationContract]
        void UpdatePhotoAlbum(PhotoAlbumDTO photoAlbum);

        [OperationContract]
        void DeletePhotoAlbum(int albumID);

        [OperationContract]
        void DeletePhoto(int albumID, int photoID);

        [OperationContract]
        void SetThumbnail(int photoId, int albumId);


    }
}
