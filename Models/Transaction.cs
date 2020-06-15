using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get; set;}
        [Required (ErrorMessage="Please input a transaction amount")]
        public int? Amount {get; set;}
        public int UserId {get; set;}
        public User TransactionUser {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}