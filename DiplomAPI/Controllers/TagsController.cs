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
    public class TagsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TagsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT tagId, tag FROM Tags";

            DataTable Tags = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader TagsReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand tgCommand = new SqlCommand(query, newCon))
                {
                    TagsReader = tgCommand.ExecuteReader();
                    Tags.Load(TagsReader);
                    TagsReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(Tags);
        }
    }
}
