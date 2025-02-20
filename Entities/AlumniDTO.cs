using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.WCF.Entities
{
    public class AlumniDTO
    {
        public int AlumniID {get;  set;}

        public string FirstName {get;  set;}

        public string MiddleName {get;  set;}

        public string LastName {get;  set;}

        public string Email {get;  set;}

        public string MobileNumber {get;  set;}

        public string Address {get;  set;}

        public System.Nullable<int> DistrictID {get;  set;}

        public System.Nullable<System.DateTime> DateOfBirth {get;  set;}

        public System.Nullable<int> GraduationYear {get;  set;}

        public string Degree {get;  set;}

        public System.Nullable<int> MajorID {get;  set;}

        public string LinkedInProfile {get;  set;}

        public System.DateTime ModifiedDate {get;  set;}

        // tambahan
        [Ignore]
        public string FullAddress { get; set; }

        [Ignore]
        public string FullName { get; set; }

        [Ignore]
        public string FacultyName { get; set; }
        [Ignore]
        public string StateName { get; set; }
        [Ignore]
        public string DistrictName { get; set; }
        [Ignore]
        public string MajorName { get; set; }
        [Ignore]
        public int FacultyID { get; set; }
        [Ignore]
        public int StateID { get; set; }
        [Ignore]
        public string HobbiesListName { get; set; }

        [Ignore]
        public IEnumerable<HobbyDTO> Hobbies { get; set;}

        [Ignore]
        public IEnumerable<SelectListItem> DistrictDDL { get; set; }
        [Ignore]
        public IEnumerable<SelectListItem> MajorDDl { get; set; }
        [Ignore]
        public IEnumerable<SelectListItem> HobbiesDDl { get; set; }
    }
}