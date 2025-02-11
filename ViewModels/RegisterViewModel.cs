using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CAMAUIGardenCentreApp.Services;
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
        private string name;

        [ObservableProperty]
        private string phone;

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
        private string billingEmail;

        [ObservableProperty]
        private int billingDay = 1;

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
                AddValidationError("CVV must be 3 digits.");
            }

            return _validationErrors.Count == 0;
        }

        private bool ValidateCorporateFields()
        {
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(CompanyName)) { 
                AddValidationError("Company name is required.");
            }

            if (string.IsNullOrWhiteSpace(BillingEmail) || !Regex.IsMatch(BillingEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { 
                AddValidationError("Billing email is required and must be a valid email address.");
            }

            return _validationErrors.Count == 0;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            ClearValidationErrors(); // Clear previous errors

            // Validate the name and phone
            if (string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Name))
            {
                AddValidationError("Phone and Password are required.");
            }

            // Check if the user already exists
            var users = await _registerService.GetUserByPhoneAsync(Phone);
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

            User user = new User
            {
                Name = Name,
                Phone = Phone,
                Type = IsPersonal ? "personal" : "corporate"
            };

            bool status = await _registerService.AddUserAsync(user);

            if (IsPersonal)
            {
                CreditCard creditCard = new CreditCard
                {
                    UserId = user.Id,
                    CardholderName = cardholderName,
                    CardNumber = (long)cardNumber,
                    cvv = (int)Cvv,
                    ExpirationDate = expirationDate,
                };

                status = await _registerService.AddCreditCardAsync(creditCard);
            }


            if (IsCorporate)
            {
                Account account = new Account
                {
                    UserId = user.Id,
                    CompanyName = companyName,
                    BillingEmail = billingEmail,
                    BillingDay = billingDay,
                };


                status = await _registerService.AddAccountAsync(account);

            }


                if (status)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful!", "OK");

                if (await _authService.Authenticate(Name, Phone))
                {
                    _authService.Login();
                    // Navigate to the main page
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    // Show error message if authentication fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid name or phone", "OK");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Registration failed.", "OK");
            }
        }

    }
}
