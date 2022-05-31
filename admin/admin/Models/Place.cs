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
        public static List<Place> getList(int Id)
        {

            var listStaff = context.Places
                                   .Where(s => s.Id != Id).ToList();

            return listStaff;
        }
        public static Place getPlace(int Id)
        {
            return context.Places.Find(Id);
        }

        public static bool updateNumberStaff(int Id, int number)
        {
            var place = context.Places.Find(Id);
            place.NumberStaff = place.NumberStaff + number;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
        }
    }
}
