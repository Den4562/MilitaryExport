using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Account_Command
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Account_Command()
        {
        }

        public Account_Command(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}