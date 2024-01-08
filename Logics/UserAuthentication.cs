using Coursework1Development.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coursework1Development.Logics
{
    internal class UserAuthentication
    {

        public static SortedDictionary<String, String> Login_user(String username, String password)
        {

           SortedDictionary<String, String> loginResponses= [];

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                loginResponses.Add("credentialsMatches", "no");
                loginResponses.Add("message", "Fields cannot be empty");
                return loginResponses;
            }

           String userDataFilePath = FilePaths.UserData();
           String userData = File.ReadAllText(userDataFilePath);
           UserList userList= JsonSerializer.Deserialize<UserList>(userData);

           User user = userList.users.FirstOrDefault(user => user.name == username && user.password==password);

            if (user == null)
            {
                loginResponses.Add("credentialsMatches", "no");
                loginResponses.Add("message", "Invalid credentials");
                return loginResponses;
            }

            loginResponses.Add("credentialsMatches", "yes");
            loginResponses.Add("message", "Login Successful");
            loginResponses.Add("userRole", user.role);

            return loginResponses;

        }
    }
}
