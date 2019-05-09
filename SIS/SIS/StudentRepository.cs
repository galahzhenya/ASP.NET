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
        private readonly List<Student> _students;

        public StudentRepository()
        {
            _students = new List<Student>();
        }

        public async Task<List<Student>> GetStudents()
        {
            return await Task.Run(() => _students);
        }

        public async Student GetStudents(int id)
        {
            return _students.FirstOrDefault(f= >f.Id == Id);
        }
    }
}