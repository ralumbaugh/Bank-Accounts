using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class WrapperModel
    {
        public User NewUser {get; set;}
        public LoginUser LoginUser {get; set;}
    }
}