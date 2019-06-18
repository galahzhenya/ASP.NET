using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PhoneMarket.Models
{
    public class DBInitializer : DropCreateDatabaseAlways<PhoneContext>
    {
        protected override void Seed(PhoneContext context)
        {
            context.Phones.Add(new Phone() { Name = "Huawei P20", Price = 40000, Produser = "Huawei" });
            context.Phones.Add(new Phone() { Name = "Samsung S9", Price = 58000, Produser = "Samsung" });
            context.Phones.Add(new Phone() { Name = "LG G6", Price = 37000, Produser = "LG" });

            base.Seed(context);
        }
    }
}