using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ItemListViewListModel
    {
     public   List<Item> Items;
        public PageInfo paginginfo { get; set; }
        public int CurrentCatigoryId { get; set; }
     public   string SearchString { get; set; }
    }
  
}