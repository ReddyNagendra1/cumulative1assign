using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cumulative1assign.Models;

namespace cumulative1assign.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        ///This Controller will access the teachers table of our School database.
        ///<summary>
        ///Returns a list of Teachers in the system.
        /// </summary>
        /// <param id="teacher">some kind of text to seacrh against the teacher name</param>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of teachers (first names and last names) 
        /// </returns>
        /// 
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            //cmd.CommandText = "Select * from Teachers";
            cmd.CommandText = "Select * from Teachers where teacherfname like '%" + SearchKey + "%' or teacherlname like '%" + SearchKey + "%' ";
            // Select* from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher Names
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop through each row the Result Set
            while (ResultSet.Read())
            {
                //Access column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                //string Salary = ResultSet["salary"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                //NewTeacher.Salary = Salary;

                //Add the Teacher Name to the list
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MYSQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers names
            return Teachers;

        }

        /// <summary>
        /// Finds a teacher from the database through an id. 
        /// </summary>
        /// <param name="teacherid">Teacher Id</param>
        /// <returns></returns>
        /// /// <example>GET api/TeacherData/FindTeacher/4 -> {Teacher Object}</example>
        /// 
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherid}")]

        public Teacher FindTeacher(int teacherid)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //Create SQL QUERY
            string query = "Select * from Teachers where teacherid =@id ";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", teacherid);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                //string Salary = ResultSet["salary"].ToString();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                //NewTeacher.Salary = Salary;
            }
            Conn.Close();

            return NewTeacher;
        }



        /// <summary>
        /// Deletes a profile of teacher into the system
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/5</example>
        /// <returns>
        /// </returns>
        /// 
        [HttpPost]

        public void DeleteTeacher(int teacherid)

        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Delete from Teachers where teacherid=@id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", teacherid);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Adds a Teacher to the database. 
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the Teachers table.</param>
        /// <example>
        /// POST api/Teacher DATA /REQUEST BODY
        /// {
        /// "TeacherFname": "Reddy",
        /// "TeacherLname": "Nagendra",
        /// "EmployeeNumber": "98RTY2"
        /// }
        /// </example>

        [HttpPost]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary  ) " +
                "values (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @Salary)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }




    }
}
