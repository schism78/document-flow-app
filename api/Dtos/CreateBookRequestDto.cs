using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api.Dtos
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Annotation { get; set; }
        public int? GenreId { get; set; }
        public int TotalCopies { get; set; } // при созданни равно avaliable copies
    }
}