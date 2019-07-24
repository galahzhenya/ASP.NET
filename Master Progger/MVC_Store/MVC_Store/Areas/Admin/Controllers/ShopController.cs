using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            //Объявляем модель типа List
            List<CategoryVM> categoryVMList;

            using (Db db = new Db()) {
                //Инициализируем модель
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

            //Возвращаем List в представление 
            return View(categoryVMList);
        }

        // POST: admin/shop/AddNewCategory
        public string AddNewCategory(string catName)
        {
            //Объявляем переменную строковую ID
            string id;

            using (Db db = new Db()) {
                //Проверяем имя категории на уникальность
                if (db.Categories.Any(x => x.Name == catName)) {
                    return "titletaken";
                }
                //инициализируем модель DTO
                CategoryDTO dto = new CategoryDTO();
                //Заполняем данныме в модель
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                //Сохраняем данные
                db.Categories.Add(dto);
                db.SaveChanges();
                //Получаем ID, что бы вернуть его в представление 
                id = dto.Id.ToString();
            }
            //Возвращаем ID в представление
            return id;
        }
        //Создаем метот сортировки
        //POST : "/Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db()) {
                //Реализуем начальный счетчик 
                int count = 1;
                //Инициализируем модель данных
                CategoryDTO dto;
                //Устанавливаем сортировку для каждой страницы
                foreach (var catId in id) {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }

        //Метод удаления страницы 
        //GET : /admin/shop/DeleteCategory/
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db()) {
                //Получение модель категории
                CategoryDTO dto = db.Categories.Find(id);
                //Удаление категорию
                db.Categories.Remove(dto);
                //Созранение изменений в базе 
                db.SaveChanges();
            }
            //Сообщение об успешном удалении
            TempData["SM"] = "You have delete a categoty!";

            //Переадресовываем пользователя на страницу Index
            return RedirectToAction("Categories");
        }

        //POST  admin/shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db()) {
                //Проверим имя на уникальность
                if (db.Categories.Any(x =>x.Name == newCatName)) {
                    return "titletaken";
                }
                //Получаем класс dto
                CategoryDTO dto = db.Categories.Find(id);
                //Редактируем модел dto
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                //Сохраняем изменения
                db.SaveChanges();
            }
            //Возвращаем слово (потому что мето должен хоть что-то возращать
            return "ok";
        }

        //Создаем метод добавления товаров 
        //GET : /admin/shop/AddProduct/

        public ActionResult AddProduct()
        {
            //Объявляем модель данных
            ProductVM model = new ProductVM();

            //Добавляем список категорий из базы в модель
            using (Db db = new Db()) {
                model.Categories = new SelectList(db.Categories.ToList(), "id","Name");
            }
            //Вернуть модель в представление 
            return View(model);
        }
    }
}