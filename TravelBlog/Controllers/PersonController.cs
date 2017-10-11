using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelBlog.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelBlog.Controllers
{
    public class PersonController : Controller
    {
        // GET: /<controller>/
        private TravelBlogContext db = new TravelBlogContext();
        public IActionResult Index()
        {
            return View(db.People.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Locations = new SelectList(db.Locations, "LocationId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Person Person, int locationId)
        {
            db.People.Add(Person);
            db.SaveChanges();
            return RedirectToAction("Index", "Location");
        }

        public IActionResult AddLocation(int PersonId)
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name");
            ViewBag.Person = db.People.FirstOrDefault(x => x.PersonId == PersonId);
            return View();
        }

        [HttpPost]
        public IActionResult AddLocation(LocationPerson locationPerson)
        {
            db.LocationPerson.Add(locationPerson);
            db.SaveChanges();
            return RedirectToAction("Index", "Location");
        }
        public IActionResult Delete(int id)
        {
            var myPerson = db.People.FirstOrDefault(People => People.PersonId == id);
            return View(myPerson);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var myPerson = db.People.FirstOrDefault(People => People.PersonId == id);
            db.Remove(myPerson);
            db.SaveChanges();
            return RedirectToAction("Index", "Location");
        }

        public IActionResult Update(int id)
        {
            var myPerson = db.People.FirstOrDefault(People => People.PersonId == id);
            ViewBag.Id = new SelectList(db.People, "Id", "Name", "Email");
            return View(myPerson);
        }
        [HttpPost]
        public IActionResult Update(Person Person)
        {
            db.Entry(Person).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var myPerson = db.People
                    .Include(person => person.LocationPerson)
                    .ThenInclude(join => join.Location)
                .FirstOrDefault(People => People.PersonId == id);

            return View(myPerson);
        }
    }
}
