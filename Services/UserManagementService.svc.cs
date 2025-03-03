using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserManagementService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserManagementService.svc or UserManagementService.svc.cs at the Solution Explorer and start debugging.
    public class UserManagementService : IUserManagementService
    {
        private AlumniManagementDataContext _dataContext;
        public string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ConnectionString;

        public UserManagementService()
        {
            _dataContext = new AlumniManagementDataContext(connStr);
        }

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspNetUserDTO.UserDTO> GetAllUsers()
        {
            var users = _dataContext.AspNetUsers.Select(u => new AspNetUserDTO.UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PasswordHash = u.PasswordHash,
                SecurityStamp = u.SecurityStamp,
                PhoneNumber = u.PhoneNumber,
                TwoFactorEnabled = u.TwoFactorEnabled,
                LockoutEndDateUtc = u.LockoutEndDateUtc,
                LockoutEnabled = u.LockoutEnabled,
                AccessFailedCount = u.AccessFailedCount,
                UserName = u.UserName,
                UserRoles = u.AspNetUserRoles.Select(ur => new AspNetUserDTO.UserRoleDTO
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    Role = new AspNetUserDTO.RoleDTO
                    {
                        Id = ur.AspNetRole.Id,
                        Name = ur.AspNetRole.Name,
                        RolePermissions = ur.AspNetRole.AspNetRolePermissions.Select(rp => new AspNetUserDTO.RolePermissionDTO
                        {
                            RoleId = rp.RoleId,
                            PermissionId = rp.PermissionId,
                            Role = new AspNetUserDTO.RoleDTO
                            {
                                Id = rp.AspNetRole.Id,
                                Name = rp.AspNetRole.Name
                            },
                            Permission = new AspNetUserDTO.PermissionDTO
                            {
                                Id = rp.AspNetPermission.Id,
                                Name = rp.AspNetPermission.Name
                            }
                        }).ToList()
                    }
                }).ToList(),
                UserClaims = u.AspNetUserClaims.Select(uc => new AspNetUserDTO.UserClaimDTO
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClaimType = uc.ClaimType,
                    ClaimValue = uc.ClaimValue
                }).ToList(),
                UserLogins = u.AspNetUserLogins.Select(ul => new AspNetUserDTO.UserLoginDTO
                {
                    LoginProvider = ul.LoginProvider,
                    ProviderKey = ul.ProviderKey,
                    UserId = ul.UserId
                }).ToList()
            }).ToList();

            return users;
        }

        public AspNetUserDTO.UserDTO GetUser(string id)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Id == id);

            return user;
        }

        public AspNetUserDTO.UserDTO GetUserByEmail(string email)
        {
            var user = GetAllUsers().FirstOrDefault(u => u.Email == email);

            return user;
        }

        public IEnumerable<AspNetUserDTO.UserDTO> GetUsersByRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsSuperAdminExists()
        {
            t var user = _dataContext.AspNetUsers.Where(u => u.AspNetUserRoles.Any(ur => ur.AspNetRole.Name == "Superadmin")).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RegisterUser(string fullName, string email, string password, bool superAdmin)
        {
            var user = new AspNetUser
            {
                FullName = fullName,
                Email = email,
                EmailConfirmed = false,
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = "",
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = email
            };
        }

        public void UpdatePassword(string id, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(AspNetUserDTO.UserDTO user)
        {
            var existingUser = _dataContext.AspNetUsers.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                if (existingUser != null)
                {
                    try
                    {
                        // Update the properties
                        existingUser.Email = user.Email;
                        existingUser.EmailConfirmed = user.EmailConfirmed;
                        existingUser.PasswordHash = user.PasswordHash;
                        existingUser.SecurityStamp = user.SecurityStamp;
                        existingUser.PhoneNumber = user.PhoneNumber;
                        existingUser.TwoFactorEnabled = user.TwoFactorEnabled;
                        existingUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
                        existingUser.LockoutEnabled = user.LockoutEnabled;
                        existingUser.AccessFailedCount = user.AccessFailedCount;
                        existingUser.UserName = user.UserName;
                        existingUser.FullName = user.FullName;

                        // Submit changes
                        _dataContext.SubmitChanges();
                    }
                    catch (ChangeConflictException)
                    {
                        // Resolve the conflict by refreshing the entity state
                        foreach (ObjectChangeConflict conflict in _dataContext.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepCurrentValues);
                        }

                        // Retry submitting changes
                        _dataContext.SubmitChanges();
                    }
                }
            }
        }

        public void UpdateUserFullName(string id, string fullName)
        {
            using (var transaction = new TransactionScope())
            {
                var existingUser = _dataContext.AspNetUsers.SingleOrDefault(u => u.Id == id);
                if (existingUser != null)
                {
                    // Refresh before making changes
                    _dataContext.Refresh(RefreshMode.OverwriteCurrentValues, existingUser);

                    existingUser.FullName = fullName;
                    _dataContext.SubmitChanges();
                }
                transaction.Complete();
            }
        }

        public void AssignSuperadmin(string id)
        {
            var existingUser = _dataContext.AspNetUsers.Where(u => u.Id == id).First();
            string roleId = _dataContext.AspNetRoles.Where(r => r.Name == "Superadmin").FirstOrDefault().ToString();

            if (existingUser != null && roleId != null)
            {
                var userRole = new AspNetUserRole
                {
                    UserId = id,
                    RoleId = roleId
                };

                _dataContext.AspNetUserRoles.InsertOnSubmit(userRole);
                _dataContext.SubmitChanges();
            }
        }
    }
}
