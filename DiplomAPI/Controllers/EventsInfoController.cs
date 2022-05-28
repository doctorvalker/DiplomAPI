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
    public class EventsInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            string query = @"SELECT eventName, eventDescription, eventPicture, dateOfStart, dateOfEnd, address FROM dbo.Events WHERE eventId=@eventId";

            DataTable EventsInfo = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader EventsInfoReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evInCommand = new SqlCommand(query, newCon))
                {
                    evInCommand.Parameters.AddWithValue("@eventId", id);
                    EventsInfoReader = evInCommand.ExecuteReader();
                    EventsInfo.Load(EventsInfoReader);
                    EventsInfoReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(EventsInfo);
        }
    }
}
