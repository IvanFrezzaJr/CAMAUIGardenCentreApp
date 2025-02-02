using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CAMAUIGardenCentreApp.Services;


public class RegisterService
{
    private readonly DatabaseContext _dbContext;

    public RegisterService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
        {
            return false; // Validation: User object must have valid login and password
        }

        return await _dbContext.AddItemAsync(user);
    }


    public async Task<bool> AddCreditCardAsync(int userId, CreditCard creditCard)
    {
        if (creditCard == null || creditCard.CardNumber == null || string.IsNullOrWhiteSpace(creditCard.CardholderName) || string.IsNullOrWhiteSpace(creditCard.ExpirationDate) || creditCard.cvv == null)
        {
            return false; // Validation: User object must have valid login and password
        }

        return await _dbContext.AddItemAsync(creditCard);
    }


    //public async Task<bool> AddAccountAsync(User user)
    //{
    //    if (user == null || string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
    //    {
    //        return false; // Validation: User object must have valid login and password
    //    }

    //    return await _dbContext.AddItemAsync(user);
    //}


    public async Task<IEnumerable<User>> GetUserByLoginAsync(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return null; // Validation: Login must not be empty
        }

        return await _dbContext.GetFileteredAsync<User>(u => u.Login == login);
    }
}


