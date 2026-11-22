using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private sealed record LoginResponse(Student? Student, string? DirectoryToken);

        public Task<Student?> GetDemoStudentAsync()
        {
            return Client.GetFromJsonAsync<Student>("api/students/2026-TEST-001");
        }

        public async Task<Student?> LoginAsync(string login, string password)
        {
            using var response = await Client.PostAsJsonAsync(
                "api/auth/login", new { login, password });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Student is not Student student ||
                string.IsNullOrWhiteSpace(student.StudentId) ||
                string.IsNullOrWhiteSpace(result.DirectoryToken))
            {
                throw new JsonException("The login API returned an incomplete response.");
            }

            student.DirectoryToken = result.DirectoryToken;
            return student;
        }

        public async Task<List<Student>> GetStudentsAsync(string directoryToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/students");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", directoryToken);

            using var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Student>>()
                ?? throw new JsonException("The directory API returned no list.");
        }
    }
}