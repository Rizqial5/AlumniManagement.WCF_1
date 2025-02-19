using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlumniImageService" in both code and config file together.
    [ServiceContract]
    public interface IAlumniImageService
    {
        [OperationContract]
        IEnumerable<ImageDTO> GetAllImage(int alumniId);

        [OperationContract]
        ImageDTO GetImageById(int imageId,int alumniId);

        [OperationContract]
        Task DeleteImageByIdAsync(int imageId,int alumniId);

        [OperationContract]
        Task AddImageAsync(IEnumerable<ImageDTO> imageDTO, int alumniId);
    }
}
