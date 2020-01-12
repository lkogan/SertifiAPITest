using SertifiAPITest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SertifiAPITest.Helpers
{
    public class Calculations
    {
        public int GetStudentWithLargestGPASwing(List<StudentStatistics> studentData)
        {
            int studentId = -1;

            try
            {
                if (studentData != null && studentData.Any())
                {
                    StudentStatistics student = studentData.OrderByDescending(x => x.GPASwing).First();
                    studentId = student.Id;
                }
                else
                {
                    return studentId;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return studentId;
            }

            return studentId;
        }

        public List<int> TopTenStudentsWithHighestGPA(List<StudentStatistics> studentData)
        {
            List<int> topTen = new List<int>();

            try
            {
                if (studentData != null && studentData.Any())
                {
                    topTen = studentData.OrderByDescending(s => s.AverageGPA).Take(10).Select(x => x.Id).ToList();

                    return topTen;
                }
                else
                {
                    return topTen;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return topTen;
            }
        }

        public int GetYearWithHighestAttendance(List<StudentStatistics> students)
        {
            int year = -1;
             
            try
            {
                if (students != null && students.Any())
                { 
                    var studentGroup = students.SelectMany(x => x.ByYearGPA)
                                      .GroupBy(x => x.Key)
                                      .Select(x => new { x.Key, Count = x.Count() });

                    int maxYears = studentGroup.Max(x => x.Count);

                    List<int> highestAttendendanceYear = studentGroup.Where(x => x.Count == maxYears).Select(x => x.Key).ToList();

                    return highestAttendendanceYear.Min(x => x);
                }
                else
                {
                    return year;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return year;
            }
        }
          
        public int GetYearWithHighestGPA(List<StudentStatistics> students)
        {
            int year = -1;

            try
            {
                if (students != null && students.Any())
                {
                    year = students.SelectMany(g => g.ByYearGPA)
                               .GroupBy(g => g.Key)
                               .Select(x => new { Year = x.Key, GpaAverage = x.Average(i => i.Value) })
                               .OrderByDescending(x => x.GpaAverage)
                               .FirstOrDefault()
                               .Year;

                    return year;
                }
                else
                {
                    return year;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return year;
            }
        }
    }
}
