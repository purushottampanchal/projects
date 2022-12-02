
using Microsoft.AspNetCore.Identity;

PasswordHasher<string> pw = new PasswordHasher<string>();

var user1 = "user 1";
var password1 = "samplePassword";
var hash1 = pw.HashPassword(user1, password1);
var providedPassword1 = "samplePassword";

string user2, password2, hash2, providedPassword2;

GenerateUSer2(pw, out user2, out password2, out hash2, out providedPassword2);

Console.WriteLine($"1. password '{password1}' for user '{user1}' hash value -- {hash1}");
Console.WriteLine($"2. password '{password2}' for user '{user2}' hash value -- {hash2}");

Console.WriteLine();
Console.WriteLine();

Console.WriteLine($"1.Varify password for {user1} with  {pw.VerifyHashedPassword(user1, hash1, providedPassword1)}");
Console.WriteLine($"1.Varify password for {user2} with  {pw.VerifyHashedPassword(user2, hash2, providedPassword2)}");

static void GenerateUSer2(PasswordHasher<string> pw, out string user2, out string password2, out string hash2, out string providedPassword2)
{
    user2 = "user 2";
    password2 = "samplePassword";
    hash2 = pw.HashPassword(user2, password2);
    providedPassword2 = "samplePassword";
}

//Console.WriteLine($"2.Varify password for {user1} with hash 2 {pw.VerifyHashedPassword(user1, hash, providedPassword1)}");
