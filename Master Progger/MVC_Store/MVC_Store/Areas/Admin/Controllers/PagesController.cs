using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Объявляем список для представления (PageVM)
            List<PageVM> pageList; 

            //Инициализировать список (DB)
            using (Db db = new Db()) 
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Возвращаем список в представление 
            return View(pageList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Проверка модели на валидность 
            if (!ModelState.IsValid) {
                return View(model);
            }

            using (Db db = new Db()) {
                // Объевляем переменную для краткого описания (slug)
                string slug;
                //инициализируем класс PagesDTO
                PagesDTO dto = new PagesDTO();
                //Присваиваем заголовок модели 
                dto.Title = model.Title.ToUpper(); 
                //Проверяем есть ли краткое описание, если нет присваиваем
                if (string.IsNullOrWhiteSpace(model.Slug)) {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Убеждаемся, что заголовок и краткое описание уникален 
                if ( db.Pages.Any(x => x.Title == model.Title)) {
                    ModelState.AddModelError("", "Thet title already exist.");
                    return View(model);
                }
                else if ( db.Pages.Any (x => x.Slug == model.Slug)) {
                    ModelState.AddModelError("", "Thet slug already exist.");
                    return View(model);
                }

                //Присваиваем оставшееся значение модели 
                dto.Slug = slug;
                dto.Body = model.Slug;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // Сохраняем модель в базу данных 
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            //Передае сообщение через TempData 
            TempData["SM"] = "You have added a new page";
            //переадресовыввем пользователя на метод Index 
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Объявляем модель PageVM
            PageVM model;
            using (Db db = new Db()) {
                //Получаем страницу
                PagesDTO dto = db.Pages.Find(id);
                //Проверяем, доступна ли страница
                if(dto == null) {
                    return Content("The page does not exist");
                }
                //Инициализируем модель данными 
                model = new PageVM(dto);
            }
            //Возвращаем представление в представление 

            return View(model);
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Проверить модель на валидность
            if (!ModelState.IsValid) {
                return View(model);
            }

            using (Db db = new Db()) {
                // Получаем id страницы
                int id = model.Id;
                //Объявим переменную для заголовка 
                string slug = null;
                //Получаем страницу по id 
                PagesDTO dto = db.Pages.Find(id);
                //Присваиваем название из полученной модели в DTO
                dto.Title = model.Title;
                //Проверяем краткий заголовок и присваиваем его, если это необходимо 
                if (model.Slug != "home") {
                    if (string.IsNullOrWhiteSpace(model.Slug)) {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                           
                }
                else {
                    slug = "home";
                }
                //Проверяем slug и title на уникальность 
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title)){
                    ModelState.AddModelError("", "That title alredy exist.");
                    return View(model);
                }
                else if (db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug)) {
                    ModelState.AddModelError("", "That slug alredy exist.");
                    return View(model);
                }
                //Присвоить остальные значения в DTO 
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                //Сохраняем изменения в базу 
                db.SaveChanges();
            }
            //Оповестить пользователя (установить сообщение в TempData)
            TempData["SM"] = "You have edited the page.";
            //Переадресация 
            return RedirectToAction("EditPage");
        }

        //GET : Admin/Pages/PageDetails/id
        public ActionResult PageDetails (int id)
        {
            //Объявляем модель PageVM
            PageVM model;

            using (Db db = new Db()) {
                //Получаем страницу
                PagesDTO dto = db.Pages.Find(id);
                //Проверяем, что страница доступна 
                if (dto == null) {
                    return Content("The page does not exist");
                }
                //Присваиваем модели поля из базы 
                model = new PageVM(dto);
            }
            //Возврат модели представлений

            return View(model);
        }

        //Метод удаления страницы 
        //GET : Admin/Pages/Delete/id
        public ActionResult DeletePage( int id)
        {
            using (Db db = new Db()) {
                //Получение страницы 
                PagesDTO dto = db.Pages.Find(id);
                //Удаление страницы
                db.Pages.Remove(dto);
                //Созранение изменений в базе 
                db.SaveChanges();
            }
            //Сообщение об успешном удалении
            TempData["SM"] = "You have delete a page!";

            //Переадресовываем пользователя на страницу Index
            return RedirectToAction("Index");
        }

        //Создаем метот сортировки
		//POST : "/Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db()) {
                //Реализуем начальный счетчик 
                int count = 1;
                //Инициализируем модель данных
                PagesDTO dto;
                //Устанавливаем сортировку для каждой страницы
                foreach (var pageId in id) {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }

        //GET : Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Объявляем модель
            SidebarVM model;

            using (Db db = new Db()) {
                //Получаем данные из DTO
                SidebarDTO dto = db.Sidebars.Find(1);
                //Заполняем модель данными 
                model = new SidebarVM(dto);

            }
            //Вернуть преставление с моделью
            return View(model);
        }

        //POST : Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db()) {
                //Получаем данные из DTO
                SidebarDTO dto = db.Sidebars.Find(1);
                //Присвоить данные в тело  ( в свойство Body )
                dto.Body = model.Body;
                //Сохранить 
                db.SaveChanges();
            }
            //Присвоить сообщение об удаче в TeamData
            TempData["SM"] = "You have edited the sidebar!"; 
            //Переадресовываем пользователей (админов)

            return RedirectToAction("EditSidebar");
        }

    }
}