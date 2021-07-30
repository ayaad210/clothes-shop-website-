using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Shop.Helper
{    public static class PagingHelper 
    {
        public static MvcHtmlString pagelinks(this HtmlHelper  Html, PageInfo PageInfo, Func<int, string> PageURL)
        {
            StringBuilder Result = new StringBuilder();

            for (int i = 1; i <= PageInfo.TotalPages; i++)
            {
                TagBuilder TAG = new TagBuilder("a");
                TAG.MergeAttribute("href", PageURL(i));
                TAG.InnerHtml = i.ToString();
                if (PageInfo.CurrentPage == i)
                {
                    TAG.AddCssClass("Selected");
                    TAG.AddCssClass("btn-primary");
                    HtmlHelper x;
                    
                }
                TAG.AddCssClass("btn btn-default");
                Result.Append(TAG.ToString());

            }
            return MvcHtmlString.Create(Result.ToString());
           
        }
    }
   
    }
