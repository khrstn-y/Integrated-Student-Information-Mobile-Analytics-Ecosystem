using System;
using Microsoft.Maui.Controls;
using StudentInformationSystemMobile.Models;
using StudentInformationSystemMobile.Services;

namespace StudentInformationSystemMobile.Views
{
    public partial class MyProfilePage : ContentPage
    {
        private readonly Label? programCaptionLabel;

        public MyProfilePage()
        {
            InitializeComponent();

            programCaptionLabel = this.FindByName<Label>("ProgramCaptionLabel");

            FirstNameEntry.IsReadOnly = true;
            MiddleNameEntry.IsReadOnly = true;
            LastNameEntry.IsReadOnly = true;
            ContactEntry.IsReadOnly = true;
            EmailEntry.IsReadOnly = true;
            DobEntry.IsReadOnly = true;
            GenderEntry.IsReadOnly = true;

            EditButton.IsEnabled = false;
            SaveButton.IsEnabled = false;

            ShowProfile(StudentSession.Current);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowProfile(StudentSession.Current);
        }

        private void ShowProfile(Student? student)
        {
            FullNameHeaderLabel.Text = student?.FullName ?? "Please sign in";
            StudentIdLabel.Text = student is null
                ? string.Empty
                : $"Student ID: {student.StudentId}";

            FirstNameEntry.Text = student?.FirstName ?? string.Empty;
            MiddleNameEntry.Text = student?.MiddleName ?? string.Empty;
            LastNameEntry.Text = student?.LastName ?? string.Empty;
            ContactEntry.Text = student?.ContactNumber ?? string.Empty;
            EmailEntry.Text = student?.Email ?? string.Empty;
            DobEntry.Text = student?.DateOfBirth ?? string.Empty;
            GenderEntry.Text = student?.Gender ?? string.Empty;

            string program = student?.Program ?? string.Empty;
            string yearLevel = student?.YearLevel ?? string.Empty;
            string block = student?.Block ?? string.Empty;
            string combined = student?.YearBlockProgram ?? string.Empty;

            bool showCombined = !string.IsNullOrWhiteSpace(combined) &&
                (string.IsNullOrWhiteSpace(program) ||
                 string.IsNullOrWhiteSpace(yearLevel) ||
                 string.IsNullOrWhiteSpace(block));

            if (programCaptionLabel is not null)
            {
                programCaptionLabel.Text = showCombined
                    ? "Year / Block / Program"
                    : "Program";
            }

            ProgramLabel.Text = DisplayValue(showCombined ? combined : program);
            YearLevelLabel.Text = DisplayValue(yearLevel);
            BlockLabel.Text = DisplayValue(block);
            StatusLabel.Text = DisplayValue(student?.EnrollmentStatus);
        }

        private static string DisplayValue(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? "—" : value;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await DisplayAlertAsync(
                "Read-only profile",
                "Profile editing isn’t connected to the database yet.",
                "OK");
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            await DisplayAlertAsync(
                "Saving unavailable",
                "Profile saving isn’t connected to the database yet.",
                "OK");
        }
    }
}