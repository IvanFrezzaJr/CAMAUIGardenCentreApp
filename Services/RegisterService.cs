using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;


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
        if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Phone))
        {
            return false; // Validation: User object must have valid login and password
        }

        return await _dbContext.AddItemAsync(user);
    }


    public async Task<bool> AddCreditCardAsync(CreditCard creditCard)
    {
        if (creditCard == null || creditCard.CardNumber == null || string.IsNullOrWhiteSpace(creditCard.CardholderName) || string.IsNullOrWhiteSpace(creditCard.ExpirationDate) || creditCard.cvv == null)
        {
            return false; // Validation: User object must have valid login and password
        }

        return await _dbContext.AddItemAsync(creditCard);
    }


    public async Task<bool> AddAccountAsync(Account account)
    {
        if (account == null || string.IsNullOrWhiteSpace(account.CompanyName) ||  string.IsNullOrWhiteSpace(account.BillingEmail))
        {
            return false; // Validation: User object must have valid login and password
        }

        return await _dbContext.AddItemAsync(account);
    }


    public async Task<IEnumerable<User>> GetUserByPhoneAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return null; // Validation: Login must not be empty
        }

        return await _dbContext.GetFileteredAsync<User>(u => u.Phone == phone);
    }


    public async Task<IEnumerable<User>> GetUserByIdAsync(int id)
    {
        if (id == null)
        {
            return null; // Validation: Login must not be empty
        }

        return await _dbContext.GetFileteredAsync<User>(u => u.Id == id);
    }


    public async Task<IEnumerable<Account>> GetUserAccountAsync(int id)
    {
        if (id == null)
        {
            return null; // Validation: Login must not be empty
        }

        return await _dbContext.GetFileteredAsync<Account>(u => u.Id == id);

    }

}


