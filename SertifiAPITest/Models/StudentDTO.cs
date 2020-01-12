using System;
using System.Collections.Generic;
using System.Text;

namespace SertifiAPITest.Models
{
    public class StudentDTO : IStudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public List<double> GPARecord { get; set; }
    }
}
