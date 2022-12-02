namespace JWTAuthTest.Models
{

    public class UserConstants
    {
        public static List<User> users = new List<User>()
        {
            new User() { UserName = "Trainer1", Password = "123", Role = "Trainer" },
            new User() { UserName = "admin1", Password = "123", Role = "Admin" }
        };
    }


    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
