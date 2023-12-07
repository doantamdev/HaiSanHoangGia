using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;

namespace DoAnCuoiKy.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        DoAnLTWEntities1 database = new DoAnLTWEntities1();

        public PartialViewResult CategoryPartial()
        {
            var cateList = database.Categories.ToList();
            return PartialView(cateList);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category cate)
        {
            try
            {
                database.Categories.Add(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error create New");
            }
        }
        public ActionResult Details(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        public ActionResult Edit(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, Category cate)
        {
            database.Entry(cate).State=System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            try
            {
                cate=database.Categories.Where(s => s.Id==id).FirstOrDefault();
                database.Categories.Remove(cate);
                database.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                return Content("This data is using in other table,Error Delete!");
            }
        }
        public ActionResult Index(string _name)
        {
            if (_name ==null)
            {
                return View(database.Categories.ToList());
            }
            else
            {
                return View(database.Categories.Where(s => s.NameCate.Contains(_name)).ToList());
            }
        }
    }
}