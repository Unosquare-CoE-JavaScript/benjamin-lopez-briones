using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext a)
        {
            _db = a;
        }

        public IActionResult Index()
        {
            List<Book> objList = _db.Books.Include(u => u.Publisher)
                                .Include(u=>u.BookAuthors).ThenInclude(u => u.Author).ToList(); //eager loading
            //List<Book> objList = _db.Books.ToList();
            //foreach (var obj in objList)
            //{
            //    //obj.Publisher = _db.Publishers.FirstOrDefault(u => u.PublisherID == obj.Publisher_Id);
            //    _db.Entry(obj).Reference(u => u.Publisher).Load(); //explicit loading
            //    _db.Entry(obj).Collection(u => u.BookAuthors).Load();
            //    foreach(var bookAuth in obj.BookAuthors)
            //    {
            //        _db.Entry(bookAuth).Reference(u => u.Author).Load(); //explicit loading
            //    }
            //}
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.PublisherID.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            obj.Book = _db.Books.FirstOrDefault(u => u.Book_ID == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
            if (obj.Book.Book_ID == 0)
            {
                _db.Books.Add(obj.Book);
            }
            else
            {
                _db.Books.Update(obj.Book);
            }
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.PublisherID.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            obj.Book = _db.Books.Include(u=>u.BookDetail).FirstOrDefault(u => u.Book_ID == id);
            //obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(u => u.BookDetailID == obj.Book.BookDetailID);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetailID == 0)
            {
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();
                var BBD = _db.Books.FirstOrDefault(u => u.Book_ID == obj.Book.Book_ID);
                BBD.BookDetailID = obj.Book.BookDetail.BookDetailID;
                _db.SaveChanges();
            }
            else
            {
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges();
            }
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var ObjDB = _db.Books.FirstOrDefault(u => u.Book_ID == id);
            _db.Books.Remove(ObjDB);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _db.BookAuthors.Include(u => u.Author).Include(u => u.Books).Where(u => u.BookID == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    BookID = id
                },
                Book = _db.Books.FirstOrDefault(u => u.Book_ID == id)
            };
            List<int> tempListOfAssignendAuthors = obj.BookAuthorList.Select(u => u.AuthorID).ToList();
            //not in LINQ, get all authors whose id is not in tempListofassignedAuthors
            var tempList = _db.Authors.Where(u => !tempListOfAssignendAuthors.Contains(u.Author_ID)).ToList();
            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_ID.ToString()
            });

            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.BookID != 0 && bookAuthorVM.BookAuthor.AuthorID != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }

            return RedirectToActionPermanent(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.BookID });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookID = bookAuthorVM.Book.Book_ID;
            BookAuthor bookAuthor = _db.BookAuthors.FirstOrDefault(u => u.AuthorID == authorId && u.BookID == bookID);
            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();
            return RedirectToActionPermanent(nameof(ManageAuthors), new { @id = bookID });
        }

        public IActionResult PlayGround()
        {
            var bookTemp = _db.Books.FirstOrDefault();
            bookTemp.Price = 100;

            var bookCollection = _db.Books;
            double totalPrice = 0;

            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            var bookList = _db.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            var bookCollection2 = _db.Books;
            var bookCount1 = bookCollection2.Count();

            var bookCount2 = _db.Books.Count();

            IEnumerable<Book> BookList1 = _db.Books;
            var filteredBook1 = BookList1.Where(b => b.Price > 500).ToList();

            IQueryable<Book> BookList2 = _db.Books;
            var filteredBook2 = BookList2.Where(b => b.Price > 500).ToList();

            //Updating related data

            var bookTmp1 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_ID == 4);
            bookTmp1.BookDetail.NumberOfChapters = 2222;

            var bookTmp2 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_ID == 4);
            bookTmp2.BookDetail.Weigth = 3333;

            _db.Books.Update(bookTmp1);
            _db.SaveChanges();

            _db.Books.Attach(bookTmp2); //attach solo actualiza props cambiadas
            _db.SaveChanges();

            //Views
            var viewList = _db.BookDetailsFromViews.ToList();
            var viewList2 = _db.BookDetailsFromViews.FirstOrDefault();
            var viewList3 = _db.BookDetailsFromViews.Where( u => u.Price > 500);


            //raw sql
            var BookRaw = _db.Books.FromSqlRaw("Select * from dbo.books").ToList();
            int id = 1;
            var BookRaw2 = _db.Books.FromSqlInterpolated($"Select * from dbo.books where Book_ID = {id}").ToList();
            var bookSproc = _db.Books.FromSqlInterpolated($"EXEC dbo.getAllBookDetails {id}").ToList();


            var filter1 = _db.Books.Include(e => e.BookAuthors.Where(p => p.AuthorID == 5)).ToList();
            var filter2 = _db.Books.Include(e => e.BookAuthors.OrderByDescending(p => p.AuthorID).Take(2)).ToList();
            return RedirectToActionPermanent(nameof(Index));
        }
    }
}
