using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using StudentInformationSystemMobile.Models;
using StudentInformationSystemMobile.Services;

namespace StudentInformationSystemMobile.Views
{
    public partial class StudentLogin : ContentPage
    {
        private readonly DatabaseService databaseService = new();
        private bool isSigningIn;

        public StudentLogin()
        {
            InitializeComponent();
        }

        private async void OnSupportTapped(object sender, TappedEventArgs e)
        {
            await DisplayAlertAsync(
                "Support",
                "Please contact the system administrator for assistance.",
                "OK");
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (isSigningIn)
            {
                return;
            }

            string login = UsernameEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlertAsync(
                    "Login Required",
                    "Enter your email or Student ID and password.",
                    "OK");
                return;
            }

            isSigningIn = true;
            StudentSession.Current = null;

            try
            {
                Student? student = await databaseService.LoginAsync(login, password);

                if (student is null)
                {
                    await DisplayAlertAsync(
                        "Login Failed",
                        "The email, Student ID, or password is incorrect.",
                        "OK");
                    return;
                }

                StudentSession.Current = student;
                PasswordEntry.Text = string.Empty;
                await Shell.Current.GoToAsync(nameof(StudentDashboard));
            }
            catch (HttpRequestException ex)
                when (ex.StatusCode == HttpStatusCode.NotFound ||
                      ex.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                await DisplayAlertAsync(
                    "Login API Unavailable",
                    "Restart StudentDirectoryApi with the POST /api/auth/login endpoint.",
                    "OK");
            }
            catch (Exception ex) when (
                ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await DisplayAlertAsync(
                    "Connection Error",
                    "Check the PowerShell window running the API, then try again.",
                    "OK");
            }
            finally
            {
                isSigningIn = false;
            }
        }
    }
}