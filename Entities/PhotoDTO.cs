using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class PhotoDTO
    {
        public int PhotoID{get; set;}

        public int AlbumID{get; set;}

        public string PhotoPath{get; set;}

        public string PhotoFilleName{get; set;}

        public System.Nullable<bool> IsPhotoAlbumThumbnail{get; set;}

        public System.DateTime ModifiedDate{get; set;}
    }
}