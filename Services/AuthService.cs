using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Data;
namespace CAMAUIGardenCentreApp.Services;


public class AuthService
{

    private const string AuthStateKey = "AuthState";
    private const string LogedUserId = "LogedUserId";
    private readonly DatabaseContext _context;

    public AuthService (DatabaseContext context) {
        _context = context;
    }


    public async Task<bool> IsAuthenticatedAsync()
    {
        await Task.Delay(1000);

        var authState = Preferences.Default.Get<bool>(AuthStateKey, false);

        return authState;
    }
    public void Login()
    {
        Preferences.Default.Set<bool>(AuthStateKey, true);
    }
    public void Logout() 
    {
        Preferences.Default.Remove(AuthStateKey);
    }


    public async Task<bool> Authenticate(string name, string phone)
    {
        IEnumerable<User> users = await _context.GetFileteredAsync<User>(p => p.Name == name && p.Phone == phone);

        if (users is not null && users.Any())
        {
            User user = users.First();

            Preferences.Default.Set<int>(LogedUserId, user.Id);

            return true;


            //bool isPasswordCorrect = PasswordHasher.VerifyPassword(password, user.Password);

            //return (isPasswordCorrect) ? true : false;
        }

        return false;
        
    }


    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
    {
        return await _context.GetFileteredAsync<Product>(p => p.CategoryId == categoryId);

    }
}