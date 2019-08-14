using MVC_Store.Models.Data;
using MVC_Store.Models.VievMidels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class PagesController : Controller
    {
        // GET: Idex/{page}
        public ActionResult Index(string page ="")
        {
            //Получение/Установление краткий заголовок Slug 
            if (page == "") {
                page = "home";
            }
            //Объявление модели и класа DTO
            PageVM model;
            PagesDTO dto;
            //Проверка на доступность текущей страницы 
            using (Db db = new Db()) {
                if (!db.Pages.Any(x => x.Slug.Equals(page))) {
                    return RedirectToAction("Index", new { page = "" });
                }
            }
            //Получение контекста данных 
            using (Db db = new Db()) {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }
            //Установка title страницы
            ViewBag.PageTitle = dto.Title;
            //Проверка боковой панели
            if (dto.HasSidebar) {
                ViewBag.Sidebar = "Yes";
            }
            else {
                ViewBag.Sidebar = "No";
            }
            //Заполнение модели данными
            model = new PageVM(dto);
            //Возврат представления 
            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            //Инициализация листа PageVM
            List<PageVM> pageVMList;
            //Получение всех страниц, кроме home 
            using (Db db = new Db()) {
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            //Возвращение частичного представление с листом данных
            return PartialView("_PagesMenuPartial", pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            //Объявление модели
            SidebarVM model;
            //Инициализация моедли
            using (Db db = new Db()) {
                SidebarDTO dto = db.Sidebars.Find(1);
                model = new SidebarVM(dto);
            }
            //Возврат модели в частичное представление 
            return PartialView("_SidebarPartial", model);
        }
    }
}