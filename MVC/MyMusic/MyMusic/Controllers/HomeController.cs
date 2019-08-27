using MyMusic.Models.Data;
using MyMusic.Models.Data.ViewModels;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyMusic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Songs(int? page)
        {
            //Объявляем модель SongVM типа List
            List<SongVM> listOfSongVM;
            //Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            using (MyMusicDb db = new MyMusicDb()) {
                //Инициализируем List и заполняем данными 
                listOfSongVM = db.Songs.ToArray().Select(x => new SongVM(x)).ToList();
            }
            //Устанавливаем постраничную навигацию 
            int songsInPage = 10;
            var onePageOfSongs = listOfSongVM.ToPagedList(pageNumber, songsInPage);
            //Возвращаем представление с данными 
            return View(onePageOfSongs);
        }

        //Метод добавления новой песни
        [HttpGet]
        public ActionResult AddSong()
        {
            SongVM model = new SongVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSong(SongVM model)
        {
            //Проверка модели на валидность 
            if (!ModelState.IsValid) {
                return View(model);
            }
            using (MyMusicDb db = new MyMusicDb()) {
                //Проверка песни на уникальность 
                if (db.Songs.Any(x => x.Name == model.Name)&& db.Songs.Any(x => x.GroupName == model.GroupName)) {
                    ModelState.AddModelError("", "This song has already been added");
                    return View(model);
                }
                SongDTO song = new SongDTO();
                song.Name = model.Name;
                song.Description = model.Description;
                song.Lyrics = model.Lyrics;
                song.GroupName = model.GroupName;
                db.Songs.Add(song);
                db.SaveChanges();
            }
            //Сообщение об удачном добавлении 
            TempData["SM"] = "You have added a song";
            return RedirectToAction("AddSong");
        }

        //Метод удаления песни 
        public ActionResult DeleteSong(int id)
        {
            using (MyMusicDb db = new MyMusicDb()) {
                SongDTO dto = db.Songs.Find(id);
                //Удаление песни
                db.Songs.Remove(dto);
                db.SaveChanges();
            }
            //Сообщение об успешном удалении
            TempData["SM"] = "You have delete a song!";
            return RedirectToAction("Songs");
        }

        //Метод просмотра деталей песни
        public ActionResult SongDetails(int id)
        {
            SongVM model;

            using (MyMusicDb db = new MyMusicDb()) {
                SongDTO dto = db.Songs.Find(id);
                model = new SongVM(dto);
            }
            return View(model);
        }

        //Метод поиска песен 
        [HttpGet]
        public ActionResult Search(string search)
        {
            List<SongVM> listOfSongVM;
            //Устанавливаем номер страницы
            var pageNumber = 1; 
            using (MyMusicDb db = new MyMusicDb()) {
                //Инициализируем List и заполняем данными 
                listOfSongVM = db.Songs.Where(x => x.Name == search).ToArray().Select(x => new SongVM(x)).ToList(); 
            }
            //Устанавливаем постраничную навигацию 
            int songsInPage = 10;
            var onePageOfSongs = listOfSongVM.ToPagedList(pageNumber, songsInPage);
            return View("Songs",onePageOfSongs);
        }

    }
}