using WeddingPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class WeddingsController : Controller
{
    private int? uid 
    {
        get 
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }
    private bool loggedIn{
        get
        {
            return uid != null;
        }
    }
    private MyContext db;
    public WeddingsController(MyContext context)
    {
        db = context;
    }

    [HttpGet("/weddings/new")]
    public IActionResult New()
    {
        ViewBag.allChefs = db.Users.ToList();
        return View("New");
    }

    [HttpPost("/weddings/create")]
    public IActionResult Create(Wedding newWedding)
    {
        if(ModelState.IsValid)
        {
        newWedding.UserId = (int)uid;
        db.Add(newWedding);
        db.SaveChanges();

        return RedirectToAction("All");
        }
        ViewBag.allWeddings = db.Users.ToList();
        return New();
    }

    [HttpGet("/weddings")]
    public IActionResult All()
    {
                if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        List<Wedding> allWeddings = db.Weddings.Include(p => p.Planner).Include(p => p.WeddingRsvp).ToList();

        return View("All", allWeddings);
    }

    [HttpGet("/weddings/{oneWeddingId}")]
    public IActionResult GetOneWedding(int oneWeddingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Wedding? wedding = db.Weddings
        .Include(d => d.Planner)
        .Include(d => d.WeddingRsvp)
        .ThenInclude(wr => wr.User)
        .FirstOrDefault(d => d.WeddingId == oneWeddingId); 

        if (wedding == null)
        {
            return RedirectToAction("Index");
        }

        return View("One", wedding);
    }

    [HttpGet("/weddings/{deletedWeddingId}/delete")]
    public IActionResult DeleteWedding(int deletedWeddingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Wedding? wedding = db.Weddings.FirstOrDefault(d => d.WeddingId == deletedWeddingId); 

        if (wedding != null && wedding.UserId == uid)
        {   
        db.Weddings.Remove(wedding);
        db.SaveChanges();
        return RedirectToAction("All");     
        }
        return RedirectToAction("All");
    }

    [HttpGet("/weddings/{weddingId}/edit")]
    public IActionResult Edit(int weddingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Wedding? wedding = db.Weddings.FirstOrDefault(d => d.WeddingId == weddingId); 

        if(wedding == null)
        {
            return Redirect("/");
        }
        return View("Edit", wedding);
    }

    [HttpPost("/weddings/{weddingId}/update")]
    public IActionResult Update(Wedding editedWedding, int weddingId)
    {
        if (ModelState.IsValid == false)
        {
            return Edit(weddingId);
        }

        Wedding? dbWedding = db.Weddings.FirstOrDefault(d => d.WeddingId == weddingId); 

        if (dbWedding == null)
        {
            return RedirectToAction("All");
        }

        dbWedding.WedderOne = editedWedding.WedderOne;
        dbWedding.WedderTwo = editedWedding.WedderTwo;
        dbWedding.Date = editedWedding.Date;
        dbWedding.Address = editedWedding.Address;
        dbWedding.UpdatedAt = DateTime.Now;

        db.Weddings.Update(dbWedding);
        db.SaveChanges();

        return RedirectToAction("GetOneWedding", new { weddingId = dbWedding.WeddingId });

    }

    [HttpPost("/weddings/{weddingId}/rsvp")]
    public IActionResult Reserve(int weddingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Rsvp? existingRsvp = db.Rsvps.FirstOrDefault(r => r.WeddingId == weddingId && r.UserId == (int)uid);

        if (existingRsvp == null)
        {
            Rsvp newRsvp = new Rsvp()
            {
                UserId = (int)uid,
                WeddingId = weddingId
            };
            db.Rsvps.Add(newRsvp);
        } 
        else
        {
            db.Rsvps.Remove(existingRsvp);
        }
        db.SaveChanges();
        return RedirectToAction("All");
    }
}