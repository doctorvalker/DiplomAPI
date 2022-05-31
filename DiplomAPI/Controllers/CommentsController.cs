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
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CommentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT dbo.Comments.comment, dbo.Users.userName
                FROM dbo.Comments INNER JOIN dbo.Users ON dbo.Comments.userId = dbo.Users.userId
                WHERE (dbo.Comments.eventId = @eventId)";

            DataTable Comments = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader CommentsReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand cmCommand = new SqlCommand(query, newCon))
                {
                    cmCommand.Parameters.AddWithValue("@eventId", id);
                    CommentsReader = cmCommand.ExecuteReader();
                    Comments.Load(CommentsReader);
                    CommentsReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(Comments);
        }

        [HttpPost]
        public JsonResult Post(Comments cmmnts)
        {
            string query = @"insert into dbo.Comments
                values (@eventId, @userId, @comment)";

            DataTable CommentsAdd = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader CommentsAddReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand cmmntAddCommand = new SqlCommand(query, newCon))
                {
                    cmmntAddCommand.Parameters.AddWithValue("@eventId", cmmnts.eventId);
                    cmmntAddCommand.Parameters.AddWithValue("@userId", cmmnts.userId);
                    cmmntAddCommand.Parameters.AddWithValue("@mark", cmmnts.comment);
                    CommentsAddReader = cmmntAddCommand.ExecuteReader();
                    CommentsAdd.Load(CommentsAddReader);
                    CommentsAddReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(MarksAdd);
        }
    }
}
