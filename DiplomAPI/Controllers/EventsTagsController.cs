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
    public class EventsTagsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsTagsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT eventId, eventName, eventDescription, eventPicture FROM dbo.Events WHERE tagId=@tagId AND (status = 'Активно')";

            DataTable EventsTags = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader EventsTagsReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evTgCommand = new SqlCommand(query, newCon))
                {
                    evTgCommand.Parameters.AddWithValue("@tagId", id);
                    EventsTagsReader = evTgCommand.ExecuteReader();
                    EventsTags.Load(EventsTagsReader);
                    EventsTagsReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(EventsTags);
        }
    }
}
