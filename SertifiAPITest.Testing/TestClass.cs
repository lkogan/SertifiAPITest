using System;
using System.IO;
using Xunit;
using SertifiAPITest.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using SertifiAPITest.Helpers;
using System.Threading.Tasks;

namespace SertifiAPITest.Testing
{
    public class TestClass
    {
        private List<StudentDTO> _studentsData = null;

        private List<StudentStatistics> _studentsStats = null;

        private HttpHelper httpHelper = null;

        private const string BASE_API_URL = "http://apitest.sertifi.net/";

        private const string STUDENTS_API_URL = "http://apitest.sertifi.net/api/Students";

        private const string STUDENT_AGGREGATE_API_URL = "http://apitest.sertifi.net/api/StudentAggregate";

        public TestClass()
        {
            var studentsJson = File.ReadAllText(@"SampleData\Students.json");

            _studentsData = JsonConvert.DeserializeObject<List<StudentDTO>>(studentsJson);

            _studentsStats = _studentsData.Select(x => new StudentStatistics(x)).ToList();

            httpHelper = new HttpHelper(BASE_API_URL);
        }

        [Fact]
        public void DoesNumberOfYearsMatchCountOfGPAs()
        {
            StudentDTO firstStudent = _studentsData.First();

            int NumberOfYears = firstStudent.EndYear - firstStudent.StartYear + 1;

            Assert.Equal(NumberOfYears, firstStudent.GPARecord.Count);
        }

        [Fact]
        public void AtLeastOneStudentExists()
        {
            StudentDTO firstStudent = _studentsData.FirstOrDefault();
             
            Assert.NotNull(firstStudent);
        }

        [Fact]
        public void NoDuplicateIDs()
        {
            bool duplicatesExists = _studentsData.GroupBy(n => n.Id).Any(c => c.Count() > 1);

            Assert.True(!duplicatesExists);
        }

         
        [Fact]
        public void CompareActualDataWithSample()
        {
            Task<List<StudentDTO>> getStudentsTask = httpHelper.GetAsync(STUDENTS_API_URL);

            List<StudentDTO> apiData = getStudentsTask.Result;
             
            Assert.True(_studentsData.SelectMany(x => x.GPARecord).SequenceEqual(apiData.SelectMany(x => x.GPARecord)));
        }

        [Fact]
        public void VerifyHighestGPAYear()
        { 
            Calculations calc = new Calculations();
            var highestGPAYear = calc.GetYearWithHighestGPA(_studentsStats);

            Assert.Equal(2016, highestGPAYear);
        }

        [Fact]
        public void VerifyHighestAttendanceYear()
        { 
            Calculations calc = new Calculations();
            var highestAttendanceYear = calc.GetYearWithHighestAttendance(_studentsStats);

            Assert.NotEqual(2020, highestAttendanceYear);
        }

        [Fact]
        public void VerifyStudentIsWithLargestGPASwing()
        {
            Calculations calc = new Calculations();
            var studentIdWithLargestSwing = calc.GetStudentWithLargestGPASwing(_studentsStats);

            Assert.Equal(15, studentIdWithLargestSwing);
        }
    }
}
