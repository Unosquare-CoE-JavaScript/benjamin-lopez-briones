namespace WizLib_Model.Models
{
    public class BookAuthor
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public Authors Author { get; set; }
        public Book Books { get; set; }
    }
}
