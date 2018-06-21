using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {

         MovieDBContext db = new MovieDBContext();

        // GET: Movies
        /// <summary>
        /// Getting the movie list
        /// </summary>
        /// <param name="movieGenre"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult Index(string movieGenre, string searchString)
        {
            // Initializes a new instance of a list<T> class that is empty and has the dafault initial capacity
            var genreLst = new List<string>();

            var genreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            // Adds the elements of the specified collection to the end of the List<T>
            genreLst.AddRange(genreQry.Distinct());
            // Initializes the new instance of the SelectList class by using the specified items for the list
            ViewBag.MovieGenre = new SelectList(genreLst);
            var movies = from m in db.Movies
                         select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filters a sequence of values based on a predicate
                movies = movies.Where(m => m.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(m => m.Genre == movieGenre);
            }
            return View(movies);
        }

        /// <summary>
        /// Getting the details of the particular movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                //returns the instance of the HttpNotFoundResult class
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// Getting the movie which was created in the post method
        /// </summary>
        /// <returns></returns>
        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// creating the particular movie and specifying it's cost
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            // Gets a value that indicates whether this instance of the model-state dictionary is valid
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        /// <summary>
        /// get method for edit after editing in post method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Initializes a new instance of the HttpStatusCodeResult class using a status code
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Finds an entity with the given primary key values
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// post method for editing the movie details
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Gets or sets the state of the entity
                db.Entry(movie).State = EntityState.Modified;
                // Save all the changes made in this context to the underlying database
                db.SaveChanges();
                // Redirects to the specified action using the action name and the controller
                return RedirectToAction("Index","Movies");
            }
            // Creates a ViewResult Object by using the model that renders a view to the response
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Movie movie = db.Movies.Find(id);
            // Remove the entity from the database
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Calls the protected Dispose method
                db.Dispose();
            }
            // Releases unmanaged resources and optionally releases managed resources
            base.Dispose(disposing);
        }
    }
}