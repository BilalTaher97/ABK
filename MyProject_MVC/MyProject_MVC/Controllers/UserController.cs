
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyProject_MVC.Models;
using System.Net.Mail;

namespace MyProject_MVC.Controllers
{
    public class UserController : Controller
    {

        private readonly MyDbContext _context;


        public UserController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Log_Reg()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelReg(string username, string email, string password)
        {

            if (ModelState.IsValid)
            {
                var user = new User
                {

                    Username = username,
                    Email = email,
                    Password = password

                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }


            return RedirectToAction("Log_Reg");
        }


        [HttpPost]
        public IActionResult HandelLogin(string email, string password)
        {

            if (ModelState.IsValid)
            {
                foreach (var user in _context.Users)
                {
                    if (user.Email == email && user.Password == password)
                    {
                        return RedirectToAction(nameof(ResetPassword));
                    }

                }

            }
            ViewData["Message"] = "Invalid email or password!";
            return View("Log_Reg");
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelReset(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {

                ViewData["Message_1"] = "Email not found!";
                return View(nameof(ResetPassword));


            }


            Random rand = new Random();

            int verificationCode = rand.Next(100000, 999999);


            HttpContext.Session.SetString("ResetCode", verificationCode.ToString());
            HttpContext.Session.SetString("ResetEmail", email);


            SendEmail(email, "Password Reset Code",$"Your reset code is: {verificationCode}");
            ViewData["Message_1"] = "A verification code has been sent to your email!";
            Console.WriteLine(email);

            return View(nameof(VerifyCode));

        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult ChangePssword()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelCode(string code)
        {
            var MyCode = HttpContext.Session.GetString("ResetCode");


            if(MyCode == null || MyCode != code)
            {
                ViewData["Message_2"] = "Invalid verification code!";
                return View("VerifyCode");
            }


            return View();
        }


        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@gmail.com", "qryb wloa fphl xdbb"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(toEmail);
                client.Send(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }


        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
