using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            int? LoggedInUserID = HttpContext.Session.GetInt32("LoggedInUserID");
            if(LoggedInUserID != null)
            {
                return RedirectToAction("LoggedIn");
            }
            return View("Index");
        }

        [HttpGet("LoggedIn")]
        public IActionResult LoggedIn()
        {
            int? LoggedInUserID = HttpContext.Session.GetInt32("LoggedInUserID");
            if(LoggedInUserID == null)
            {
                return RedirectToAction("Index");
            }
            UserWrapper MyUser = new UserWrapper();
            MyUser.CurrentUser = (User)dbContext.Users
                .FirstOrDefault(user => user.UserId == LoggedInUserID);
            List<Transaction> UserTransactions = dbContext.Transactions
                .Where(u => u.UserId == (int)LoggedInUserID)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
            MyUser.CurrentUser.TransactionHistory = UserTransactions;
            int UserBalance = 0;
            foreach(Transaction transaction in MyUser.CurrentUser.TransactionHistory)
            {
                UserBalance += (int)transaction.Amount;
            }
            MyUser.Balance = UserBalance;
            return View(MyUser);
        }

        [HttpPost("RegisterUser")]
        public IActionResult Register(WrapperModel WrappedUser)
        {
            User user = WrappedUser.NewUser;
            if(dbContext.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("NewUser.Email", "Email already in use!");
            }
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("LoggedInUserID", user.UserId);
                return RedirectToAction("LoggedIn");
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Login(WrapperModel WrappedUser)
        {
            LoginUser user = WrappedUser.LoginUser;
            if(ModelState.IsValid)
            {
                User UserInDb = dbContext.Users.FirstOrDefault(u=> u.Email == user.Email);
                if(UserInDb == null)
                {
                    ModelState.AddModelError("LoginUser.Email", "The email/password combination is incorrect.");
                    return View("Index");
                }
                PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                var result = Hasher.VerifyHashedPassword(user, UserInDb.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginUser.Email", "The email/password combination is incorrect.");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("LoggedInUserID", UserInDb.UserId);
                return RedirectToAction("LoggedIn");
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult Transaction(UserWrapper WrappedTransaction)
        {
            Transaction NewTransaction = WrappedTransaction.NewTransaction;
            int? LoggedInUserID = HttpContext.Session.GetInt32("LoggedInUserID");
            NewTransaction.UserId = (int)LoggedInUserID;
            WrappedTransaction.CurrentUser = (User)dbContext.Users.FirstOrDefault(user => user.UserId == LoggedInUserID);
            List<Transaction> UserTransactions = dbContext.Transactions
                .Where(u => u.UserId == (int)LoggedInUserID)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
            WrappedTransaction.CurrentUser.TransactionHistory = UserTransactions;
            int UserBalance = 0;
            foreach(Transaction transaction in WrappedTransaction.CurrentUser.TransactionHistory)
            {
                UserBalance += (int)transaction.Amount;
            }
            WrappedTransaction.Balance = UserBalance;
            if(UserBalance+NewTransaction.Amount < 0)
            {
                ModelState.AddModelError("NewTransaction.Amount","Insufficient Funds");
            }
            if(ModelState.IsValid)
            {
                dbContext.Add(NewTransaction);
                dbContext.SaveChanges();
                return RedirectToAction("LoggedIn");
            }
            
            return View("LoggedIn", WrappedTransaction);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
