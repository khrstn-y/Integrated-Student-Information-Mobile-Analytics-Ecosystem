using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using StudentInformationSystemMobile.Models;

namespace StudentInformationSystemMobile.Services
{
    public class DatabaseService
    {
        private static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri("http://10.0.2.2:5080/"),
            Timeout = TimeSpan.FromSeconds(10)
        };

        public Task<Student?> GetDemoStudentAsync()
        {
            return Client.GetFromJsonAsync<Student>(
                "api/students/2026-TEST-001");
        }

        public async Task<Student?> LoginAsync(string login, string password)
        {
            using var response = await Client.PostAsJsonAsync(
                "api/auth/login",
                new { login, password });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            Student? student = await response.Content.ReadFromJsonAsync<Student>();

            if (student is null || string.IsNullOrWhiteSpace(student.StudentId))
            {
                throw new JsonException("The login response has no Student ID.");
            }

            return student;
        }
    }
}