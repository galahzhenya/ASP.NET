﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVC_Store.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }
    }
}