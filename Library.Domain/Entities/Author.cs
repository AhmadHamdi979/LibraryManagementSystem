using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
