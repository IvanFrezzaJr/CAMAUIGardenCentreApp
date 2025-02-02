using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CAMAUIGardenCentreApp.Models;
using System.Diagnostics;


namespace CAMAUIGardenCentreApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly RegisterService _registerService;
        private readonly AuthService _authService;

        public RegisterViewModel(RegisterService registerService, AuthService authService)
        {
            _registerService = registerService;
            _authService = authService;
            IsPersonal = true;
            IsCorporate = false;
        }

        [ObservableProperty]
        private bool isCorporate = false;

        [ObservableProperty]
        private bool isPersonal = true;

        partial void OnIsCorporateChanged(bool value)
        {
            if (value) IsPersonal = false;
        }

        partial void OnIsPersonalChanged(bool value)
        {
            if (value) IsCorporate = false;
        }

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        // personal fields
        [ObservableProperty]
        private string cardholderName;

        [ObservableProperty]
        private long? cardNumber;

        [ObservableProperty]
        private string expirationDate;

        partial void OnExpirationDateChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // Remove any non-numeric characters
                var cleanValue = new string(value.Where(char.IsDigit).ToArray());

                // Ensure the string is no longer than 4 characters
                if (cleanValue.Length > 4)
                {
                    cleanValue = cleanValue.Substring(0, 4);
                }

                // Format it to MM/YY (if possible)
                if (cleanValue.Length >= 2)
                {
                    cleanValue = cleanValue.Insert(2, "/"); // Insert slash between MM and YY
                }

                ExpirationDate = cleanValue;
            }
        }


        [ObservableProperty]
        private int? cvv;

        // corporate fields
        [ObservableProperty]
        private string companyName;

        [ObservableProperty]
        private string companyTaxID;

        [ObservableProperty]
        private string billingEmail;

        private List<string> _validationErrors = new List<string>();

        private void AddValidationError(string message)
        {
            if (!_validationErrors.Contains(message))
            {
                _validationErrors.Add(message);
            }
        }

        private void ClearValidationErrors()
        {
            _validationErrors.Clear();
        }

        private bool ValidatePersonalFields()
        {
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(cardholderName)) { 
                AddValidationError("Cardholder name is required.");
            }

            if (CardNumber.ToString().Length < 13 || CardNumber.ToString().Length > 19)
            {
                Debug.WriteLine($"---------------------------------- {CardNumber}");
                AddValidationError("Card number must be between 13 and 19 digits.");
            }
            else if (CardNumber < 0)
            {
                AddValidationError("Card number must not be negative.");
            }

            if (ExpirationDate != null)
            {
                if (!Regex.IsMatch(ExpirationDate, @"^(0[1-9]|1[0-2])\/\d{2}$")) { 
                    AddValidationError("Expiration date must be in MM/YY format.");
                }

                if (DateTime.TryParseExact(ExpirationDate, "MM/yy", null, System.Globalization.DateTimeStyles.None, out DateTime expiration) && expiration < DateTime.Now) { 
                    AddValidationError("Expiration date cannot be in the past.");
                }
            } else
            {
                AddValidationError("Expiration date must be informed.");
            }


            if (Cvv.ToString().Length != 3)
            {
                Debug.WriteLine($"---------------------------------- {CardNumber}");
                AddValidationError("CVV must be 3 digits.");
            }

            return _validationErrors.Count == 0;
        }

        private bool ValidateCorporateFields()
        {
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(CompanyName))
                AddValidationError("Company name is required.");

            if (string.IsNullOrWhiteSpace(CompanyTaxID))
                AddValidationError("Company Tax ID is required.");

            if (string.IsNullOrWhiteSpace(BillingEmail) || !Regex.IsMatch(BillingEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddValidationError("Billing email is required and must be a valid email address.");

            return _validationErrors.Count == 0;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            ClearValidationErrors(); // Clear previous errors

            // Validate the login and password
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                AddValidationError("Login and Password are required.");
            }

            // Check if the user already exists
            var users = await _registerService.GetUserByLoginAsync(Login);
            if (users is not null && users.Any())
            {
                AddValidationError("User already exists.");
            }

            // Validate personal fields
            if (IsPersonal)
            {
                if (!ValidatePersonalFields())
                {
                    string personalErrors = string.Join("\n", _validationErrors);
                    await Application.Current.MainPage.DisplayAlert("Validation Errors", personalErrors, "OK");
                    return;
                }
            }

            // Validate corporate fields
            if (IsCorporate)
            {
                if (!ValidateCorporateFields())
                {
                    string corporateErrors = string.Join("\n", _validationErrors);
                    await Application.Current.MainPage.DisplayAlert("Validation Errors", corporateErrors, "OK");
                    return;
                }
            }

            // If there are validation errors, stop the registration process
            if (_validationErrors.Any())
            {
                string allErrors = string.Join("\n", _validationErrors);
                await Application.Current.MainPage.DisplayAlert("Validation Errors", allErrors, "OK");
                return;
            }

            // Proceed to create the user only if all validations pass
            string passwordHash = PasswordHasher.HashPassword(Password);

            User user = new User
            {
                Login = Login,
                Password = passwordHash,
                Type = IsPersonal ? "personal" : "corporate"
            };

            bool status = await _registerService.AddUserAsync(user);




            CreditCard creditCard = new CreditCard
            {
                UserId = user.Id,   
                CardholderName = cardholderName,
                CardNumber = (long)cardNumber,
                cvv = (int)Cvv,
                ExpirationDate = expirationDate,
            };

            status = await _registerService.AddCreditCardAsync(user.Id, creditCard);

            if (status)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful!", "OK");

                if (await _authService.Authenticate(Login, Password))
                {
                    _authService.Login();
                    // Navigate to the main page
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    // Show error message if authentication fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid login or password", "OK");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Registration failed.", "OK");
            }
        }

    }
}
