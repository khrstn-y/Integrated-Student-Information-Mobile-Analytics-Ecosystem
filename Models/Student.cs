namespace StudentInformationSystemMobile.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string YearLevel { get; set; } = string.Empty;
        public string Block { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public string YearBlockProgram { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string EnrollmentStatus { get; set; } = string.Empty;
        public string DirectoryToken { get; set; } = string.Empty;

        public string FullName =>
            $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();

        public string AcademicSummary =>
            string.IsNullOrWhiteSpace(Program) || string.IsNullOrWhiteSpace(YearLevel)
                ? YearBlockProgram
                : $"{Program} • {YearLevel}";
    }
}