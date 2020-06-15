using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage="An email address is required")]
        [EmailAddress (ErrorMessage="Please enter a valid email address")]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required (ErrorMessage="A password is required")]
        [MinLength (8, ErrorMessage="Password must be at least 8 characters!")]
        public string Password {get;set;}
    }
}