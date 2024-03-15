using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class ProgressTracking
    {
        public int? Student_Id { get; set; }
        public int? Lesson_Id { get; set; }
        public string? LessonStatus { get; set; }
        public DateTime LastAccessedDate { get; set; }

        public string connString = ProjectConstants.connString;

        public void AddProgress(ProgressTracking tracking)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "INSERT INTO COURSE_PROGRESS (STUDENT_ID, LESSON_ID, LESSON_STATUS) " +
                                         "VALUES(:STUDENT_ID, :LESSON_ID, :LessonStatus, TO_DATE(:ENROLLED_ON_DATE, 'YYYY-MM-DD'))";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", tracking.Student_Id));
                    cmd.Parameters.Add(new OracleParameter("LESSON_ID", tracking.Lesson_Id));
                    cmd.Parameters.Add(new OracleParameter("LESSON_STATUS", tracking.LessonStatus));
                    cmd.Parameters.Add(new OracleParameter("LAST_ACCESSED_DATE", tracking.LastAccessedDate.ToString("yyyy-MM-dd")));

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
    }
}
