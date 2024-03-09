using Elearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentView()
        {
            Student student = new Student();
            student.GetStudents();
            ViewBag.sqldata = student;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            if (string.IsNullOrEmpty(student.Student_Name)
                || string.IsNullOrEmpty(student.Contact)
                || string.IsNullOrEmpty(student.DOB)
                || string.IsNullOrEmpty(student.Email)
                || string.IsNullOrEmpty(student.Country))
            {
                TempData["Message"] = "Please fill all the fields before submission";
                return View(student);
            }
            else
            {
                student.AddStudent(student);
                TempData["Message"] = "Student Added Successfully";
                return RedirectToAction("StudentView");
            }
        }

        public IActionResult UpdateStudent(int studentId) 
        {
            Student student = new Student();
            student.GetStudentById(studentId);
            ViewBag.studentData = student;
            return View("UpdateStudent",student);
        }

        public IActionResult UpdateStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please fill all the fields before submission";
                return View(student);
            }
            else 
            {
                student.UpdateStudent(student);
                return View();
            }
        }
    }
}
