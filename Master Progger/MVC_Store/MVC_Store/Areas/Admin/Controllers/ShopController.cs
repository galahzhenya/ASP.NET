﻿using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
        [HttpGet]
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

        //Создаем метод добавления товаров 
        //POST : /admin/shop/AddProduct/
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            //Проверить модель на валидность 
            if (!ModelState.IsValid) {
                using (Db db = new Db()) {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            //Проверить имя продукта на уникальность 
            using (Db db = new Db()) {
                if (db.Products.Any(x => x.Name == model.Name)) {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is talen!");
                    return View(model);
                }
            }
            //Объявляем переменную ProductId 
            int id;
            //Инициализируем и сохраняем в базу  на основе ProductDTO
            using (Db db = new Db()) {
                ProductDTO product = new ProductDTO();
                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "_").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                CategoryDTO catDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = catDTO.Name;

                db.Products.Add(product);
                db.SaveChanges();

                id = product.Id;
            }
            //Добавить сообщение пользователю 
            TempData["SM"] = "You have added a product";

            #region Upload Image
            //Создать необходимые ссылки дериктории  (пути куда сохранять фото) 
            var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Imeges\\Uploads"));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\"+id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products" + id.ToString() + "\\Gallery\\Thumbs");
            //Проверяем наличие директории (если нет - создаем)
            if (!Directory.Exists(pathString1)) {
                Directory.CreateDirectory(pathString1);
            }
            if (!Directory.Exists(pathString2)) {
                Directory.CreateDirectory(pathString2);
            }
            if (!Directory.Exists(pathString3)) {
                Directory.CreateDirectory(pathString3);
            }
            if (!Directory.Exists(pathString4)) {
                Directory.CreateDirectory(pathString4);
            }
            if (!Directory.Exists(pathString5)) {
                Directory.CreateDirectory(pathString5);
            }

            //Проверяем был ли уже такой файл загружен 
            if (file != null && file.ContentLength > 0) {
                //Получаем расширение файла
                string ext = file.ContentType.ToLower();
                //Проверяем расширение файла 
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png") {

                    using (Db db = new Db()) {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                        return View(model);
                    }
                }

                //Переменная с именем изображения (Объявляем) 
                string imageName = file.FileName;
                //Сохраняем изображение в модель DTO
                using (Db db = new Db()) {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;
                    db.SaveChanges();
                }
                //Назначаем пути к оригинальному и уменшенному изображению 
                var path = string.Format($"{pathString2}\\{imageName}"); //Оригинальное изображение
                var path2 = string.Format($"{pathString3}\\{imageName}"); // Уменьшенное 
                //Сохраняем оригиналльное изображение 
                file.SaveAs(path);
                //Создаем и сохраняем уменьшенную 
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion
            //Переадресовать пользователя 
            return RedirectToAction("AddProduct");
        }
    }
}