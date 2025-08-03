using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Author
{
    public class UpdateAuthorRequest
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
