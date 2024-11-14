using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace AiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private SqlConnection _connection;

        public UserController()
        {
            _connection = new SqlConnection("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                _connection.Open();
                var command = new SqlCommand("SELECT * FROM Users", _connection);
                var reader = command.ExecuteReader();
                
                string allUsers = "";
                while (reader.Read())
                {
                    allUsers += reader["Id"].ToString() + " - " + reader["Name"].ToString() + "\n";
                }

                reader.Close();
                _connection.Close();
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                _connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Id = " + id, _connection);
                var reader = command.ExecuteReader();

                string userDetails = "";
                if (reader.Read())
                {
                    userDetails = reader["Id"].ToString() + " - " + reader["Name"].ToString();
                }

                reader.Close();
                _connection.Close();
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong: " + ex.Message);
            }
        }
    }
}
