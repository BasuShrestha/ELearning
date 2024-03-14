using System.Diagnostics;
using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elearning.Controllers
{
    public class LessonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LessonView()
        {
            Lesson lesson = new Lesson();
            lesson.GetLessons();
            ViewBag.SQLData = lesson;

            Course course = new Course();
            course.GetCourses();
            if (course.Courses != null)
            {
                List<Course> availableCourses = new List<Course> { };
                foreach (Course c in course.Courses)
                {
                    if (c.Is_Deleted == 0)
                    {
                        availableCourses.Add(c);
                    }
                }
                ViewBag.CourseList = new SelectList(availableCourses, "Course_Id", "Title");
            }

            List<String> lessonTypes = new List<String> 
            { 
                "PDF", "Video", "Link"
            };
            ViewBag.LessonTypes = new SelectList(lessonTypes, "LessonType");

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult AddLesson(Lesson lesson)
        {
            if (!ModelState.IsValid)
            {
                return View(lesson);
            }

            if (string.IsNullOrEmpty(lesson.Title))
            {
                TempData["Message"] = "Empty values in the fields";
                return View(lesson);
            }

            AddLessonDetail(lesson);

            TempData["Message"] = "Lesson details inserted successfully";
            return RedirectToAction("LessonView");
        }
        
        [HttpPost]
        public IActionResult AddLessonDetail(Lesson lesson)
        {
            if (string.IsNullOrEmpty(lesson.Title) || string.IsNullOrEmpty(lesson.Course_Id.ToString()))
            {
                TempData["Message"] = "Empty field values";
                return View();
            }
            else
            {
                lesson.AddLesson(lesson);
                ViewBag.Message = "Lesson details added";
                TempData["Message"] = "Lesson details inserted successfully";
                return RedirectToAction("LessonView");
            }
        }

        public IActionResult UpdateLesson(int id)
        {
            Lesson lesson = new();
            lesson = lesson.GetLessonById(id);
            ViewBag.SQLData = lesson;

            Course course = new Course();
            course.GetCourses();
            if (course.Courses != null)
            {
                List<Course> availableCourses = new List<Course> { };
                foreach (Course c in course.Courses)
                {
                    if (c.Is_Deleted == 0)
                    {
                        availableCourses.Add(c);
                    }
                }
                ViewBag.CourseList = new SelectList(availableCourses, "Course_Id", "Title");
            }
            List<String> lessonTypes = new List<String>
            {
                "PDF", "Video", "Link"
            };
            ViewBag.LessonTypes = new SelectList(lessonTypes, "LessonType");
            return View("UpdateLesson", lesson);
        }

        [HttpPost]
        public IActionResult UpdateLesson(Lesson lesson)
        {
            try
            {
                if (string.IsNullOrEmpty(lesson.Title) || string.IsNullOrEmpty(lesson.Course_Id.ToString()))
                {
                    TempData["Message"] = "Empty fields submitted";
                    return RedirectToAction("LessonView");
                }
                else
                {
                    Lesson updatedLesson = new Lesson();
                    updatedLesson.Title = lesson.Title;
                    updatedLesson.Course_Id = lesson.Course_Id;
                    Console.WriteLine(lesson.Title);
                    Console.WriteLine(lesson.Course_Id);
                    updatedLesson.UpdateLesson(lesson);
                    TempData["Message"] = $"Lesson: {lesson.Title} updated successfully";
                    return RedirectToAction("LessonView");
                }
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
                return RedirectToAction("LessonView");
            }
        }

        public IActionResult DeleteLesson()
        {
            return View(new Lesson());
        }

        [HttpPost]
        public IActionResult DeleteLesson(int id)
        {
            try
            {
                Console.WriteLine($"del: {id}");
                Lesson newLesson = new Lesson();
                newLesson.DeleteLesson(id);
                ViewBag.Message = $"Lesson deleted from the database";
                Console.WriteLine(id);
                TempData["Message"] = $"Lesson with id {id} deleted!";
                return RedirectToAction("LessonView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
                return RedirectToAction("LessonView");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
