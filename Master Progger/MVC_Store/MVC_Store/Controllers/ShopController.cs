using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult Category(string name)
        {
            List<ProductVM> productVMList;
            using (Db db = new Db()) {
                CategoryDTO categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;
                productVMList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();

                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();

                if (productCat == null) {
                    var catName = db.Categories.Where(x => x.Slug == name).Select(x => x.Name).FirstOrDefault();
                    ViewBag.CategoryName = catName;
                }
                else {
                    ViewBag.CategoryName = productCat.CategoryName;
                }
            }
            return View(productVMList);
        }

        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //Объявляем модели данных 
            ProductVM model;
            ProductDTO dto;
            //Инициализируем id продукта
            int id = 0;
            //Проверка на доступность продукта
            using (Db db = new Db()) {
                if (!db.Products.Any(x => x.Slug.Equals(name))) {
                    return RedirectToAction("Index", "Shop");
                }
                //Инициализируем модель ProductDTO данными 
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
                //Получаем id
                id = dto.Id;
                //Инициализируем модель ProductVM данными
                model = new ProductVM(dto);
            }
            //Получаем изображения из галереи 
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));
            //Возвращаем модель в представление 
            return View("ProductDetails", model);
        }
    }
}