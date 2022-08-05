using BookSellingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookSellingApp.Controllers
{
    public class BooksController : Controller
    {

        Uri baseAddress = new Uri("https://localhost:7242/api");
        HttpClient client;

        public BooksController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

         public static List<Book>? bookList = new List<Book>();
        public ActionResult Index()
        {
            
            HttpResponseMessage responseMessage =  client.GetAsync(baseAddress + "/books/GetBooks").Result;

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
        public ActionResult AddToCart(int id)
        {

            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/books/GetBook/" + id.ToString()).Result;
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
        public ActionResult RemoveFromCart(Book book)
        {
            cartList.Remove(book);
            return View("AddToCart", cartList);
        }
        // GET: BooksController/Create
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
           
            var postTask = client.PostAsJsonAsync<Book>(baseAddress + "/books/PostBook", book);
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
        public ActionResult SearchBook(string searchByNameOrZoner)
        {
            ViewData["BookInfo"] = searchByNameOrZoner;
            List<Book>? bookList = new List<Book>();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + $"/books/Search/{searchByNameOrZoner}").Result;
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
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/books/GetBook/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(data);
            }
            return View(book);
        }

        // GET: BooksController/Edit/5
        [HttpGet]
        public ActionResult UpdateBooks(int id)
        {
            Book? book = new Book();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/books/GetBook/" + id.ToString()).Result;
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
            var putTask = client.PutAsJsonAsync<Book>(baseAddress + "/books/PutBook/" + book.Id.ToString(), book);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(book);
        }


        //// GET: BooksController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        public ActionResult DeleteBook(int id)
        {

            //HTTP DELETE
            var deleteTask = client.DeleteAsync(baseAddress + "/books/DeleteBook/" + id);
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
