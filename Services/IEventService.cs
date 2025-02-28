using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEventService" in both code and config file together.
    [ServiceContract]
    public interface IEventService
    {
        [OperationContract]
        IEnumerable<EventDTO> GetAllEvents();

        [OperationContract]
        EventDTO GetEventById(int eventId);

        [OperationContract]
        void UpsertEvent(EventDTO eventDTO);    

        [OperationContract]
        void DeleteEvent(int eventId);
    }
}
