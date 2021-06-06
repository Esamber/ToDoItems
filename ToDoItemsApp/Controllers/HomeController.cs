using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoItemsApp.Models;

namespace ToDoItemsApp.Controllers
{
    public class HomeController : Controller
    {
            private string _connectionString =
                @"Data Source=.\sqlexpress;Initial Catalog=ToDoItems;Integrated Security=true;";

        public IActionResult Index()
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            IndexViewModel vm = new IndexViewModel()
            {
                IncompleteItems = db.GetIncompleteItems(),
                Categories = db.GetCategories()
            };
            return View(vm);
        }
        public IActionResult AddItem()
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            AddItemViewModel vm = new AddItemViewModel()
            {
                Categories = db.GetCategories()
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult SubmitItem(Item item)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            db.AddItem(item);
            return Redirect("/home/index");
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitCategory(Category category)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            db.AddCategory(category);
            return Redirect("/home/index");
        }
        public IActionResult MarkAsCompleted(int id)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            DateTime dateCompleted = DateTime.Now;
            db.MarkItemAsCompleted(id, dateCompleted);
            return Redirect("/home/index");
        }
        public IActionResult CompletedItems()
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            CompletedItemsViewModel vm = new CompletedItemsViewModel()
            {
                CompletedItems = db.GetCompletedItems(),
                Categories = db.GetCategories()
            };
            return View(vm);
        }
        public IActionResult Categories()
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            CategoriesViewModel vm = new CategoriesViewModel()
            {
                Categories = db.GetCategories()
            };
            return View(vm);
        }
        public IActionResult EditCategory(int id)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            EditCategoryViewModel vm = new EditCategoryViewModel()
            {
                Category = db.GetCategoryById(id)
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult SubmitEditedCategory(Category category)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            db.EditCategory(category);
            return Redirect("/home/index");
        }
        public IActionResult ItemsForCategory(int id)
        {
            ToDoItemsDb db = new ToDoItemsDb(_connectionString);
            GetByCategoryViewModel vm = new GetByCategoryViewModel()
            {
                ItemsByCategory = db.GetItemsByCategory(id),
                Category = db.GetCategoryById(id)
            };
            return View(vm);
        }
    }
}
