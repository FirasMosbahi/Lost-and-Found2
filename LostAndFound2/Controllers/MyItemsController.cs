using LostAndFound2.Data;
using LostAndFound2.Data.UnitOfWork;
using LostAndFound2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound2.Controllers
{
    public class MyItemsController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork(DBContext.Instance);
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("id") == "")
                return RedirectToAction("Login", "User");
            List<Item> items = _unitOfWork.ItemRepository.Find(i => i.Owner.Id.ToString() == HttpContext.Session.GetString("id")).ToList();
            return View(items);
        }
        public IActionResult ModifyItem(int id)
        {
            Item item = _unitOfWork.ItemRepository.GetById(id);
            if (HttpContext.Session.GetString("id") == item.Owner.Id.ToString())
            {
                return View();
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public IActionResult ModifyItem(IFormCollection form)
        {
            Item item = _unitOfWork.ItemRepository.GetById(int.Parse(HttpContext.Session.GetString("item")));
            if (form["Name"] != "")
                item.Name = form["Name"].ToString();
            if (form["Color"] != "")
                item.Color = form["Color"].ToString();
            if (form["Category"] != "")
                item.Category = form["Category"].ToString();
            if (form["Description"] != "")
                item.Category = form["Description"].ToString();
            if (form["PhotoLink"] != "")
                item.PhotoLink = form["PhotoLink"].ToString();
            _unitOfWork.Complete();
            HttpContext.Session.SetString("success", "item modified successfuly");
            return RedirectToAction("MyItems", "Item");
        }
        public IActionResult DeleteItem(int id)
        {
            _unitOfWork.ItemRepository.Delete(_unitOfWork.ItemRepository.GetById(id));
            _unitOfWork.Complete();
            HttpContext.Session.SetString("success", "item deleted successfuly");
            return RedirectToAction("MyItems", "Item");
        }
    }
}
