using System.Data;
using Elearning.Utilities;
using Humanizer;
using NuGet.Protocol;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class EnrolledCourse : Course
    {
        public DateTime EnrolledOnDate { get; set; }
        public int EnrolmentDeleted { get; set; }
        public int EnrolmentCount { get; set; } // ADDED TO COUNT TOTAL ENROLMENTS

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

        public List<Student> GetDistinctStudents()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "SELECT DISTINCT s.STUDENT_ID,s.STUDENT_NAME,s.CONTACT,s.DOB,s.EMAIL,s.COUNTRY,s.IS_DELETED " + 
                                         "FROM ENROLMENT e JOIN STUDENT s ON e.STUDENT_ID = s.STUDENT_ID WHERE s.IS_DELETED = 0";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    List<Student> distinctEnrolledStudents= new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Student_Id = reader.GetInt32(0),
                            Student_Name = reader.GetString(1),
                            Contact = reader.GetString(2),
                            DOB = reader.GetDateTime(3),
                            Email = reader.GetString(4),
                            Country = reader.GetString(5),
                            Is_Deleted = reader.GetInt32(6)
                        };

                        distinctEnrolledStudents.Add(student);
                    }

                    reader.Dispose();
                    conn.Close();
                    return distinctEnrolledStudents;
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
                                         "WHERE c.IS_DELETED = 0";

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

        public List<EnrolledCourse> GetTopThreeEnrolledCoursesWithCounts(int month, int year)
        {
            List<EnrolledCourse> topEnrolledCourses = new List<EnrolledCourse>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = @"
                        SELECT c.COURSE_ID, c.TITLE, COUNT(e.STUDENT_ID) AS ENROLLMENT_COUNT
                        FROM ENROLMENT e
                        JOIN COURSE c ON e.COURSE_ID = c.COURSE_ID
                        WHERE EXTRACT(MONTH FROM e.ENROLLED_ON_DATE) = :MONTH
                        AND EXTRACT(YEAR FROM e.ENROLLED_ON_DATE) = :YEAR
                        AND c.IS_DELETED = 0
                        AND e.IS_DELETED = 0
                        GROUP BY c.COURSE_ID, c.TITLE
                        ORDER BY ENROLLMENT_COUNT DESC
                        FETCH FIRST 3 ROWS ONLY";

                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.Parameters.Add(new OracleParameter("MONTH", month));
                    cmd.Parameters.Add(new OracleParameter("YEAR", year));
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();

                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EnrolledCourse enrolledCourse = new EnrolledCourse
                        {
                            Course_Id = reader.GetInt32(0),
                            Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                            EnrolmentCount = reader.GetInt32(2)
                        };
                        topEnrolledCourses.Add(enrolledCourse);
                    }

                    reader.Dispose();
                    conn.Close();

                    return topEnrolledCourses;
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

