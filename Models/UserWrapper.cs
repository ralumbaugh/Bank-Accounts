using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class UserWrapper
    {
        public User CurrentUser {get; set;}
        public Transaction NewTransaction {get; set;}
        public int Balance {get; set;}
    }
}