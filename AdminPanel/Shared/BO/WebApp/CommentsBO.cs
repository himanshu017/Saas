using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class CommentsBO
    {
        public long Id { get; set; }
        public byte TypeId { get; set; }
        public long? CustomerId { get; set; }
        public long? UserId { get; set; }
        public string? CommentDescription { get; set; }
    }
}
