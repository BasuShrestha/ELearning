using Elearning.Utilities;
using NuGet.Protocol;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Enrolment
    {
        public int Student_Id { get; set; }
        public int Course_Id { get; set; }
        public DateTime EnrolledOnDate { get; set; }
        public string Student_Name { get; set; }
        public string Course_Title { get; set; }
        public int Is_Deleted { get; set; }

        public ICollection<Enrolment> Enrolments = new List<Enrolment>();

        public string connString = ProjectConstants.connString;

        public void AddEnrolment(Enrolment enrolment)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "INSERT INTO ENROLMENT (STUDENT_ID, COURSE_ID, ENROLLED_ON_DATE) " +
                                         "VALUES(:STUDENT_ID, :COURSE_ID, TO_DATE(:ENROLLED_ON_DATE, 'YYYY-MM-DD'))";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", enrolment.Student_Id));
                    cmd.Parameters.Add(new OracleParameter("COURSE_ID", enrolment.Course_Id));
                    cmd.Parameters.Add(new OracleParameter("ENROLLED_ON_DATE", enrolment.EnrolledOnDate.ToString("yyyy-MM-dd")));

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

        public void GetEnrolments()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT e.STUDENT_ID, e.COURSE_ID, e.ENROLLED_ON_DATE, s.STUDENT_NAME, c.TITLE, e.IS_DELETED " +
                                   "FROM ENROLMENT e JOIN STUDENT s ON e.STUDENT_ID = s.STUDENT_ID " +
                                   "JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Enrolment enrollment = new Enrolment
                        {
                            Student_Id = reader.GetInt32(0),
                            Course_Id = reader.GetInt32(1),
                            EnrolledOnDate = reader.GetDateTime(2),
                            Student_Name = reader.GetString(3),
                            Course_Title = reader.GetString(4),
                            Is_Deleted = reader.GetInt32(5)
                        };
                        Enrolments.Add(enrollment);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public void DeleteEnrolment(int studentId, int courseId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "DELETE FROM ENROLMENT WHERE STUDENT_ID = :STUDENT_ID AND COURSE_ID = :COURSE_ID";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("STUDENT_ID", studentId));
                    cmd.Parameters.Add(new OracleParameter("COURSE_ID", courseId));

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
