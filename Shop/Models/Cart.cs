using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Cart
    {
        public Cart()
        {
            this.DateTime = DateTime.Now;
        }
        public   Cart OldCart { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal SellPrice { get; set; }
        public int OrderId { get; internal set; }

        public List<CartLine> LineCollecrtion = new List<CartLine>();


        ShopDBEntities1 db = new ShopDBEntities1();

        public void AddItem(Item Item, int Quantity = 1)
        {
            if (Item != null)
            {
                CartLine oldLine =( OldCart==null?null:OldCart.LineCollecrtion.Where(ol => ol.item.id==Item.id).FirstOrDefault());
                int oldQuantity =( oldLine == null ? 0 : oldLine.Quantity);

                CartLine line = LineCollecrtion.Where(b => b.item.id == Item.id).FirstOrDefault();

                if (line == null && Item.Count + oldQuantity >= 1)
                {
                    LineCollecrtion.Add(new CartLine { item = Item, Quantity = Quantity });
                }

                else if (line!=null)
                {
                    if (line.Quantity + Quantity <= Item.Count+oldQuantity)
                    {
                        line.Quantity += Quantity;

                    }
                }
            }

        }//end add

        public  void MinusItem(int id)
        {
            Item item = db.Items.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                foreach (CartLine line in LineCollecrtion)
                {
                    if (line.item.id == item.id && line.Quantity > 1)
                    {
                        line.Quantity--;
                    }
                }

            };
        }

        public void RemoveLine(Item item)
        {
          
            LineCollecrtion.RemoveAll(b => b.item.id == item.id);

        }//end removeline

        public decimal ComputeTotalValue()
        {
            return LineCollecrtion.Sum(b => b.item.SellPrice* b.Quantity);
        }//end compute

        public void Clear()
        {
            OldCart = null;
            LineCollecrtion.Clear();
            this.DateTime = DateTime.Now;
            this.SellPrice = 0;
            this.CustomerId = 0;
            
            
        }

    }//endCart


    public class CartLine
    {
        public Item item { get; set; }
        public int Quantity { get; set; }
        public string ReternUrl { get; set; }

    }
}

