using Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace Shop.Helper
{
    public static class EventsClass
    {
      static  ShopDBEntities1 db = new ShopDBEntities1();

        public static Order ConvertCartToOrder(Cart Cart)
        {
            Order Order = new Order();

            if (Cart != null)
            {
                Order.id = Cart.OrderId;
                   Order.Customerid =  Cart.CustomerId;
                Order.Userid = 2;
                Order.SellPrice = Cart.SellPrice;
                Order.TotalPrice =Cart.ComputeTotalValue();
                Order.DateTime = Cart.DateTime;
                Order.IsDeleted = false;
             
                
               foreach (CartLine line in Cart.LineCollecrtion)
                {
                    Order.OrderDetails.Add(new OrderDetail() { ItemId = line.item.id, Count = line.Quantity, ItemPrice = line.item.SellPrice });

                }
                
              
            }
            return     Order;
        }

        public static void InsertEvent(Cart cart,int Userid,int option=0)
        {
            Order order = ConvertCartToOrder(cart);

            StringBuilder sb = new StringBuilder();
            sb.Append("تم اضافة بواسطة  ");
            sb.Append(" " + db.Users.Where(u => u.id == Userid).First().Name );
            
            foreach (CartLine line in cart.LineCollecrtion)
            {
                Item item = db.Items.Where(i => i.IsDeleted == false && i.id == line.item.id).First();
                item.Count -= line.Quantity;
                sb.AppendLine(" , " + line.Quantity + " " +line.item.Name);

            }
            sb.AppendLine(" :ل ");
                sb.Append(" " + db.Customers.Where(c => c.Id == cart.CustomerId).First().Name+" ");
            db.Orders.Add(order);
            if (option==0)
            {
                db.Events.Add(new Shop.Event() { EventDateTime = DateTime.Now, Text = sb.ToString() });

            }
            db.SaveChanges();

        }
        public static void EditEvent(Cart cart, int Userid)
        {
            Order order = ConvertCartToOrder(cart);

            StringBuilder sb = new StringBuilder();
            sb.Append("تم تعديل بواسطة  ");
            sb.Append(" " + db.Users.Where(u => u.id == Userid).First().Name);
            sb.Append(" من ");
           

            foreach (CartLine line in cart.OldCart.LineCollecrtion)
            {
                Item item = db.Items.Where(i => i.IsDeleted == false && i.id == line.item.id).First();
                sb.AppendLine(" , " + line.Quantity + " " + line.item.Name);

            }
            sb.AppendLine(" :ل ");
            sb.Append(" " + db.Customers.Where(c => c.Id == cart.CustomerId).First().Name + " ");
            sb.Append(" اللذى تم اضافتة بواسطة    ");
            sb.Append(" " + db.Users.Where(u => u.id == cart.OldCart.UserId).First().Name);
            sb.Append(" بسعر    ");
            sb.Append(cart.SellPrice.ToString());
            sb.Append(" ج    ");

            DeleteEvent(cart.OrderId, 2,1);

            sb.AppendLine(" :الى ");
            foreach (CartLine line in cart.LineCollecrtion)
            {
                Item item = db.Items.Where(i => i.IsDeleted == false && i.id == line.item.id).First();
                sb.AppendLine(" , " + line.Quantity + " " + line.item.Name);

            }
            sb.AppendLine(" :ل ");
            sb.Append(" " + db.Customers.Where(c => c.Id == cart.CustomerId).First().Name + " ");
           
            sb.Append(" بسعر    ");
            sb.Append(cart.SellPrice.ToString());
            sb.Append(" ج    ");

            InsertEvent(cart, 2,1);
            db.Events.Add(new Shop.Event() { EventDateTime = DateTime.Now, Text = sb.ToString() });
            db.SaveChanges();


            //foreach (CartLine line in cart.LineCollecrtion)
            //{
            //  CartLine OldeLine=  cart.OldCart.LineCollecrtion.Where(c => c.item.id == line.item.id).FirstOrDefault();
            //    if (OldeLine==null)
            //    {
            //        db.Items.Find(line.item.id).Count -= line.Quantity;
            //        sb.Append(" حيث تم اضافة ");
            //        sb.Append(line.Quantity.ToString());
            //        sb.Append(" ");
            //        sb.Append(line.item.Name);
            //        order.OrderDetails.Add(new OrderDetail() { Count = line.Quantity, ItemId = line.item.id, ItemPrice = line.item.SellPrice, IsDeleted = false });

            //    }

            //    else

            //    {
            //        sb.Append(" و ");

            //        if (OldeLine.Quantity>line.Quantity)
            //        {
            //            db.Items.Find(line.item.id).Count += (OldeLine.Quantity- line.Quantity);

            //        }
            //        else if (OldeLine.Quantity < line.Quantity)
            //        {
            //            db.Items.Find(line.item.id).Count -= (line.Quantity - OldeLine.Quantity);

            //        }
            //        sb.Append("و تعديل");
            //        sb.Append(" من ");
            //        sb.Append(" "+OldeLine.Quantity.ToString()+" ");
            //        sb.Append(OldeLine.item.Name);
            //        sb.Append(" الى ");
            //        sb.Append(" " + line.Quantity.ToString() + " ");
            //        sb.Append(line.item.Name);
            //       order.OrderDetails.Where(d => d.ItemId == OldeLine.item.id).FirstOrDefault();

            //        cart.OldCart.LineCollecrtion.Remove(OldeLine);
            //    }


            //}
            //sb.Append(" وتم اضافةجديد  ");

            //foreach (CartLine OldLine in cart.OldCart.LineCollecrtion)
            //{
            //    db.Items.Find(OldLine.item.id).Count += OldLine.Quantity;
            //    sb.Append(OldLine.Quantity.ToString());
            //    sb.Append("  ");

            //    sb.Append(OldLine.item.Name);
            //    sb.Append("  ");

            //}

            //db.Entry(order).State = EntityState.Modified;
            ////db.Events.Add(new Shop.Event() { EventDateTime = DateTime.Now, Text = sb.ToString() });

            //db.SaveChanges();



        }
        public static void DeleteEvent(int Orderid, int Userid,int option=0)
        {
            Order order = db.Orders.Find(Orderid);

            StringBuilder sb = new StringBuilder();
            sb.Append("تم مسح بواسطة  ");
            sb.Append(" " + db.Users.Where(u => u.id == Userid).First().Name);
            sb.Append(" الطلبالذى يحتوى على");
            foreach (OrderDetail line in order.OrderDetails)
            {
                Item item = db.Items.Where(i => i.IsDeleted == false && i.id == line.ItemId).First();
                item.Count += line.Count;
                sb.AppendLine(" , " + line.Count + " " + line.Item.Name);

            }
            sb.AppendLine(" :ل العميل ");
            sb.Append(" " + db.Customers.Where(c => c.Id == order.Customerid).First().Name + " ");
            db.Orders.Remove(order);
            if (option==0)
            {
            db.Events.Add(new Shop.Event() { EventDateTime = DateTime.Now, Text = sb.ToString() });

            }

            db.SaveChanges();

        }


    }
}


