using System.Net.Http;
using System.Net.Http.Json;
using StudentInformationSystemMobile.Models;

namespace StudentInformationSystemMobile.Services
{
    public class DatabaseService
    {
        private static readonly HttpClient Client = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public async Task<Student?> GetDemoStudentAsync()
        {
            const string url =
                "http://10.0.2.2:5080/api/students/2026-TEST-001";

            return await Client.GetFromJsonAsync<Student>(url);
        }
    }
}