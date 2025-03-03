using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AlumniManagement.WCF.Entities
{
    public class AspNetUserDTO
    {
        public class RoleDTO
        {
            public string Id{get; set;}

            public string Name{get; set;}

            public ICollection<RolePermissionDTO> RolePermissions { get; set; }
        }

        public class PermissionDTO
        {
            public int Id{get; set;}

            public string Name{get; set;}

            public ICollection<RolePermissionDTO> RolePermissions { get; set; }
        }

        public class RolePermissionDTO
        {
            public string RoleId{get; set;}

            public int PermissionId{get; set;}

            public PermissionDTO Permission{get; set;}

            public RoleDTO Role{get; set;}
        }

        public class UserDTO
        {
            public string Id{get; set;}

            public string FullName{get; set;}

            public string Email{get; set;}

            public bool EmailConfirmed{get; set;}

            public string PasswordHash{get; set;}

            public string SecurityStamp{get; set;}

            public string PhoneNumber{get; set;}

            public bool PhoneNumberConfirmed{get; set;}

            public bool TwoFactorEnabled{get; set;}

            public System.Nullable<System.DateTime> LockoutEndDateUtc{get; set;}

            public bool LockoutEnabled{get; set;}

            public int AccessFailedCount{get; set;}

            public string UserName{get; set;}

            public ICollection<UserClaimDTO> UserClaims{get; set;}

            public ICollection<UserLoginDTO> UserLogins{get; set;}

            public ICollection<UserRoleDTO> UserRoles{get; set;}
        }

        public class UserRoleDTO
        {
            public string UserId{get; set;}

            public string RoleId{get; set;}

            public RoleDTO Role{get; set;}

            public UserDTO User{get; set;}
        }

        public class UserClaimDTO
        {
            public int Id{get; set;}

            public string UserId{get; set;}

            public string ClaimType{get; set;}

            public string ClaimValue{get; set;}

            public UserDTO User{get; set;}
        }

        public class UserLoginDTO
        {
            public string LoginProvider{get; set;}

            public string ProviderKey{get; set;}

            public string UserId{get; set;}

            public UserDTO User{get; set;}
        }
    }
}