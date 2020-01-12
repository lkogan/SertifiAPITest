using System;
using System.Collections.Generic;
using System.Text;

namespace SertifiAPITest.Models
{
    public interface IStudentDTO
    {
        int Id { get; set; }
        string Name { get; set; }
        int StartYear { get; set; }
        int EndYear { get; set; }
        List<double> GPARecord { get; set; }
    }
}
