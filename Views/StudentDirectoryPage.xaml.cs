using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using StudentInformationSystemMobile.Models;
using StudentInformationSystemMobile.Services;

namespace StudentInformationSystemMobile.Views
{
    public partial class StudentDirectoryPage : ContentPage
    {
        private readonly DatabaseService databaseService = new();
        private readonly ObservableCollection<Student> students = new();
        private readonly ObservableCollection<Student> filteredStudents = new();

        private bool isLoading;
        private string searchText = string.Empty;

        public StudentDirectoryPage()
        {
            InitializeComponent();
            StudentsCollectionView.ItemsSource = filteredStudents;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (isLoading)
            {
                return;
            }

            Student? signedInStudent = StudentSession.Current;

            if (signedInStudent is null ||
                string.IsNullOrWhiteSpace(signedInStudent.DirectoryToken))
            {
                students.Clear();
                ApplySearch();

                await DisplayAlertAsync(
                    "Sign In Required",
                    "Sign in again to open the Student Directory.",
                    "OK");
                return;
            }

            isLoading = true;

            try
            {
                var enrolledStudents = await databaseService.GetStudentsAsync(
                    signedInStudent.DirectoryToken);

                if (!ReferenceEquals(StudentSession.Current, signedInStudent))
                {
                    return;
                }

                students.Clear();

                foreach (Student student in enrolledStudents)
                {
                    students.Add(student);
                }

                ApplySearch();
            }
            catch (HttpRequestException ex) when (
                ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                students.Clear();
                ApplySearch();

                await DisplayAlertAsync(
                    "Session Expired",
                    "Log out and sign in again to refresh the directory.",
                    "OK");
            }
            catch (Exception ex) when (
                ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                await DisplayAlertAsync(
                    "Could not refresh directory",
                    "Check that the API is running, then reopen Student Directory.",
                    "OK");
            }
            finally
            {
                isLoading = false;
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = e.NewTextValue?.Trim() ?? string.Empty;
            ApplySearch();
        }

        private void ApplySearch()
        {
            filteredStudents.Clear();

            foreach (Student student in students)
            {
                if (student.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    student.StudentId.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    filteredStudents.Add(student);
                }
            }
        }

        private void OnStudentSelected(object sender, SelectionChangedEventArgs e)
        {
            StudentsCollectionView.SelectedItem = null;
        }
    }
}