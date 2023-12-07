using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DoAnCuoiKy.Models
{
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public class CartItem
        {
            public Product product { get; set; }
            public int quantity { get; set; }
        }
        public void Add_Product_Cart(Product add, int soluong = 1)
        {
            var item = Items.FirstOrDefault(s => s.product.ProductID == add.ProductID);

            //Nếu giỏ hàng rỗng thì thêm dòng hàng mới vào giỏ
            if (item == null)
            {
                items.Add(new CartItem
                {
                    product = add,
                    quantity = soluong
                });
            }

            //Tổng số lượng trong giỏ hàng được cộng dồn
            else
                item.quantity += soluong;
        }
        public int Total_quantity()
        {
            return items.Sum(s => s.quantity);
        }
        public decimal Total_money()
        {
            var total = items.Sum(s => s.quantity * s.product.Price);
            return (decimal)total;
        }
        public void Update_quantity(int id, int NewQuantity)
        {
            var item = items.Find(s => s.product.ProductID == id);
            if (item != null)
            {
                //Nếu số lượng mua nhỏ hơn số lượng tồn
                if (items.Find(s => s.product.Quantity > NewQuantity) != null)
                    item.quantity = NewQuantity; //Chấp nhận số lượng mua
                else
                    item.quantity = 1; //Số lượng mua trả về 1
            }
        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s.product.ProductID == id);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}