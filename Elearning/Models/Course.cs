using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Course
    {
        public int Course_Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Is_Deleted { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }

        public List<Course> Courses = new List<Course>();

        public string connString = ProjectConstants.connString;

        public void GetCourses()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString)) 
                {
                    //string query = "SELECT c.COURSE_ID, c.TITLE, i.INSTRUCTOR_ID, i.NAME FROM COURSE c JOIN INSTRUCTOR i ON c.COURSE_ID = i.COURSE_ID";
                    string query = "SELECT COURSE_ID, TITLE, DESCRIPTION, IS_DELETED FROM COURSE";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Course course = new Course();
                        course.Course_Id = reader.GetInt32(0);
                        course.Title = reader.GetString(1);
                        course.Description = reader.GetString(2);
                        course.Is_Deleted = reader.GetInt32(3);
                        Courses.Add(course);
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

        public void GetCourseById(int courseId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT * FROM COURSE WHERE COURSE_ID = {courseId}";
                    OracleCommand cmd = new OracleCommand( query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        Course course = new Course();
                        course.Course_Id = reader.GetInt32(0);
                        course.Title = reader.GetString(1);
                        course.Description = reader.GetString(2);
                        course.Is_Deleted = reader.GetInt32(3);
                        Courses.Add(course);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception)
            { 
                Console.WriteLine (exception.ToString());
                throw;
            }
        }

        public void AddCourse(Course course)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"INSERT INTO COURSE (TITLE, DESCRIPTION) VALUES('{course.Title}','{course.Description}')";
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

        public void UpdateCourse(Course course)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"UPDATE COURSE SET TITLE = '{course.Title}', " +
                                   $"DESCRIPTION = '{course.Description}' " +
                                   $"WHERE COURSE_ID = {course.Course_Id}";
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

        public void DeleteCourse(int courseId) 
        {
            try 
            {
                Console.WriteLine(courseId);
                using (OracleConnection conn = new OracleConnection(connString)) 
                {
                    string query = $"UPDATE COURSE SET IS_DELETED = 1 WHERE COURSE_ID = {courseId}";
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
    }
}
