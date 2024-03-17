using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Progress
    {
        public int? StudentId { get; set; }
        public int? LessonId { get; set; }
        public string? LessonStatus { get; set; }
        public string? StudentName { get; set; }
        public int? CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? LessonTitle { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public List<Progress> CourseProgresses = new List<Progress>();

        public string connString = ProjectConstants.connString;

        public void AddCourseProgress(Progress progress)
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

        public List<Progress> GetCourseProgress()
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
                        Progress progress = new Progress
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

        public Progress GetCourseProgressById(int studentId, int lessonId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT cp.STUDENT_ID, cp.LESSON_ID, cp.LESSON_STATUS, cp.LAST_ACCESSED_DATE, c.COURSE_ID, c.TITLE AS COURSE, s.STUDENT_NAME, " +
                                   $"l.TITLE AS LESSON FROM COURSE_PROGRESS cp " +
                                   $"JOIN LESSON l ON cp.LESSON_ID = l.LESSON_ID " +
                                   $"JOIN COURSE c ON l.COURSE_ID = c.COURSE_ID " +
                                   $"JOIN STUDENT s ON cp.STUDENT_ID = s.STUDENT_ID " +
                                   $"WHERE cp.STUDENT_ID = {studentId} AND cp.LESSON_ID = {lessonId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    Progress progress = new Progress();
                    while (reader.Read())
                    {
                        progress.StudentId = reader.GetInt32(0);
                        progress.LessonId = reader.GetInt32(1);
                        progress.LessonStatus = reader.GetString(2);
                        progress.LastAccessedDate = reader.GetDateTime(3);
                        progress.CourseId = reader.GetInt32(4);
                        progress.CourseTitle = reader.GetString(5);
                        progress.StudentName = reader.GetString(6);
                        progress.LessonTitle = reader.GetString(7);
                        
                    }
                    reader.Dispose();
                    conn.Close();
                    return progress;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public void UpdateLessonProgress(Progress progress)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "UPDATE COURSE_PROGRESS SET LAST_ACCESSED_DATE = TO_DATE(:LAST_ACCESSED_DATE, 'YYYY-MM-DD'), " +
                                         "LESSON_STATUS = :LESSON_STATUS " +
                                         "WHERE STUDENT_ID = :STUDENT_ID AND LESSON_ID = :LESSON_ID";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("LAST_ACCESSED_DATE", progress.LastAccessedDate.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new OracleParameter("LESSON_STATUS", progress.LessonStatus));
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", progress.StudentId));
                    cmd.Parameters.Add(new OracleParameter("LESSON_ID", progress.LessonId));

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

        public void DeleteCourseProgress(int studentId, int lessonId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "DELETE FROM COURSE_PROGRESS WHERE STUDENT_ID = :STUDENT_ID AND LESSON_ID = :LESSON_ID";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", studentId));
                    cmd.Parameters.Add(new OracleParameter("LESSON_ID", lessonId));

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

        public List<Progress> GetCourseProgressFiltered(string studentName, string courseTitle, string lessonTitle, DateTime? dateFrom, DateTime? dateTo)
        {
            List<Progress> filteredProgresses = new List<Progress>();

            // Start building your query
            string query = "SELECT cp.STUDENT_ID, cp.LESSON_ID, cp.LESSON_STATUS, cp.LAST_ACCESSED_DATE, c.COURSE_ID, c.TITLE AS COURSE, s.STUDENT_NAME, " +
                           "l.TITLE AS LESSON FROM COURSE_PROGRESS cp " +
                           "JOIN LESSON l ON cp.LESSON_ID = l.LESSON_ID " +
                           "JOIN COURSE c ON l.COURSE_ID = c.COURSE_ID " +
                           "JOIN STUDENT s ON cp.STUDENT_ID = s.STUDENT_ID WHERE 1=1";

            // Append conditions based on input
            if (!string.IsNullOrEmpty(studentName))
            {
                query += " AND UPPER(s.STUDENT_NAME) LIKE UPPER(:STUDENT_NAME)";
            }
            if (!string.IsNullOrEmpty(courseTitle))
            {
                query += " AND UPPER(c.TITLE) LIKE UPPER(:COURSE_TITLE)";
            }
            if (!string.IsNullOrEmpty(lessonTitle))
            {
                query += " AND UPPER(l.TITLE) LIKE UPPER(:LESSON_TITLE)";
            }
            if (dateFrom.HasValue)
            {
                query += " AND cp.LAST_ACCESSED_DATE >= :DATE_FROM";
            }
            if (dateTo.HasValue)
            {
                query += " AND cp.LAST_ACCESSED_DATE <= :DATE_TO";
            }

            using (OracleConnection conn = new OracleConnection(connString))
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                if (!string.IsNullOrEmpty(studentName))
                {
                    cmd.Parameters.Add(new OracleParameter("STUDENT_NAME", $"%{studentName}%"));
                }
                if (!string.IsNullOrEmpty(courseTitle))
                {
                    cmd.Parameters.Add(new OracleParameter("COURSE_TITLE", $"%{courseTitle}%"));
                }
                if (!string.IsNullOrEmpty(lessonTitle))
                {
                    cmd.Parameters.Add(new OracleParameter("LESSON_TITLE", $"%{lessonTitle}%"));
                }
                if (dateFrom.HasValue)
                {
                    cmd.Parameters.Add(new OracleParameter("DATE_FROM", dateFrom.Value));
                }
                if (dateTo.HasValue)
                {
                    cmd.Parameters.Add(new OracleParameter("DATE_TO", dateTo.Value));
                }

                conn.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Progress progress = new Progress
                    {
                        StudentId = reader.IsDBNull(reader.GetOrdinal("STUDENT_ID")) ? null : reader.GetInt32(reader.GetOrdinal("STUDENT_ID")),
                        LessonId = reader.IsDBNull(reader.GetOrdinal("LESSON_ID")) ? null : reader.GetInt32(reader.GetOrdinal("LESSON_ID")),
                        LessonStatus = reader.IsDBNull(reader.GetOrdinal("LESSON_STATUS")) ? null : reader.GetString(reader.GetOrdinal("LESSON_STATUS")),
                        LastAccessedDate = reader.IsDBNull(reader.GetOrdinal("LAST_ACCESSED_DATE")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LAST_ACCESSED_DATE")),
                        CourseId = reader.IsDBNull(reader.GetOrdinal("COURSE_ID")) ? null : reader.GetInt32(reader.GetOrdinal("COURSE_ID")),
                        CourseTitle = reader.IsDBNull(reader.GetOrdinal("COURSE")) ? null : reader.GetString(reader.GetOrdinal("COURSE")),
                        StudentName = reader.IsDBNull(reader.GetOrdinal("STUDENT_NAME")) ? null : reader.GetString(reader.GetOrdinal("STUDENT_NAME")),
                        LessonTitle = reader.IsDBNull(reader.GetOrdinal("LESSON")) ? null : reader.GetString(reader.GetOrdinal("LESSON"))
                    };
                    filteredProgresses.Add(progress);
                }
            }
            return filteredProgresses;
        }

    }
}
