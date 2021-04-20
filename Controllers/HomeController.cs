using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Reach.Models;

namespace Reach.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }
        // (((((((((((((((((((((((((((((((((((((((((Home Page)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("")]

        public IActionResult Index()
    
        {
            ViewBag.Org= _context.Organizations;
            int? userid = HttpContext.Session.GetInt32("UserId");
            ViewBag.CurrentUser = _context.Users.FirstOrDefault(u => u.UserId == userid);
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Search)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Search")]
        public IActionResult Search(int OrgId, int ZipCode){
            ViewBag.Org= _context.Organizations;

            List<Church> AllChurches = _context.Churches
            .Where(chu =>chu.ZipCode <= ZipCode + 150 || chu.ZipCode>= ZipCode  - 150)
            // .Where(Org =>Org.OrganizationId == OrgId)
            .ToList();
            List<Church> Sort = new List<Church>();
            foreach (Church item in AllChurches)
            {
                if(item.OrganizationId == OrgId){
                Sort.Add(item);
                }
               Console.WriteLine(item.ChurchName); 
            }
            ViewBag.Sort= Sort;
            return View("Results");
        }
        // (((((((((((((((((((((((((((((((((((((((((Results)))))))))))))))))))))))))))))))))))))))))//
          [HttpGet("Results")]
        public IActionResult Results(int OrgId, int ZipCode){

       ViewBag.Org= _context.Organizations;

            List<Church> AllChurches = _context.Churches
            .Where(chu =>chu.ZipCode <= ZipCode + 150 || chu.ZipCode>= ZipCode  - 150)
            // .Where(Org =>Org.OrganizationId == OrgId)
            .ToList();
            List<Church> Sort = new List<Church>();
            foreach (Church item in AllChurches)
            {
                if(item.OrganizationId == OrgId){
                Sort.Add(item);
                }
            Console.WriteLine(item.ChurchName); 
            }
            ViewBag.Sort= Sort;
            return View("Results");
        }

        // (((((((((((((((((((((((((((((((((((((((((New Church)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("NewChurch")]

        public IActionResult NewChurch()
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Org = _context.Organizations;
             ViewBag.CurrentUser = _context.Users.FirstOrDefault(u => u.UserId == userid);
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Privacy)))))))))))))))))))))))))))))))))))))))))//
        public IActionResult Privacy()
        {
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Create New Church)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Church/New")]
        public IActionResult CreateChurch(Church NewChurch)
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            // %%%%%%% Checks the date is in Future%%%%%%%%
            if (ModelState.IsValid)
            {
                NewChurch.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Add(NewChurch);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewChurch");
        }


        // (((((((((((((((((((((((((((((((((((((((((Dashboard)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Dashboard")]
        public ActionResult Dashboard()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);

            List<Church> AllChurches = _context.Churches.Include(Org =>Org.Owner).Where(x => x.Accept != false)
            .ToList();
             ViewBag.CurrentUser = _context.Users.FirstOrDefault(u => u.UserId == userid);
            return View(AllChurches);
        }
        // (((((((((((((((((((((((((((((((((((((((((Displays Login Page)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Login")]
        public ActionResult Login()
        {
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Creates New User )))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("New/User")]
        public IActionResult NewUser(User newUser)
        {

            // %%%%%%%%%%%%%%Checks the email for duplicates
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                // %%%%%  error message%%%%%%%%%%%%%%%%%%%%%%%%%%
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Login");
                // You may consider returning to the View at this point
            }
            //%%%%%%%%%%%%%%%%%%%%Check For Validation%%%%%%%%%%%%%%%%%%%%%%%%%%
            if (ModelState.IsValid)
            {
                // %%%%%%%%%%%%%%%%%%%%%%%%%Hashes the password%%%%%%%%%%%%%%%%%%%%%%%%% 
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                //Save your user object to the database
                // %%%%%%%%%%%%% Adds the user to Database%%%%%%%%%%%%%%%%%
                _context.Add(newUser);

                _context.SaveChanges();
                // %%%%%%%%%%%%%%%Save user ID To Session%%%%%%%%%%%%%%%%%%%%%%%%%%%
                // %%%%%%%%%%%%%%%% declares a key is in green for the userid%%%%%%%
                HttpContext.Session.SetInt32("UserId", newUser.UserId);

                // %%%%%%%%%%%Redirect to a Dashboard%%%%%%%%%%%%%%%%%%%%%%%%
                return RedirectToAction("Dashboard");
            }
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Validations must be Triggered will return View%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            return View("Login");
        }

        // (((((((((((((((((((((((((((((((((((((((((Login To Account)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            // %%%%%%%%% Look into DashBoard and make sure Email is in DB 
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.LoginEmail);
            if (foundUser == null)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Login");
            }
            // %%%%%%%%%%%%%%% Makes sure Password match in database
            var hasher = new PasswordHasher<LoginUser>();

            var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Login");
            }
            // Set the key in session 
            HttpContext.Session.SetInt32("UserId", foundUser.UserId);
            return RedirectToAction("Dashboard");

        }
        // (((((((((((((((((((((((((((((((((((((((((AdminDash)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("AdminLogin")]
        public ActionResult AdminLogin()
        {
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((AdminDash)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("AdminDash")]
        public ActionResult AdminDash()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? adminid = HttpContext.Session.GetInt32("AdminId");
            if (adminid == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentAdmin = _context.Admins.First(u => u.AdminId == adminid);
             List<Church> AllChurches = _context.Churches.Include(Org =>Org.Owner).ToList();

            return View(AllChurches);
        }
        // (((((((((((((((((((((((((((((((((((((((((Add To Dashboard)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Add/{ChurchId}")]
        public IActionResult AddToList(int ChurchId)
        {
            Church UpdateChurch = _context.Churches
            .FirstOrDefault(Chu => Chu.ChurchId == ChurchId);
            UpdateChurch.Accept = true;
            UpdateChurch.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("AdminDash");
        }
        // (((((((((((((((((((((((((((((((((((((((((Remove from Dashboard)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Remove/{ChurchId}")]
        public IActionResult RemoveFromList(int ChurchId)
        {
            Church UpdateChurch = _context.Churches
            .FirstOrDefault(Chu => Chu.ChurchId == ChurchId);
            UpdateChurch.Accept = false;
            UpdateChurch.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("AdminDash");
        }

        // (((((((((((((((((((((((((((((((((((((((((Delete From DataBase)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Delete/{ChurchId}")]
        public IActionResult DeleteChurch(int ChurchId)
        {

            Church DeleteChurch = _context.Churches
           .FirstOrDefault(Chu => Chu.ChurchId == ChurchId);
            _context.Remove(DeleteChurch);
            _context.SaveChanges();
            return RedirectToAction("AdminDash");
        }
        // (((((((((((((((((((((((((((((((((((((((((ChurchInfo)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("ChurchInfo/{ChurchId}")]
        public ActionResult ChurchInfo(int ChurchId)
        {
            ViewBag.AllChu = _context.Churches
           .First(Chu => Chu.ChurchId == ChurchId);
            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Create Admin Page)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("CreateAdmin")]
        public ActionResult CreateAdmin()
        {
            int? adminid = HttpContext.Session.GetInt32("AdminId");
            if (adminid == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentAdmin = _context.Admins.First(u => u.AdminId == adminid);

            return View();
        }

        // (((((((((((((((((((((((((((((((((((((((((CreatE New Admin)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("New/Admin")]
        public IActionResult NewAdmin(Admin NewAdmin)
        {

            // %%%%%%%%%%%%%%Checks the email for duplicates
            if (_context.Users.Any(u => u.Name == NewAdmin.Name))
            {
                // %%%%%  error message%%%%%%%%%%%%%%%%%%%%%%%%%%
                ModelState.AddModelError("Name", "UserName Not Avaliable!");
                return View("CreateAdmin");
                // You may consider returning to the View at this point
            }
            //%%%%%%%%%%%%%%%%%%%%Check For Validation%%%%%%%%%%%%%%%%%%%%%%%%%%
            if (ModelState.IsValid)
            {
                // %%%%%%%%%%%%%%%%%%%%%%%%%Hashes the password%%%%%%%%%%%%%%%%%%%%%%%%% 
                PasswordHasher<Admin> Hasher = new PasswordHasher<Admin>();
                NewAdmin.Password = Hasher.HashPassword(NewAdmin, NewAdmin.Password);
                //Save your user object to the database
                // %%%%%%%%%%%%% Adds the user to Database%%%%%%%%%%%%%%%%%
                _context.Add(NewAdmin);

                _context.SaveChanges();
                // %%%%%%%%%%%%%%%Save user ID To Session%%%%%%%%%%%%%%%%%%%%%%%%%%%
                // %%%%%%%%%%%%%%%% declares a key is in green for the userid%%%%%%%
                HttpContext.Session.SetInt32("AdminId", NewAdmin.AdminId);

                // %%%%%%%%%%%Redirect to a Dashboard%%%%%%%%%%%%%%%%%%%%%%%%
                return RedirectToAction("AdminDash");
            }
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Validations must be Triggered will return View%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            return View("CreateAdmin");
        }
        // (((((((((((((((((((((((((((((((((((((((((Log in Admin)))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("AdminLogin")]
        public IActionResult AdminLogin(AdminLogin AdminToLogin)
        {
            // %%%%%%%%% Look into DashBoard and make sure Email is in DB 
            var AdminFound = _context.Admins.FirstOrDefault(u => u.Name == AdminToLogin.LoginName);
            if (AdminToLogin == null)
            {
                ModelState.AddModelError("LoginName", "Please check your UserName and password");
                return View("AdminLogin");
            }
            // %%%%%%%%%%%%%%% Makes sure Password match in database
            var hasher = new PasswordHasher<AdminLogin>();

            var result = hasher.VerifyHashedPassword(AdminToLogin, AdminFound.Password, AdminToLogin.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginPassword", "Please check your email and password");
                return View("AdminLogin");
            }
            // Set the key in session 
            HttpContext.Session.SetInt32("AdminId", AdminFound.AdminId);
            return RedirectToAction("AdminDash");

        }
        // (((((((((((((((((((((((((((((((((((((((((Last thing)))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Final")]
        public ActionResult Final()
        {
            int? adminid = HttpContext.Session.GetInt32("AdminId");
            if (adminid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentAdmin = _context.Admins.First(u => u.AdminId == adminid);

            return View();
        }
        // (((((((((((((((((((((((((((((((((((((((((Create Organization)))))))))))))))))))))))))))) 
        [HttpPost("Org/New")]
        public IActionResult NewOrg(Organization NewOrg)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(NewOrg);
                _context.SaveChanges();
                return RedirectToAction("AdminDash");
            }
            return View("Final");
        }

        // (((((((((((((((((((((((((((((((((((((((((Combine Organization with Church)))))))))))))))))))))))))))))))))))))))))//
        // [HttpPost("Attach")]
        // public IActionResult Attach(Denomination NewDenom)
        // {
        //     _context.Add(NewDenom);
        //     Console.WriteLine(NewDenom.ChurchId);
        //     Console.WriteLine(NewDenom.OrganizationId);
        //     _context.SaveChanges();
        //     return RedirectToAction("Dashboard");
        // }
        // (((((((((((((((((((((((((((((((((((((((((Logout)))))))))))))))))))))))))))))))))))))))))//
 [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        // (((((((((((((((((((((((((((((((((((((((((Last thing)))))))))))))))))))))))))))))))))))))))))//
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // (((((((((((((((((((((((((((((((((((((((((Lastthing)))))))))))))))))))))))))))))))))))))))))//
    }
}
