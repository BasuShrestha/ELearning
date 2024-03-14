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
        public DateTime DOB { get; set; }
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
                            DOB = reader.GetDateTime(3),
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
            try { 
                Console.WriteLine(studentId);
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"SELECT STUDENT_ID,STUDENT_NAME,CONTACT,DOB,EMAIL,COUNTRY,IS_DELETED FROM STUDENT WHERE STUDENT_ID = {studentId}";
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
                            DOB = reader.GetDateTime(3),
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

        //public void AddStudent(Student student)
        //{
        //    try
        //    {
        //        student.DOB = student.BirthDate.ToString("yyyy-MM-dd");
        //        Console.WriteLine(student.Student_Name);
        //        Console.WriteLine(student.Contact);
        //        Console.WriteLine(student.DOB);
        //        Console.WriteLine(student.Email);
        //        Console.WriteLine(student.Country);
        //        using (OracleConnection conn = new OracleConnection(connString))
        //        {
        //            string queryString = $"INSERT INTO STUDENT (STUDENT_NAME, CONTACT, DOB, EMAIL, COUNTRY) " +
        //                $"VALUES('{student.Student_Name}','{student.Contact}',{student.DOB},'{student.Email}','{student.Country}')";

        //            OracleCommand cmd = new OracleCommand(queryString, conn);
        //            conn.Open();
        //            cmd.ExecuteReader();
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);

        //    }
        //}

        public void AddStudent(Student student)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = "INSERT INTO STUDENT (STUDENT_NAME, CONTACT, DOB, EMAIL, COUNTRY) " +
                                         "VALUES(:StudentName, :Contact, TO_DATE(:DOB, 'YYYY-MM-DD'), :Email, :Country)";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("StudentName", student.Student_Name));
                    cmd.Parameters.Add(new OracleParameter("Contact", student.Contact));
                    cmd.Parameters.Add(new OracleParameter("DOB", student.DOB.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new OracleParameter("Email", student.Email));
                    cmd.Parameters.Add(new OracleParameter("Country", student.Country));

                    conn.Open();
                    cmd.ExecuteNonQuery();
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
                //student.DOB = student.BirthDate.ToString("yyyy-MM-dd");
                Console.WriteLine($"Student ID: {student.Student_Id}");
                Console.WriteLine($"Student Name: {student.Student_Name}");
                Console.WriteLine($"Contact: {student.Contact}");
                //Console.WriteLine($"BirthDate: {student.BirthDate}");
                Console.WriteLine($"DOB: {student.DOB}");
                Console.WriteLine($"Email: {student.Email}");
                Console.WriteLine($"Country: {student.Country}");

                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string queryString = $"UPDATE STUDENT SET STUDENT_NAME = :StudentName, CONTACT = :Contact, DOB = TO_DATE(:DOB, 'YYYY-MM-DD'), EMAIL = :Email, COUNTRY = :Country" +
                                         $"WHERE STUDENT_ID = :StudentId";

                    OracleCommand cmd = new OracleCommand(queryString, conn);
                    cmd.Parameters.Add(new OracleParameter("StudentName", student.Student_Name));
                    cmd.Parameters.Add(new OracleParameter("Contact", student.Contact));
                    cmd.Parameters.Add(new OracleParameter("DOB", student.DOB.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new OracleParameter("Email", student.Email));
                    cmd.Parameters.Add(new OracleParameter("Country", student.Country));
                    cmd.Parameters.Add(new OracleParameter("StudentId", student.Student_Id));

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
        public void DeleteById  (int studentId)
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
