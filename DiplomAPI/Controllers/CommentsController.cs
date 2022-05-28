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
    public class CommentsController : ControllerBase
    {
        string query = @"SELECT dbo.Users.userName, dbo.Comments.comment 
            FROM dbo.Comments INNER JOIN dbo.Users ON dbo.Comments.userId = dbo.Users.userId
            WHERE (dbo.Comments.userId = 1)";
        private readonly IConfiguration _configuration;

        public CommentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            string query = @"SELECT eventId, ROUND(AVG(mark), 1) AS mark FROM dbo.Marks
                WHERE eventId = @eventId
                GROUP BY eventId";

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
    }
}
