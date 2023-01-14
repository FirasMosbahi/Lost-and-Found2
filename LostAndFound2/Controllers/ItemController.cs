using LostAndFound2.Data;
using LostAndFound2.Data.UnitOfWork;
using LostAndFound2.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LostAndFound2.Controllers
{
    public class ItemController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork(DBContext.Instance);
        public IActionResult Index(int id)
        {
            List<Item> items = _unitOfWork.ItemRepository.GetAll().ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            Debug.WriteLine("hi" + HttpContext.Session.GetString("id"));
            if(HttpContext.Session.GetString("id") == "" || HttpContext.Session.GetString("id")==null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddItem(IFormCollection form ,IFormFile formFile )
        {
            User user = _unitOfWork.UserRepository.GetById(int.Parse(HttpContext.Session.GetString("id") ?? "0"));
            try
            {
                Item item = new Item(form["Name"].ToString() , form["Description"].ToString() , form["Color"].ToString() , form["Image_Link"].ToString(), form["Category"].ToString());
                user.Items.Add(item);
                _unitOfWork.Complete();
                HttpContext.Session.SetString("success", "Item created successfuly");
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return RedirectToAction("Index", "Item");
        }
        public IActionResult FindItem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FiltredItem(IFormCollection form)
        {
            List<Item> items = _unitOfWork.ItemRepository.Find(i => i.Color == form["Color"].ToString() && i.Category == form["Category"].ToString()).ToList();
            return View(items);
        }
    }
}
