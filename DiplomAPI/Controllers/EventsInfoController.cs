using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DiplomAPI.Models;

namespace DiplomAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class EventsInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT eventName, eventDescription, eventPicture, CONVERT(varchar(10),dateOfStart,120) AS dateOfStart, 
                CONVERT(varchar(10),dateOfEnd,120) AS dateOfEnd, address FROM dbo.Events WHERE eventId=@eventId";

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

        [HttpPost]
        public JsonResult Post(Events ev)
        {
            string query = @"insert into dbo.Events
                values (@eventName, @eventDescription, '', @dateOfStart, @dateOfEnd, @address, @tagId, 'Неактивно')";

            DataTable EventsAdd = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader EventsAddReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evAddCommand = new SqlCommand(query, newCon))
                {
                    evAddCommand.Parameters.AddWithValue("@eventName", ev.eventName);
                    evAddCommand.Parameters.AddWithValue("@eventDescription", ev.eventDescription);
                    evAddCommand.Parameters.AddWithValue("@dateOfStart", ev.dateOfStart);
                    evAddCommand.Parameters.AddWithValue("@dateOfEnd", ev.dateOfEnd);
                    evAddCommand.Parameters.AddWithValue("@adress", ev.address);
                    evAddCommand.Parameters.AddWithValue("@tagId", ev.tagId);
                    EventsAddReader = evAddCommand.ExecuteReader();
                    EventsAdd.Load(EventsAddReader);
                    EventsAddReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(EventsAdd);
        }
    }
}
