using System;
using System.Collections.Generic;
using System.Text;

namespace SertifiAPITest.Models
{
    public interface IStudentStatistics
    {
        double GPASwing { get; }
        double AverageGPA { get; }
    }
}
