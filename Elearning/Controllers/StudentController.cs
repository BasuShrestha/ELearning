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

        public IActionResult AddStudent()
        {
            return View("AddStudentDetail");
        }

        [HttpPost]
        public IActionResult AddStudentDetail(Student student)
        {

            Console.WriteLine($"Student ID: {student.Student_Id}");
            Console.WriteLine($"Student Name: {student.Student_Name}");
            Console.WriteLine($"Contact: {student.Contact}");
            Console.WriteLine($"DOB: {student.DOB}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Country: {student.Country}");
            if (string.IsNullOrEmpty(student.Student_Name)
                || string.IsNullOrEmpty(student.Contact)
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
            Console.WriteLine($"In here: {studentId}");
            Student student = new Student();
            student.GetStudentById(studentId);
            Console.WriteLine(student.Students[0].Student_Name);
            ViewBag.studentData = student;
            return View("UpdateStudent", student);
        }

        [HttpPost]
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
                return RedirectToAction("StudentView");
            }
        }

        public IActionResult DeleteStudent()
        { 
            return View(new Student());
        }

        [HttpPost]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                Student student = new Student();
                student.DeleteById(id);
                ViewBag.Message = $"Student deleted from the database";
                Console.WriteLine(id);
                TempData["Message"] = "Student deleted!";
                return RedirectToAction("StudentView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
                return RedirectToAction("StudentView");
            }
        }
    }
}
