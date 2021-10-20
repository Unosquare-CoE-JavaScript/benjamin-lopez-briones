using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib_Model.Models
{
    public class Fluent_Book
    {
        public int Book_ID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public double Price { get; set; }
        public int BookDetailID { get; set; }
        public Fluent_BookDetail BookDetail { get; set; }
        public int PublisherID { get; set; }
        public Fluent_Publisher Publisher { get; set; }
        public ICollection<Fluent_BookAuthor> BookAuthors { get; set; }
    }
}
