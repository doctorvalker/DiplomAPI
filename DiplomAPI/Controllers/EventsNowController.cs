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
    public class EventsNowController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsNowController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT eventId, eventName, eventDescription, eventPicture, 
                convert(varchar(10), dateOfStart, 120) AS dateOfStart, convert(varchar(10), dateOfEnd, 120) AS dateOfEnd, address, tag
                FROM dbo.Events WHERE (GETDATE() > dateOfStart) AND (GETDATE() < dateOfEnd)";

            DataTable Events = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader EventsReader;
            using(SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evCommand = new SqlCommand(query,newCon))
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
