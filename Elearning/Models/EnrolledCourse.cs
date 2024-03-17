using System.Data;
using Elearning.Utilities;
using NuGet.Protocol;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class EnrolledCourse : Course
    {
        public DateTime EnrolledOnDate { get; set; }
        public int EnrolmentDeleted { get; set; }

        public List<EnrolledCourse> EnrolledCourses = new List<EnrolledCourse>();

        public List<EnrolledCourse> GetAllEnrolledCourses()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"SELECT c.COURSE_ID,c.TITLE,c.DESCRIPTION,c.IS_DELETED,e.ENROLLED_ON_DATE,e.IS_DELETED " +
                                         $"FROM ENROLMENT e JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID";
                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EnrolledCourse enrolledCourse = new EnrolledCourse
                        {
                            Course_Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Is_Deleted = reader.GetInt32(3),
                            EnrolledOnDate = reader.GetDateTime(4),
                            EnrolmentDeleted = reader.GetInt32(5)
                        };
                        EnrolledCourses.Add(enrolledCourse);
                    }
                    reader.Dispose();
                    conn.Close();
                    return EnrolledCourses;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }

        }

        public List<EnrolledCourse> GetDistinctEnrolledCourses()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "SELECT DISTINCT c.COURSE_ID, c.TITLE, c.DESCRIPTION, c.IS_DELETED " +
                                         "FROM ENROLMENT e JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID " +
                                         "WHERE c.IS_DELETED = 0"; // Assuming you want only active courses

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    List<EnrolledCourse> distinctEnrolledCourses = new List<EnrolledCourse>();
                    while (reader.Read())
                    {
                        EnrolledCourse enrolledCourse = new EnrolledCourse
                        {
                            Course_Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Is_Deleted = reader.GetInt32(3)
                        };

                        distinctEnrolledCourses.Add(enrolledCourse);
                    }

                    reader.Dispose();
                    conn.Close();
                    return distinctEnrolledCourses;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public List<EnrolledCourse> GetDistinctLesson()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "SELECT DISTINCT c.COURSE_ID, c.TITLE, c.DESCRIPTION, c.IS_DELETED " +
                                         "FROM ENROLMENT e JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID " +
                                         "WHERE c.IS_DELETED = 0"; // Assuming you want only active courses

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    List<EnrolledCourse> distinctEnrolledCourses = new List<EnrolledCourse>();
                    while (reader.Read())
                    {
                        EnrolledCourse enrolledCourse = new EnrolledCourse
                        {
                            Course_Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Is_Deleted = reader.GetInt32(3)
                        };

                        distinctEnrolledCourses.Add(enrolledCourse);
                    }

                    reader.Dispose();
                    conn.Close();
                    return distinctEnrolledCourses;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }


        public List<EnrolledCourse> GetEnrolledCoursesByStudentId(int studentId)
        {
            try
            {
                Console.WriteLine(studentId);
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"SELECT c.COURSE_ID,c.TITLE,c.DESCRIPTION,c.IS_DELETED,e.ENROLLED_ON_DATE,e.IS_DELETED " +
                                         $"FROM ENROLMENT e JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID WHERE e.STUDENT_ID = {studentId}";
                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EnrolledCourse enrolledCourse = new EnrolledCourse
                        {
                            Course_Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Is_Deleted = reader.GetInt32(3),
                            EnrolledOnDate = reader.GetDateTime(4),
                            EnrolmentDeleted = reader.GetInt32(5)
                        };
                        EnrolledCourses.Add(enrolledCourse);
                    }
                    reader.Dispose();
                    conn.Close();
                    return EnrolledCourses;
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
