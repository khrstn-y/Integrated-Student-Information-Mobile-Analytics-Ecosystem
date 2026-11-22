using StudentInformationSystemMobile.Models;

namespace StudentInformationSystemMobile.Services
{
    internal static class StudentSession
    {
        public static Student? Current { get; set; }
    }
}