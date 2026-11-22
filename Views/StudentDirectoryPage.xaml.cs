using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
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

            if (isLoading || students.Count > 0)
            {
                return;
            }

            isLoading = true;

            try
            {
                Student? student = await databaseService.GetDemoStudentAsync();

                if (student is null)
                {
                    await DisplayAlertAsync(
                        "Student Directory",
                        "The API returned no student record.",
                        "OK");
                    return;
                }

                students.Add(student);
                ApplySearch();
            }
            catch (Exception ex) when (
                ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                await DisplayAlertAsync(
                    "Could not load student",
                    "Check that the API is running in PowerShell, then reopen the Student Directory.",
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