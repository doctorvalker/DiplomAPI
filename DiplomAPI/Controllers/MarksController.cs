﻿using Microsoft.AspNetCore.Http;
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
        public JsonResult Get(int id)
        {
            string query = @"SELECT eventId, ROUND(AVG(mark), 1) AS mark FROM dbo.Marks
                WHERE eventId = @eventId
                GROUP BY eventId";

            DataTable Marks = new DataTable();
            string sqlDS = _configuration.GetConnectionString("EventsApp");
            SqlDataReader MarksReader;
            using (SqlConnection newCon = new SqlConnection(sqlDS))
            {
                newCon.Open();
                using (SqlCommand mrCommand = new SqlCommand(query, newCon))
                {
                    mrCommand.Parameters.AddWithValue("@eventId", id);
                    MarksReader = mrCommand.ExecuteReader();
                    Marks.Load(MarksReader);
                    MarksReader.Close();
                    newCon.Close();
                }
            }

            return new JsonResult(Marks);
        }
    }
}
