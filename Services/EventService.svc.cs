using AlumniManagement.WCF.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AlumniManagement.WCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EventService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EventService.svc or EventService.svc.cs at the Solution Explorer and start debugging.
    public class EventService : IEventService
    {
        private AlumniManagementDataContext _context;
        private string connectionString = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public EventService()
        {
            _context = new AlumniManagementDataContext(ConnectionString);
        }

        public IEnumerable<EventDTO> GetAllEvents()
        {
            var data = _context.Events.ToList();

            var result = data.Select(x => new EventDTO
            {
                EventID = x.EventID,
                Title = x.Title,
                Description = x.Description,
                EventImageName = x.EventImageName,
                EventImagePath = x.EventImagePath,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsClosed = x.IsClosed,
                ModifiedDate = x.ModifiedDate,
                DateDisplay = DateDisplayFormat(x.StartDate, x.EndDate),
                Status = x.IsClosed ? "Closed" : "Open" 

            });



            return result.OrderByDescending(f => f.ModifiedDate);
        }

        private string DateDisplayFormat(DateTime date, DateTime endDate)
        {
            DateTime checkDate = new DateTime(1753, 1, 1);

            if (endDate == checkDate)
            {
                return date.ToString("dd-MMM-yyyy");
            }

            return date.ToString("dd-MMM-yyyy") + " - " + endDate.ToString("dd-MMM-yyyy");
        }

        public EventDTO GetEventById(int eventId)
        {
            var selectedData = GetAllEvents().FirstOrDefault(m => m.EventID == eventId);

            return selectedData;
        }



        public void UpsertEvent(EventDTO eventDto)
        {
            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.UpsertEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int) { Value = (object)eventDto.EventID ?? 0 });
                    command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 255) { Value = eventDto.Title });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1000) { Value = eventDto.Description });
                    command.Parameters.Add(new SqlParameter("@EventImagePath", SqlDbType.NVarChar, 255) { Value = eventDto.EventImagePath });
                    command.Parameters.Add(new SqlParameter("@EventImageName", SqlDbType.NVarChar, 100) { Value = eventDto.EventImageName });
                    command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = eventDto.StartDate });
                    command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = eventDto.EndDate });
                    command.Parameters.Add(new SqlParameter("@IsClosed", SqlDbType.Bit) { Value = eventDto.IsClosed });
                    command.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = DateTime.Now });
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteEvent(int eventId)
        {
            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                using (var command = new SqlCommand("dbo.DeleteEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int) { Value = eventId });

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
