using System;
using System.Collections.Generic;
using System.Linq;
using admin.Helpers;
using System.Web;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace admin.Models
{
    public partial class Place
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int NumberStaff { get; set; }
        public string Name { get; set; }

        private static ShopContext context = new ShopContext();
        public static string getListOfPag(int page)
        {
            var list = context.Places;

            var listPaged = list
                .Skip((page - 1) * Pagination.ITEM_PER_PAGE)
                .Take(Pagination.ITEM_PER_PAGE)
                .ToList();

            return JsonConvert.SerializeObject(listPaged);
        }

        public static List<Place> getList()
        {
            var listPlace = context.Places.ToList();

            return listPlace;
        }

        public static List<Place> getList(int Id)
        {

            var list = context.Places
                                   .Where(s => s.Id != Id).ToList();

            return list;
        }
        public static Place getPlace(int Id)
        {
            return context.Places.Find(Id);
        }
        public static int totalPlace()
        {
            var list = context.Places;

            return list.Count();
        }

        public static List<Place> search(string search, string sort)
        {
            var place = ( from p in context.Places
                         where EF.Functions.Like(p.Name, $"%{search}%")
                         select p);
            if (sort == "name")
                place = place.OrderBy(p => p.Name);

            if (sort == "numberStaff")
                place = place.OrderBy(p => p.NumberStaff);

            return place.ToList();
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

        public static bool createPlace(string placeName, string address)
        {
            Place place = context.Places
                                   .Where(p => p.Name == placeName).SingleOrDefault();

            if(place == null)
            {
                var newPlace = new Place()
                {
                    Address = address,
                    NumberStaff = 0,
                    Name = placeName
                };
                context.Places.Add(newPlace);
                var rs = context.SaveChanges();
                if (rs == 0)
                    return false;
                return true;
            }
            return false;
            
        }

        public static bool updatePlace(int Id, string placeName, string address)
        {
            Place placeCur = context.Places.Find(Id);
            if (placeCur.Name == placeName && placeCur.Address == address)
                return true;

            Place placeCheck = context.Places
                                  .Where(p => p.Name == placeName).SingleOrDefault();

            if (placeCheck == null)
            {
                placeCur.Name = placeName;
                placeCur.Address = address;

                var rs = context.SaveChanges();
                if (rs == 0)
                    return false;
                return true;
            }
            return false;
        }
    }
}
