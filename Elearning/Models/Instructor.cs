using System.Reflection.Metadata;
using Elearning.Utilities;
using Humanizer;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Instructor
    {
        public int? InstructorId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact {  get; set; }
        public int? CourseId { get; set; }
        public int? IsDeleted { get; set; }

        public string? Course_Name { get; set; }

        public List<Instructor> Instructors  = new List<Instructor>();

        public string connString = ProjectConstants.connString;

        public void GetInstructors()
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT INSTRUCTOR_ID, NAME, EMAIL, CONTACT, IS_DELETED FROM INSTRUCTOR";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        //Instructor instructor = new Instructor();
                        //instructor.Instructor_Id = reader.GetInt32(0);
                        //instructor.Name = reader.GetString(1);
                        //instructor.Course_Id = reader.GetInt32(2);
                        //instructor.Is_Deleted = reader.GetInt32(3);
                        //instructor.Course_Name = reader.GetString(4);
                        //Instructors.Add(instructor);
                        Instructor instructor = new Instructor();
                        instructor.InstructorId = reader.GetInt32(0);
                        instructor.Name = reader.GetString(1);
                        instructor.Email = reader.GetString(2);
                        instructor.Contact = reader.GetString(3);
                        instructor.IsDeleted = reader.GetInt32(4);
                        Instructors.Add(instructor);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            } 
            catch (Exception exception) 
            { 
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public Instructor GetInstructorById(int instructorId)
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT INSTRUCTOR_ID, NAME, EMAIL, CONTACT, IS_DELETED FROM INSTRUCTOR WHERE INSTRUCTOR_ID = {instructorId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    Instructor instructor = new Instructor();
                    while (reader.Read())
                    {
                        instructor.InstructorId = reader.GetInt32(0);
                        instructor.Name = reader.GetString(1);
                        instructor.Email = reader.GetString(2);
                        instructor.Contact = reader.GetString(3);
                        instructor.IsDeleted = reader.GetInt32(4);
                    }
                    reader.Dispose();
                    conn.Close();
                    return instructor;
                }
            }
            catch (Exception exception) 
            { 
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public void AddInstructor(Instructor instructor)
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString)) 
                {
                    string query = $"INSERT INTO INSTRUCTOR (NAME, EMAIL, CONTACT) VALUES('{instructor.Name}','{instructor.Email}','{instructor.Contact}')";
                    OracleCommand cmd = new OracleCommand( query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                } 
            } 
            catch (Exception exception) 
            { 
                Console.WriteLine (exception.ToString());
                throw;
            }
        }

        public void UpdateInstructor(Instructor instructor)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {

                    Console.WriteLine(instructor.Name);
                    Console.WriteLine(instructor.Email);
                    Console.WriteLine(instructor.Contact);
                    Console.WriteLine(instructor.InstructorId);
                    string query = $"UPDATE INSTRUCTOR SET NAME = '{instructor.Name}', " +
                                   $"EMAIL = '{instructor.Email}', " +
                                   $"CONTACT = '{instructor.Contact}' " +
                                   $"WHERE INSTRUCTOR_ID = {instructor.InstructorId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public void DeleteInstructor(int id)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"UPDATE INSTRUCTOR SET IS_DELETED = 1 WHERE INSTRUCTOR_ID = {id}";
                    OracleCommand cmd = new OracleCommand ( query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            { 
                Console.WriteLine(exception.ToString());
                throw;
            }
        }
    }
}
