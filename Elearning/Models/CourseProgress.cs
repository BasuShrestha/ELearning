using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class CourseProgress
    {
        public int? StudentId { get; set; }
        public int? LessonId { get; set; }
        public string? LessonStatus { get; set; }
        public string? StudentName { get; set; }
        public int? CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? LessonTitle { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public List<CourseProgress> CourseProgresses = new List<CourseProgress>();

        public string connString = ProjectConstants.connString;

        public void AddCourseProgress(CourseProgress progress)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "INSERT INTO COURSE_PROGRESS (STUDENT_ID, LESSON_ID, LESSON_STATUS, LAST_ACCESSED_DATE) " +
                                         "VALUES(:STUDENT_ID, :LESSON_ID, :LESSON_STATUS, TO_DATE(:LAST_ACCESSED_DATE, 'YYYY-MM-DD'))";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", progress.StudentId));
                    cmd.Parameters.Add(new OracleParameter("LESSON_ID", progress.LessonId));
                    cmd.Parameters.Add(new OracleParameter("LESSON_STATUS", progress.LessonStatus));
                    cmd.Parameters.Add(new OracleParameter("LAST_ACCESSED_DATE", progress.LastAccessedDate.ToString("yyyy-MM-dd")));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public List<CourseProgress> GetCourseProgress()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT cp.STUDENT_ID, cp.LESSON_ID, cp.LESSON_STATUS, cp.LAST_ACCESSED_DATE, c.COURSE_ID, c.TITLE AS COURSE, s.STUDENT_NAME, " +
                                   "l.TITLE AS LESSON FROM COURSE_PROGRESS cp " +
                                   "JOIN LESSON l ON cp.LESSON_ID = l.LESSON_ID " +
                                   "JOIN COURSE c ON l.COURSE_ID = c.COURSE_ID " +
                                   "JOIN STUDENT s ON cp.STUDENT_ID = s.STUDENT_ID";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CourseProgress progress = new CourseProgress
                        {
                            StudentId = reader.GetInt32(0),
                            LessonId = reader.GetInt32(1),
                            LessonStatus = reader.GetString(2),
                            LastAccessedDate = reader.GetDateTime(3),
                            CourseId = reader.GetInt32(4),
                            CourseTitle = reader.GetString(5),
                            StudentName = reader.GetString(6),
                            LessonTitle = reader.GetString(7)
                        };
                        CourseProgresses.Add(progress);

                    }
                    reader.Dispose();
                    conn.Close();
                    return CourseProgresses;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }
    }
}
