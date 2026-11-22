using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using StudentInformationSystemMobile.Models;
using StudentInformationSystemMobile.Services;

namespace StudentInformationSystemMobile.Views
{
    public partial class StudentDashboard : ContentPage
    {
        private readonly VisualElement? statusBadge;

        public StudentDashboard()
        {
            InitializeComponent();
            statusBadge = this.FindByName<VisualElement>("StatusBadge");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Student? student = StudentSession.Current;

            if (student is null)
            {
                WelcomeNameLabel.Text = "Please sign in";
                StudentSummaryLabel.Text = string.Empty;
                SetEnrollmentStatus(string.Empty);
                return;
            }

            WelcomeNameLabel.Text = student.FullName;
            StudentSummaryLabel.Text = string.IsNullOrWhiteSpace(student.AcademicSummary)
                ? student.StudentId
                : $"{student.StudentId} • {student.AcademicSummary}";

            SetEnrollmentStatus(student.EnrollmentStatus);
        }

        private void SetEnrollmentStatus(string enrollmentStatus)
        {
            StatusBadgeLabel.Text = enrollmentStatus;

            if (statusBadge is not null)
            {
                statusBadge.IsVisible = !string.IsNullOrWhiteSpace(enrollmentStatus);
            }
        }

        private async void OnStudentDirectoryClicked(object sender, TappedEventArgs e)
        {
            await OpenStudentPageAsync(nameof(StudentDirectoryPage));
        }

        private async void OnMyProfileClicked(object sender, TappedEventArgs e)
        {
            await OpenStudentPageAsync(nameof(MyProfilePage));
        }

        private async Task OpenStudentPageAsync(string route)
        {
            if (StudentSession.Current is null)
            {
                await DisplayAlertAsync(
                    "Sign In Required",
                    "Return to the login screen and sign in with a registered student account.",
                    "OK");
                return;
            }

            await Shell.Current.GoToAsync(route);
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlertAsync(
                "Logout",
                "Are you sure you want to log out?",
                "Yes",
                "Cancel");

            if (confirm)
            {
                StudentSession.Current = null;
                await Shell.Current.GoToAsync("//WelcomePage");
            }
        }
    }
}