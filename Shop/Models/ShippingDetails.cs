using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ShippingDetails

    {
        [Required(ErrorMessage = "Please Enter  Name")]
        public string Name { get; set; }


        [Display(Name = "Adress Line1")]
        public string Line1 { get; set; }


        [Display(Name = "Adress Line2")]
        public string Line2 { get; set; }


        [Required(ErrorMessage = "Please Enter  City")]
        public string City { get; set; }


        [Required(ErrorMessage = "Please Enter  Country")]
        public string Country { get; set; }

        public bool GifWrap { get; set; }


    }
}