namespace WizLib_Model.Models
{
    public class Fluent_BookAuthor
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public Fluent_Book Fluent_Book { get; set; }
        public Fluent_Author Fluent_Author { get; set; }
    }
}
