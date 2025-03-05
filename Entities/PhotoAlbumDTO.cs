using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class PhotoAlbumDTO
    {
        public int AlbumID  {get; set;}

        public string AlbumName {get; set;}

        public System.DateTime ModifiedDate {get; set;}

        public string ThumbnailPhoto { get; set;}



    }
}