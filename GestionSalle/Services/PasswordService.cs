using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GestionSalle.Services
{
 public interface IPasswordService
 {
 string HashPassword(string password);
 bool VerifyPassword(string hashedPassword, string providedPassword);
 }
 public class PasswordService : IPasswordService
 {
 public string HashPassword(string password)
 {
 byte[] salt = RandomNumberGenerator.GetBytes(16);
 var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256,10000,32);
 var outputBytes = new byte[49];
 outputBytes[0] =0x01; // format marker
 Buffer.BlockCopy(salt,0, outputBytes,1,16);
 Buffer.BlockCopy(subkey,0, outputBytes,17,32);
 return Convert.ToBase64String(outputBytes);
 }
 public bool VerifyPassword(string hashedPassword, string providedPassword)
 {
 try
 {
 var decoded = Convert.FromBase64String(hashedPassword);
 if (decoded[0] !=0x01) return false;
 var salt = new byte[16];
 Buffer.BlockCopy(decoded,1, salt,0,16);
 var storedSubkey = new byte[32];
 Buffer.BlockCopy(decoded,17, storedSubkey,0,32);
 var generated = KeyDerivation.Pbkdf2(providedPassword, salt, KeyDerivationPrf.HMACSHA256,10000,32);
 return CryptographicOperations.FixedTimeEquals(storedSubkey, generated);
 }
 catch
 {
 return false;
 }
 }
 }
}
