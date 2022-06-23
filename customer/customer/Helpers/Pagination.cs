using System;

namespace customer.Helpers
{
    public class Pagination
    {
        public int Order { get; set; }
        public int Curr { get; set; }
        public int Num { get; set; }
        public int Unit { get; set; }
        public int Max { get; set; }
        public int End { get; set; }
        
        public static int ITEM_PER_PAGE = 3;
        public static int PAGE_PER_PAGINATION = 5;

        public Pagination(int numObjects, int page)
        {
            Num = (int) Math.Ceiling(numObjects * 1.0 / ITEM_PER_PAGE);
            Order = (int) Math.Ceiling(page * 1.0 / PAGE_PER_PAGINATION);
            Max = Order * PAGE_PER_PAGINATION;
            Unit = Max - PAGE_PER_PAGINATION;
            End = (Num < Max) ? Num % PAGE_PER_PAGINATION : (Max - Unit);
            Curr = page;
        }
    }
}