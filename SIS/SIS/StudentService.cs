using SIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SIS
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepositiry;

        public StudentService()
        {
            _studentRepositiry = new StudentRepository();
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _studentRepositiry.GetStudents();
        }
    }
}