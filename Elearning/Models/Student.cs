using System.Data;
using Elearning.Utilities;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Student
    {
        public int? Student_Id { get; set; }
        public string? Student_Name { get; set; }
        public string? Contact { get; set; }
        public string? DOB { get; set; }
        public DateOnly BirthDate = new DateOnly();
        public string? Email { get; set; }
        public string? Country { get; set; }
        public int? Is_Deleted { get; set; }

        public List<Student> Students = new List<Student>();

        public string connString = ProjectConstants.connString;

        public void GetStudents()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = "SELECT STUDENT_ID,STUDENT_NAME,CONTACT,DOB,EMAIL,COUNTRY,IS_DELETED FROM STUDENT";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Student_Id = reader.GetInt32(0),
                            Student_Name = reader.GetString(1),
                            Contact = reader.GetString(2),
                            DOB = reader.GetString(3),
                            Email = reader.GetString(4),
                            Country = reader.GetString(5),
                            Is_Deleted = reader.GetInt32(6)
                        };
                        Students.Add(student);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception)
            { 
                Console.WriteLine(exception.Message);
            }
        }

        public void GetStudentById(int studentId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"SELECT * FROM STUDENT WHERE STUDENT_ID= {studentId}";
                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Student student = new Student 
                        { 
                            Student_Id = reader.GetInt32(0),
                            Student_Name = reader.GetString(1),
                            Contact = reader.GetString(2),
                            DOB = reader.GetString(3),
                            Email = reader.GetString(4),
                            Country = reader.GetString(5),
                            Is_Deleted = reader.GetInt32(6)
                        };
                        Students.Add(student);

                    }
                    Console.WriteLine(Students[0]);
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

            }
        }

        public void AddStudent(Student student)
        {
            try
            {
                student.DOB = student.BirthDate.ToString("yyyy-MM-dd");
                Console.WriteLine(student.DOB);
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"INSERT INTO STUDENT (STUDENT_NAME, CONTACT, DOB, EMAIL, COUNTRY) " +
                        $"VALUES('{student.Student_Name}','{student.Contact}','{student.DOB}','{student.Email}','{student.Country}','',)";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

            }
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                student.DOB = student.BirthDate.ToString("yyyy-MM-dd");
                Console.WriteLine($"Student ID: {student.Student_Id}");
                Console.WriteLine($"Student Name: {student.Student_Name}");
                Console.WriteLine($"Contact: {student.Contact}");
                Console.WriteLine($"DOB: {student.DOB}");
                Console.WriteLine($"Email: {student.Email}");
                Console.WriteLine($"Country: {student.Country}");

                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"UPDATE STUDENT SET STUDENT_NAME = '{student.Student_Name}', CONTACT = '{student.Contact}', DOB = '{student.DOB}', EMAIL = '{student.Email}', COUNTRY = '{student.Country}' WHERE STUDENT_ID = {student.Student_Id}";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public void DeleteByID(int studentId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"UPDATE STUDENT SET IS_DELETED = 1 WHERE STUDENT_ID = {studentId}";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
