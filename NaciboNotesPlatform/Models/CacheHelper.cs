using Project.BLL.Managers;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace NaciboNotesPlatform.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");
            if(result == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                result = categoryManager.GetActives();
                WebCache.Set("category-cahce", result, 20, true);
            }
            return result;

        }
        public static void RemoveCategoriesFromCache() //Bir kategori eklenip, silinip, güncellendikten sonra eski verilerin cache'te kalmaması için bu metodu yazdım.
        {
            RemoveCache("category-cache");
        }
        public static void RemoveCache(string key) //anahtar değeri ("category-cache") hatırlamak gerekmesin sürekli diye yukarıdaki metodu yazdık 
        {
            WebCache.Remove(key);
        }
    }
}