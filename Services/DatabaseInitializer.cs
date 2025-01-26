using CAMAUIGardenCentreApp.Data;
using System.Threading.Tasks;

namespace CAMAUIGardenCentreApp.Services;

public class DatabaseInitializer
{
    private readonly DatabaseContext _context;

    public DatabaseInitializer(DatabaseContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.InitCategory(); // Init category
        await _context.InitProduct(); // Init product
    }
}
