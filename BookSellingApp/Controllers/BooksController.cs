using BookSellingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookSellingApp.Controllers
{

    [Authorize(Policy = "readonlypolicy")]
     
    public class BooksController : Controller
    {
        public readonly IWebHostEnvironment _webHostEnvironment;

        Uri baseAddress = new Uri("https://ebookstoreapi.azurewebsites.net");
        HttpClient client;

        public BooksController(IWebHostEnvironment webHostEnvironment)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            _webHostEnvironment = webHostEnvironment;
        }

         public static List<Book>? bookList = new List<Book>();
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            HttpResponseMessage responseMessage =  client.GetAsync(baseAddress + "api/books/GetBooks").Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string data =  responseMessage.Content.ReadAsStringAsync().Result;
                bookList = JsonConvert.DeserializeObject<List<Book>>(data);
                return View("Index",bookList);
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View("AddBook");
        }

        public static List<Book>? cartList = new List<Book>();

        public ActionResult Cartlist()
        {
            return View("AddToCart",cartList);
        }
        public ActionResult AddToCart(int id)
        {

            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "api/books/GetBook/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                var book = JsonConvert.DeserializeObject<Book>(data);
                if(book != null)
                    cartList.Add(book);
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    return View("AddToCart");
                }
                    
            }  
            return View("AddToCart", cartList);

        }
        public ActionResult RemoveFromCart(int id)
        {
            var book=cartList.Where(e=>e.Id==id)
                .FirstOrDefault();
            cartList.Remove(book);
            return View("AddToCart", cartList);
        }
        // GET: BooksController/Create
        [Authorize(Policy = "writepolicy")]
        public ActionResult AddBook()
        {
            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(Book book)
        {
            var name = bookList.Where(b => b.Name == book.Name).FirstOrDefault();
            if (name != null)
            {
                return View("BookValidationView",book);
            }
            if(book.Image != null)
            {
                string folder = "Book/Images/";
                folder += Guid.NewGuid().ToString() + "_" + book.Image.FileName;
                book.ImageUrl = folder;
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath,folder);
                 book.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create)); 
            }
            var postTask = client.PostAsJsonAsync<Book>(baseAddress + "api/books/PostBook", book);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View("book");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SearchBook(string searchByNameOrZoner)
        {
            ViewData["BookInfo"] = searchByNameOrZoner;
            List<Book>? bookList = new List<Book>();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + $"api/books/Search/{searchByNameOrZoner}").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                bookList = JsonConvert.DeserializeObject<List<Book>>(data);
            }
            return View("Index", bookList);
        }

        // GET: BooksController/Details/5
        public ActionResult Details(int id)
        {
            Book? book = new Book();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "api/books/GetBook/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(data);
            }
            return View(book);
        }

        // GET: BooksController/Edit/5
        [Authorize(Policy = "writepolicy")]
        [HttpGet]
        public ActionResult UpdateBooks(int id)
        {
            Book? book = new Book();
           
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "api/books/GetBook/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(data);
            }
            return View(book);
        }

   
        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBooks(Book book)
        {

            var name = bookList.Where(b => b.Name == book.Name).FirstOrDefault();
            if (name != null)
            {
                return View("BookValidationView", book);
            }
            if (book.Image != null)
            {
                string folder = "Book/Images/";
                folder += Guid.NewGuid().ToString() + "_" + book.Image.FileName;
                book.ImageUrl = folder;
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                book.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            var putTask = client.PutAsJsonAsync<Book>(baseAddress + "api/books/PutBook/" + book.Id.ToString(), book);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [Authorize(Policy = "writepolicy")]
        public ActionResult DeleteBook(int id)
        {

            //HTTP DELETE
            var deleteTask = client.DeleteAsync(baseAddress + "api/books/DeleteBook/" + id);
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
