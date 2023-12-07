using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.Ajax.Utilities;

namespace DoAnCuoiKy.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DoAnLTWEntities1 database = new DoAnLTWEntities1();
        public ActionResult Index(string category, int? page, double min = double.MinValue, double max = double.MaxValue)
        {
            //Kích thước trang
            int pageSize = 8;
            int pageNum = (page ?? 1);


            if (category == null)
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro);
                return View(productList.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro)
                    .Where(x => x.Category == category);
                return View(productList);
            }

        }
        public ActionResult SearchOption(double min = double.MinValue, double max = double.MaxValue)
        {
            var list = database.Products.Where(p => (double)p.Price >= min && (double)p.Price <= max).ToList();
            return View(list);
        }
        public ActionResult Create()
        {
            Product pro = new Product();
            return View(pro);
        }
        public ActionResult SelectCate()
        {
            Category se_cate = new Category();
            se_cate.ListCate = database.Categories.ToList<Category>();
            return PartialView(se_cate);
        }
        [HttpPost]

        public ActionResult Create(Product pro)
        {
            try
            {
                if (pro.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pro.UploadImage.FileName);
                    string extent = Path.GetExtension(pro.UploadImage.FileName);
                    filename = filename + extent;
                    pro.ImagePro = "~/Content/images/" + filename;
                    pro.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                }
                database.Products.Add(pro);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var findPro = database.Products.Find(id);

            if (findPro == null)
            {

                return HttpNotFound();
            }


            List<Category> list = database.Categories.ToList();


            ViewBag.listCategory = new SelectList(list, "IDCate", "NameCate", findPro.Category);

            return View(findPro);
        }
        [HttpPost]
        public ActionResult Edit(int id, Product pro)
        {
            try
            {
                // Find the existing product by ID
                var existingProduct = database.Products.Find(id);

                if (existingProduct == null)
                {
                    // Handle the case where the product with the given ID doesn't exist
                    return HttpNotFound();
                }

                // Update the product properties
                existingProduct.NamePro = pro.NamePro;
                existingProduct.Price = pro.Price;
                existingProduct.Category = pro.Category;
                existingProduct.DecriptionPro = pro.DecriptionPro;
                existingProduct.Quantity = pro.Quantity;


                if (pro.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pro.UploadImage.FileName);
                    string extent = Path.GetExtension(pro.UploadImage.FileName);
                    filename = filename + extent;
                    existingProduct.ImagePro = "~/Content/images/" + filename;
                    pro.UploadImage.SaveAs(Server.MapPath("~/Content/images/" + filename));
                }
                List<Category> list = database.Categories.ToList();

                ViewBag.listCategory = new SelectList(list, "IDCate", "NameCate", existingProduct.Category);

                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View(database.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product pro)
        {
            try
            {
                pro = database.Products.Where(s => s.ProductID == id).FirstOrDefault();
                database.Products.Remove(pro);
                database.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                return Content("Không xóa được");
            }
        }
    }
}