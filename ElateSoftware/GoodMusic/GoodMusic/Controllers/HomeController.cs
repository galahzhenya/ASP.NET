using GoodMusic.Models.Data;
using GoodMusic.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
 
namespace GoodMusic.Controllers
{
    public class HomeController : Controller
    {
        //Метод вывода списка песен  
        [HttpGet]
        public ActionResult Songs(int? page)
        {
            //Объявляем модель SongVM типа List
            List<SongVM> listOfSongVM;
            //Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            using (GoodMusicDb db = new GoodMusicDb()) {
                //Инициализируем List и заполняем данными 
                listOfSongVM = db.Songs.ToArray().Select(x => new SongVM(x)).ToList();
            }
            //Устанавливаем постраничную навигацию 
            int songsInPage = 10; 
            var onePageOfSongs = listOfSongVM.ToPagedList(pageNumber, songsInPage);
            ViewBag.onePageOfSongs = onePageOfSongs;
            //Возвращаем представление с данными 
            return View(onePageOfSongs);
        }

        //Метод добавления новой песни
        [HttpGet]
        public ActionResult AddSong()
        {
            //Объявляем модель данных
            SongVM model = new SongVM();

            /*//Добавляем список категорий из базы в модель
            using (GoodMusicDb db = new GoodMusicDb()) {
                model.Categories = new SelectList(db.Categories.ToList(), "id", "Name");
            }*/
            //Вернуть модель в представление 
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSong( SongVM model )
        {
            //Проверить модель на валидность 
            if (!ModelState.IsValid) {
                using (GoodMusicDb db = new GoodMusicDb()) {
                    //model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                }
                return View(model);
            }
            //Проверить имя продукта на уникальность 
            using (GoodMusicDb db = new GoodMusicDb()) {
                if (db.Songs.Any(x => x.Name == model.Name)) {
                    //model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That song name is taken!");
                    return View(model);
                }
                SongDTO song = new SongDTO();
                song.Name = model.Name;
                song.Description = model.Description;
                db.Songs.Add(song);
                db.SaveChanges();
            }
            //Сообщение об удачном добавлении 
            TempData["SM"] = "You have added a song";
            //Переадресовать пользователя 
            return RedirectToAction("AddSong");
        }

        //Метод удаления песни 
        public ActionResult DeleteSong(int id)
        {
            using (GoodMusicDb db = new GoodMusicDb()) {
                //Получение модель категории
                SongDTO dto = db.Songs.Find(id);
                //Удаление категорию
                db.Songs.Remove(dto);
                //Созранение изменений в базе 
                db.SaveChanges();
            }
            //Сообщение об успешном удалении
            TempData["SM"] = "You have delete a song!";

            //Переадресовываем пользователя на страницу Index
            return RedirectToAction("Songs");
        }

        //Метод просмотра деталей песни
        public ActionResult SongDetails(int id)
        {
            //Объявляем модель PageVM
            SongVM model;

            using (GoodMusicDb db = new GoodMusicDb()) {
                //Получаем страницу
                SongDTO dto = db.Songs.Find(id);              
                //Присваиваем модели поля из базы 
                model = new SongVM(dto);
            }
            //Возврат модели представлений

            return View(model);
        }

        //Метод изменения параметров песни
        [HttpGet]
        public ActionResult EditSong(int id)
        {
            //Объявляем модель
            SongVM model;

            using (GoodMusicDb db = new GoodMusicDb()) {
                //Получаем данные из DTO
                SongDTO dto = db.Songs.Find(id);
                //Заполняем модель данными 
                model = new SongVM(dto);

            }
            //Вернуть преставление с моделью
            return View(model);
        }
    }
}