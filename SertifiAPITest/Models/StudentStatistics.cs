using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SertifiAPITest.Models
{
    public class StudentStatistics : IStudentDTO, IStudentStatistics
    {
        public StudentStatistics(IStudentDTO dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.StartYear = dto.StartYear;
            this.EndYear = dto.EndYear;
            this.GPARecord = dto.GPARecord;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public List<double> GPARecord { get; set; }

        public double GPASwing { get { return GPARecord.Max() - GPARecord.Min(); } }

        public double AverageGPA { get { return GPARecord.Average(); } }

        public Dictionary<int, double> ByYearGPA { get { return TransformGPADataByYear(); } }

        private Dictionary<int, double> TransformGPADataByYear()
        {
            Dictionary<int, double> result = new Dictionary<int, double>();

            int currentYear = StartYear;

            for (int i = 0; i < GPARecord.Count; i++)
            {
                result.Add(currentYear, GPARecord[i]);

                currentYear++;
            }

            return result;
        }
    }
}
