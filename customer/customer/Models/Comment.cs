

#nullable disable
using System;
using System.Linq;
using Newtonsoft.Json;

namespace customer.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? IdProduct { get; set; }
        public int? IdAccount { get; set; }

        private static ShopContext _context = new ShopContext();
        
        public static Comment LeaveComment(int accountId, int productId, string content)
        {
            var comment = new Comment
            {
                IdAccount = accountId,
                IdProduct = productId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            
            return comment;
        }

        public static dynamic RetrieveComments(int productId, int page)
        {
            var comments = (from c in _context.Comments 
                join a in _context.Accounts on c.IdAccount equals a.Id
                where c.IdProduct == productId
                orderby c.CreatedAt descending
                select new
                {
                    id = c.Id,
                    content = c.Content,
                    createdAt = c.CreatedAt,
                    accountName = a.Name,
                    accountAvatar = a.Avatar
                });
            
            int numObjects = comments.Count();
            
            var commentsPaged = comments
                .Skip((page - 1) * Pagination.ITEM_PER_PAGE)
                .Take(Pagination.ITEM_PER_PAGE)
                .ToList();

            Pagination pagination = new Pagination(numObjects, page);

            return new { comments = commentsPaged, pagination };
        }
    }
}
