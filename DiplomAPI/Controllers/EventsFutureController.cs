using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DiplomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsFutureController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsFutureController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT eventId, eventName, eventPicture
                FROM dbo.Events WHERE (GETDATE() < dateOfStart) AND (GETDATE() < dateOfEnd) AND (status = 'Активен')";

            DataTable Events = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader EventsReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evCommand = new SqlCommand(query, newCon))
                {
                    EventsReader = evCommand.ExecuteReader();
                    Events.Load(EventsReader);
                    EventsReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(Events);
        }
    }
}
