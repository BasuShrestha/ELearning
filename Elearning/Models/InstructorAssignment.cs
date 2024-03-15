using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class InstructorAssignment
    {
        public int Course_Id { get; set; }

        public string Course_Title { get; set; }

        public int Instructor_Id { get; set; }
        public string Instructor_Name { get; set; }
        public int Is_Deleted { get; set; }

        public DateTime AssignedDate { get; set; }

        public List<InstructorAssignment> Assignments = new List<InstructorAssignment>();

        public string connString = ProjectConstants.connString;

        public void AddInstructorAssignment(InstructorAssignment assignment)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "INSERT INTO INSTRUCTOR_ASSIGNMENT (COURSE_ID, INSTRUCTOR_ID, ASSIGNED_DATE) " +
                                         "VALUES(:COURSE_ID, :INSTRUCTOR_ID, TO_DATE(:ASSIGNED_DATE, 'YYYY-MM-DD'))";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("COURSE_ID", assignment.Course_Id));
                    cmd.Parameters.Add(new OracleParameter("INSTRUCTOR_ID", assignment.Instructor_Id));
                    cmd.Parameters.Add(new OracleParameter("ASSIGNED_DATE", assignment.AssignedDate.ToString("yyyy-MM-dd")));

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

        public void GetInstructorAssignments()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT c.COURSE_ID, c.TITLE, i.INSTRUCTOR_ID, i.NAME, ia.ASSIGNED_DATE FROM INSTRUCTOR_ASSIGNMENT ia " +
                                   "JOIN COURSE c ON ia.COURSE_ID = c.COURSE_ID JOIN INSTRUCTOR i ON ia.INSTRUCTOR_ID = i.INSTRUCTOR_ID";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        InstructorAssignment assignment = new InstructorAssignment
                        {
                            
                            Course_Id = reader.GetInt32(0),
                            Course_Title = reader.GetString(1),
                            Instructor_Id = reader.GetInt32(2),
                            Instructor_Name = reader.GetString(3),
                            AssignedDate = reader.GetDateTime(4)
                        };
                        Assignments.Add(assignment);
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

        public InstructorAssignment GetInstructorAssignmentById(int courseId, int instructorId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT c.COURSE_ID, c.TITLE, i.INSTRUCTOR_ID, i.NAME, ia.ASSIGNED_DATE FROM INSTRUCTOR_ASSIGNMENT ia " +
                                   $"JOIN COURSE c ON ia.COURSE_ID = c.COURSE_ID JOIN INSTRUCTOR i ON ia.INSTRUCTOR_ID = i.INSTRUCTOR_ID " +
                                   $"WHERE ia.COURSE_ID = {courseId} AND ia.INSTRUCTOR_ID = {instructorId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    InstructorAssignment assignment = new InstructorAssignment();
                    while (reader.Read())
                    {
                        assignment.Course_Id = reader.GetInt32(0);
                        assignment.Course_Title = reader.GetString(1);
                        assignment.Instructor_Id = reader.GetInt32(2);
                        assignment.Instructor_Name = reader.GetString(3);
                        assignment.AssignedDate = reader.GetDateTime(4);
                    }
                    reader.Dispose();
                    conn.Close();
                    return assignment;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public void DeleteAssignment(int courseId, int instructorId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "DELETE FROM INSTRUCTOR_ASSIGNMENT WHERE COURSE_ID = :COURSE_ID AND INSTRUCTOR_ID = :INSTRUCTOR_ID";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("COURSE_ID", courseId));
                    cmd.Parameters.Add(new OracleParameter("INSTRUCTOR_ID", instructorId));

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
