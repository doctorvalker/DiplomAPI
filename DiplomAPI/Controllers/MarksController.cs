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
    public class MarksController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public MarksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT TOP (100) PERCENT eventId, ROUND(AVG(mark), 1) AS mark
                FROM dbo.Marks GROUP BY eventId ORDER BY eventId;";

            DataTable Marks = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader MarksReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand evCommand = new SqlCommand(query, newCon))
                {
                    MarksReader = evCommand.ExecuteReader();
                    Marks.Load(MarksReader);
                    MarksReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(Marks);
        }
    }
}
