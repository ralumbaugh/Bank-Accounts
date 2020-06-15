using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required (ErrorMessage="A first name is required")]
        [MinLength (2, ErrorMessage="First name must be at least two characters")]
        public string FirstName {get; set;}
        [Required (ErrorMessage="A last name is required")]
        [MinLength (2, ErrorMessage="Last name must be at least two characters")]
        public string LastName {get; set;}
        [Required (ErrorMessage="An email is required")]
        [EmailAddress (ErrorMessage="Please enter a valid email address")]
        public string Email {get; set;}
        [DataType(DataType.Password)]
        [Required (ErrorMessage="A password is required")]
        [MinLength (8, ErrorMessage="Password must be at least 8 characters long")]
        public string Password {get; set;}
        public List<Transaction> TransactionHistory {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get; set;}
    }
}