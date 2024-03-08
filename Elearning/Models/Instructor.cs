using System.Reflection.Metadata;
using Elearning.Utilities;
using Humanizer;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Instructor
    {
        public int Instructor_Id { get; set; }
        public string Name { get; set; }
        public int Course_Id { get; set; }
        public int Is_Deleted { get; set; }

        public string Course_Name { get; set; }

        public List<Instructor> Instructors  = new List<Instructor>();

        public string connString = ProjectConstants.connString;

        public void GetInstructors()
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT i.INSTRUCTOR_ID, i.NAME, i.COURSE_ID, c.TITLE FROM INSTRUCTOR i JOIN COURSE c ON i.COURSE_ID = c.COURSE_ID";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    { 
                        Instructor instructor = new Instructor();
                        instructor.Instructor_Id = reader.GetInt32(0);
                        instructor.Name = reader.GetString(1);
                        instructor.Course_Id = reader.GetInt32(2);
                        instructor.Course_Name = reader.GetString(3);
                        Instructors.Add(instructor);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            } 
            catch (Exception exception) 
            { 
                Console.WriteLine(exception.ToString());
            }
        }

        //public void GetInstructorCourses()
        //{ 
        //    try 
        //    {
        //        using (OracleConnection conn = new OracleConnection(connString))
        //        {
        //            string query = $"SELECT";
        //        }
        //    } 
        //    catch (Exception exception) 
        //    { 
        //    } 
        //}

        public void GetInstructorById(int instructorId)
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT * FROM INSTRUCTOR WHERE INSTRUCTOR_ID = {instructorId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor();
                        instructor.Instructor_Id = reader.GetInt32(0);
                        instructor.Name = reader.GetString(1);
                        instructor.Course_Id = reader.GetInt32(2);
                        Instructors.Add(instructor);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception) 
            { 
                Console.WriteLine(exception.ToString());
            }
        }

        public void AddInstructor(Instructor instructor)
        {
            try 
            {
                using (OracleConnection conn = new OracleConnection(connString)) 
                {
                    string query = $"INSERT INTO INSTRUCTOR (NAME, COURSE_ID) VALUES('{instructor.Name}',{instructor.Course_Id})";
                    OracleCommand cmd = new OracleCommand( query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                } 
            } 
            catch (Exception exception) 
            { 
                Console.WriteLine (exception.ToString());
            }
        }

        public void UpdateInstructor(Instructor instructor)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"UPDATE INSTRUCTOR SET NAME = '{instructor.Name}', " +
                                   $"COURSE_ID = '{instructor.Course_Id}' " +
                                   $"WHERE INSTRUCTOR_ID = {instructor.Instructor_Id}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
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
            }
        }
    }
}
