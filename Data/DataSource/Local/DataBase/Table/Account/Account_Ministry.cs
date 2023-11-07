using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Account_Ministry
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Account_Ministry()
        {
        }

        public Account_Ministry(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}