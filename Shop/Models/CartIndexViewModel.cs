using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class CartIndexViewModel
    {
        //public CartIndexViewModel(string returnUrl)
        //   {
        //       ReternUrl = returnUrl;
        //   }
        public Cart Cart { get; set; }
        public string ReternUrl { get; set; }
     


    }
}