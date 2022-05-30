using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace admin.Models
{
    public partial class Place
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int NumberStaff { get; set; }

        private static ShopContext context = new ShopContext();
        public static List<Place> getList()
        {

            var listPlace = context.Places.ToList();

            return listPlace;
        }

        public static bool updateNumberStaff(int Id)
        {
            var place = context.Places.Find(Id);
            place.NumberStaff = place.NumberStaff + 1;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
        }
    }
}
