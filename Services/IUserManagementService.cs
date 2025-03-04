using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserManagementService" in both code and config file together.
    [ServiceContract]
    public interface IUserManagementService
    {
        [OperationContract]
        bool IsSuperAdminExists();

        [OperationContract]
        void RegisterUser(string fullName, string email, string password, bool superAdmin);

        [OperationContract]
        void UpdatePassword(string id, string password);

        [OperationContract]
        void UpdateUser(AspNetUserDTO.UserDTO user);
        [OperationContract]
        void AssignSuperadmin(string id);

        [OperationContract]
        void DeleteUser(string id);

        [OperationContract]
        AspNetUserDTO.UserDTO GetUser(string id);

        [OperationContract]
        IEnumerable<AspNetUserDTO.UserDTO> GetAllUsers();

        [OperationContract]
        IEnumerable<AspNetUserDTO.UserDTO> GetUsersByRole(string role);

        [OperationContract]
        AspNetUserDTO.UserDTO GetUserByEmail(string email);

        [OperationContract]
        void UpdateUserFullName(string id, string fullName);

        [OperationContract]
        void UpdateUserRoles(string id, string newRoles);

        [OperationContract]
        IEnumerable<AspNetUserDTO.RoleDTO> GetAllRoles();

        [OperationContract]
        void InsertRoles(AspNetUserDTO.RoleDTO roleDTO);

        [OperationContract]
        void DeleteRoles(string id);

        [OperationContract]
        AspNetUserDTO.RoleDTO GetRoleById(string id);




    }
}
