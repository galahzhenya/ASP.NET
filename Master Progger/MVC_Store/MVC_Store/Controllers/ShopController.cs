using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            //Объявление модели
            List<CategoryVM> categoryVMList;
            //Инициализация модели
            using (Db db = new Db()) {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x))
                    .ToList();
            }
            //Возвращение частичного представления с моделью 
            return PartialView("_CategoryMenuPartial", categoryVMList);
        }
    }
}