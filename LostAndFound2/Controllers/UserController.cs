using LostAndFound2.Data;
using LostAndFound2.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;

namespace LostAndFound2.Controllers
{
    public class UserController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork(DBContext.Instance);
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Name == form["Name"].ToString()).Count() == 0)
            {
                ViewData["danger"] = "No user name with this name";
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (!(_unitOfWork.UserRepository.Find(u => u.Name == form["Name"].ToString() ).First().Password == form["Password"].ToString() ))
                {
                    ViewData["danger"] = "Wrong credentials";
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    HttpContext.Session.SetString("id", _unitOfWork.UserRepository.Find(u => u.Name == form["Name"].ToString()).First().Id.ToString());
                    ViewData["success"] = "User connected successfuly";
                    return RedirectToAction("Index", "Home");
                }
            }      
        }
        [HttpGet]
        public IActionResult SignUp() { return View(); }
        [HttpPost]
        public IActionResult SignUp(IFormCollection form)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Name == (form["Name"].ToString())).FirstOrDefault() != null)
            {
                ViewData["danger"] = "UserName already exists";
                return RedirectToAction("SignUp", "User");
            }
            if (form["Password"].ToString() != form["Repeted_Password"].ToString())
            {
                ViewData["danger"] = "Password and repeted password does not match";
                return RedirectToAction("SignUp", "User");
            }
            if (form["Name"].ToString() == "" || form["Password"].ToString() == "" || form["Phone"].ToString() == "")
            {
                ViewData["danger"] = "Please fill all fields";
                return RedirectToAction("SignUp", "User");
            }
            _unitOfWork.UserRepository.Add(new Models.User(form["Name"].ToString(), form["Password"].ToString(), long.Parse(form["Phone"])));
            _unitOfWork.Complete();
            ViewData["success"] = "User created successfuly";
            return RedirectToAction("Index", "Home");          
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.SetString("id" , "");
            ViewData["success"] = "User logout successfuly";
            return RedirectToAction("LogIn", "User");
        }
    }
}
