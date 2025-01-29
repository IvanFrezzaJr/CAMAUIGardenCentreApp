using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using BCrypt.Net;



namespace CAMAUIGardenCentreApp;

public class PasswordHasher
{
    // Gera um hash seguro para a senha
    public static string HashPassword(string password)
    {
        // Gera um hash com um salt automático
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    // Verifica se a senha está correta
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
