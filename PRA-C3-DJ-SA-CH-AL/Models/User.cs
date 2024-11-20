using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHashed { get; set; }
        public int Credits { get; set; }

        
        // Method to hash the password
        public void SetPassword(string password)
        {
            // Hash the password using bcrypt
            PasswordHashed = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method to verify the password
        public bool VerifyPassword(string password)
        {
            // Verify the password against the stored hash
            return BCrypt.Net.BCrypt.Verify(password, PasswordHashed);
        }
    }
}
