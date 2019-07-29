using GoodMusic.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoodMusic.Models.ViewModels.Home
{
    public class SongVM
    {
        public SongVM()
        {

        }

        public SongVM(SongDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}