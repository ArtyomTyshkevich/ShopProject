using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShopProject.Data;
using ShopProject.Models;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace ShopProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _db;
        IWebHostEnvironment _env;
        public ProductDto productDtoTemp { get; set; } =new ProductDto();
        public Product ProductTemp { get; set; } = new Product();
        public HomeController(Context db, IWebHostEnvironment env)
        {
            _db= db;
            _env= env;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList= _db.products;
            return View(objProductList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductDto obj)
        {
            Product prodObj= new Product() { Name=obj.Name,Price=obj.Price,Brand=obj.Brand};
            string fileName ="";
            if (ModelState.IsValid && obj.ImageFile!=null)
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "productImage");
                fileName=Guid.NewGuid().ToString()+"_"+obj.ImageFile.FileName;
                string filepath = Path.Combine(uploadFolder,fileName);
                prodObj.ImageFileName=fileName;
                using (var stream = System.IO.File.Create(filepath))
                {
                    obj.ImageFile.CopyTo(stream);
                }

                _db.products.Add(prodObj);
                _db.SaveChanges();
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = _db.products.Find(id);

            if (productFromDb == null)
            {
                return NotFound();
            }
  
            var productDtoFromDb = new ProductDto() {Name=productFromDb.Name,Brand=productFromDb.Brand,Price=productFromDb.Price};
            ProductTemp = productFromDb;
            return View(productDtoFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductDto obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.ImageFile != null)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "productImage");
                    string fileName = Guid.NewGuid().ToString() + "_" + obj.ImageFile.FileName;
                    string filepath = Path.Combine(uploadFolder, fileName);
                    using (var stream = System.IO.File.Create(filepath))
                    {
                        obj.ImageFile.CopyTo(stream);
                    }
                    string uploadFolderOld = Path.Combine(_env.WebRootPath, "productImage", ProductTemp.ImageFileName);
                    System.IO.File.Delete(uploadFolderOld);

                    ProductTemp.ImageFileName= fileName;
                }
                ProductTemp.Name= obj.Name;
                ProductTemp.Price=obj.Price;
                ProductTemp.Brand=obj.Brand;
                
                _db.products.Update(ProductTemp);
                _db.SaveChanges();
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = _db.products.Find(id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if(id ==null)
            {
                return RedirectToAction("Index");
            }
            var obj = _db.products.Find(id);
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            string uploadFolder = Path.Combine(_env.WebRootPath, "productImage",obj.ImageFileName);
            System.IO.File.Delete(uploadFolder);
            _db.products.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}