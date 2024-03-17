using System.Reflection.Metadata;
using Elearning.Utilities;
using Humanizer;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Models
{
    public class Lesson
    {
        public int? Lesson_Id { get; set; }
        public string? Title { get; set; }
        public int? Course_Id { get; set; }
        public int? Is_Deleted { get; set; }
        public string? Content_Name { get; set; }
        public string? Content_Type { get; set; }

        public string? Course_Title { get; set; }

        public List<Lesson> Lessons = new List<Lesson>();

        public string connString = ProjectConstants.connString;

        public void GetLessons()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT l.LESSON_ID, l.TITLE, l.COURSE_ID, l.IS_DELETED, c.TITLE, l.CONTENT_NAME " +
                                   $"FROM LESSON l JOIN COURSE c ON l.COURSE_ID = c.COURSE_ID";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lesson lesson = new Lesson();
                        lesson.Lesson_Id = reader.GetInt32(0);
                        lesson.Title = reader.GetString(1);
                        lesson.Course_Id = reader.GetInt32(2);
                        lesson.Is_Deleted = reader.GetInt32(3);
                        lesson.Course_Title = reader.GetString(4);
                        lesson.Content_Name = reader.GetString(5);
                        Lessons.Add(lesson);
                    }
                    reader.Dispose();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public Lesson GetLessonByLessonId(int lessonId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT l.LESSON_ID, l.TITLE, l.COURSE_ID, l.IS_DELETED, c.TITLE, l.CONTENT_NAME " +
                                   $"FROM LESSON l JOIN COURSE c ON l.COURSE_ID = c.COURSE_ID " +
                                   $"WHERE LESSON_ID = {lessonId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    Lesson lesson = new Lesson();
                    while (reader.Read())
                    {
                        lesson.Lesson_Id = reader.GetInt32(0);
                        lesson.Title = reader.GetString(1);
                        lesson.Course_Id = reader.GetInt32(2);
                        lesson.Is_Deleted= reader.GetInt32(3);
                        lesson.Course_Title = reader.GetString(4);
                        lesson.Content_Name = reader.GetString(5);
                    }
                    reader.Dispose();
                    conn.Close();
                    return lesson;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
        }

        public List<Lesson> GetLessonByCourseId(int courseId)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"SELECT LESSON_ID, TITLE, COURSE_ID, IS_DELETED, CONTENT_NAME " +
                                   $"FROM LESSON WHERE COURSE_ID = {courseId}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    cmd.BindByName = true;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    List<Lesson> courseLessons = new List<Lesson>();
                    
                    while (reader.Read())
                    {
                        Lesson lesson = new Lesson();

                        lesson.Lesson_Id = reader.GetInt32(0);
                        lesson.Title = reader.GetString(1);
                        lesson.Course_Id = reader.GetInt32(2);
                        lesson.Is_Deleted = reader.GetInt32(3);
                        lesson.Content_Name = reader.GetString(4);
                        courseLessons.Add(lesson);
                    }
                    reader.Dispose();
                    conn.Close();
                    return courseLessons;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
        }

        public void AddLesson(Lesson lesson)
        {
            try
            {
                if (lesson.Content_Type == "PDF" || lesson.Content_Type == "Video")
                {
                    if (lesson.Content_Type == "Video")
                    {
                        lesson.Content_Name = $"{lesson.Content_Name}.mp4";
                    }
                    else
                    { 
                        lesson.Content_Name = $"{lesson.Content_Name}.{lesson.Content_Type?.ToLower()}";
                    }
                }
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    Console.WriteLine(lesson.Content_Name);
                    string query = $"INSERT INTO LESSON (TITLE, COURSE_ID, CONTENT_NAME) VALUES('{lesson.Title}',{lesson.Course_Id},'{lesson.Content_Name}')";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void UpdateLesson(Lesson lesson)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {

                    Console.WriteLine(lesson.Title);
                    Console.WriteLine(lesson.Course_Id);
                    Console.WriteLine(lesson.Lesson_Id);
                    string query = $"UPDATE LESSON SET TITLE = '{lesson.Title}', " +
                                   $"COURSE_ID = '{lesson.Course_Id}' " +
                                   $"WHERE LESSON_ID = {lesson.Lesson_Id}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void DeleteLesson(int id)
        {
            try
            {
                Console.WriteLine($"and here: { id}");
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    string query = $"UPDATE LESSON SET IS_DELETED = 1 WHERE LESSON_ID = {id}";
                    OracleCommand cmd = new OracleCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
