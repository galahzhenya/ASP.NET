﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Store.Models.Data
{
    [Table("tblSidebar")]
    public class SidebarDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }

    }
}