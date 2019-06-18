using SIS.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SIS
{
    public class StudentRepository
    {
        private static List<Student> _students;

        public StudentRepository()
        {
            _students = new List<Student> { new Student { Id = 1, Email = "galax@mail.ru", FirstName = "Женя", LastName = "Галах" } };
        }

        public async Task<List<Student>> GetStudents()
        {
            return await Task.Run(() => _students);
        }

        public async Task<Student> GetStudents(int id)
        {
            return await Task.Run(()=> _students.FirstOrDefault(f =>f.Id == id));
        }
/*
        public async Task<Student> AddStudent(Student student)
        {
            _students.Add(student);
        }
        */
    }

}