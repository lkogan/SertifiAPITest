using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SertifiAPITest.Helpers;
using SertifiAPITest.Models;

namespace SertifiAPITest
{
    class Program
    {
        private const string BASE_API_URL = "http://apitest.sertifi.net/";

        private const string STUDENTS_API_URL = "http://apitest.sertifi.net/api/Students";

        private const string STUDENT_AGGREGATE_API_URL = "http://apitest.sertifi.net/api/StudentAggregate";

        private const string SUBMITTER_NAME = "Leo Kogan";

        private const string SUBMITTER_EMAIL = "k_leo@hotmail.com";
         
        static void Main(string[] args)
        { 
            //Get student data from API
            HttpHelper httpHelper = new HttpHelper(BASE_API_URL);
            Task<List<StudentDTO>> getStudentsTask = httpHelper.GetAsync(STUDENTS_API_URL);
            List<StudentDTO> studentData = getStudentsTask.Result;
            
            
            //Convert to list of classes containing calculated stats, in addition to data
            List<StudentStatistics> studentStatistics = studentData.Select(x => new StudentStatistics(x)).ToList();
            
            
            //Perform calculations in order to obtain 4 metrics desired
            Calculations calc = new Calculations(); 
            var highestAttendanceYear = calc.GetYearWithHighestAttendance(studentStatistics);       
            var highestGPAYear = calc.GetYearWithHighestGPA(studentStatistics);
            var topTen = calc.TopTenStudentsWithHighestGPA(studentStatistics);
            var studentIdWithLargestSwing = calc.GetStudentWithLargestGPASwing(studentStatistics);
             

            //Build a response object
            Response response = new Response
            {
                YourName = SUBMITTER_NAME,
                YourEmail = SUBMITTER_EMAIL,
                YearWithHighestAttendance = highestAttendanceYear,
                YearWithHighestOverallGpa = highestGPAYear,
                Top10StudentIdsWithHighestGpa = topTen,
                StudentIdMostInconsistent = studentIdWithLargestSwing
            };
             

            //Send the response to API 
            Task<string> jsonoutput = httpHelper.PutAsync(STUDENT_AGGREGATE_API_URL, response);
            
            
            //Output input and output on the console
            string input = JsonConvert.SerializeObject(studentData, Formatting.Indented);
            string output = JsonConvert.SerializeObject(response, Formatting.Indented);

            Console.WriteLine("Received data:");
            Console.WriteLine(input);
            Console.WriteLine("");
            Console.WriteLine("Submitted response:");
            Console.WriteLine(output);
            Console.WriteLine("");

            Console.ReadLine();
        }
    }
}
