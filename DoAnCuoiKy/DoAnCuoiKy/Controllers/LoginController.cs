using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;

namespace DoAnCuoiKy.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DoAnLTWEntities1 database = new DoAnLTWEntities1();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authen(Customer customer)
        { 
            var check = database.Customers.Where(s => s.EmailCus.Equals(customer.EmailCus)&& s.PassCus.Equals(customer.PassCus)).FirstOrDefault();
            if (check == null)
            {
                customer.LoginErrorMessage ="Error Email or Password, Please try again!";
                return View("Index",customer);
            }
            else
            {
                Session["IDCus"]=customer.IDCus;
                Session["EmailCus"] = customer.EmailCus;
                return RedirectToAction("Index","Home");
            }           
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var check = database.Customers.FirstOrDefault(s => s.EmailCus == customer.EmailCus);
                if (check == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.Customers.Add(customer);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists!Use another email please";
                    return View();
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }

        public ActionResult Show()
        {

            return View(database.Customers.ToList());
        }

    }
}