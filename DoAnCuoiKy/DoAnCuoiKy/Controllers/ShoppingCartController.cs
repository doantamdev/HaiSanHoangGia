using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;

namespace DoAnCuoiKy.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DoAnLTWEntities1 database = new DoAnLTWEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart item = Session["Cart"] as Cart;
            return View(item);
        }

        //Action Tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        //Action thêm product vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            //Lấy product theo ID
            var product = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (product != null)
            {
                GetCart().Add_Product_Cart(product);
            }
            return RedirectToAction("showCart", "ShoppingCart");
        }

        //Cập nhật số lượng
        public ActionResult UpdateCartQuantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int ID = int.Parse(form["IDProduct"]);
            int Quantity = int.Parse(form["CartQuantity"]);
            cart.Update_quantity(ID, Quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");

        }

        //Xoá sản phẩm
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        //Icon giỏ hàng
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;

                //Bảng hoá đơn sản phẩm
                OrderPro order = new OrderPro();
                order.DateOrder = DateTime.Now;
                order.AddressDeliverry = form["AddressDelivery"];
                order.IDCus  = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(order);
                foreach (var item in cart.Items)
                {
                    //Lưu dòng sản phẩm vào bảng Chi tiết Hoá đơn
                    OrderDetail detail = new OrderDetail();
                    detail.IDOrder = order.ID;
                    detail.IDProduct = item.product.ProductID;
                    detail.UnitPrice = (double)item.product.Price;
                    detail.Quantity = item.quantity;
                    database.OrderDetails.Add(detail);

                    // -- Xử lý cập nhật lại số lượng tồn trong bảng Product -- //
                    //Lấy ID Product đang có trong giỏ hàng
                    foreach (var p in database.Products.Where(s => s.ProductID==detail.IDProduct))
                    {
                        //Số lượng tồn mới = Số lượng tồn - Số đã mua
                        var updateQuantity = p.Quantity - item.quantity;


                        //Thực hiện cập nhật lại số lượng tồn cho cột Quantity của bảng Product
                        p.Quantity = updateQuantity;
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Lỗi thanh toán - Xin kiểm tra thông tin khách hàng...Xin cảm ơn.");
            }
        }


        //Thanh toán thành công
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}