using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ap1.Models;

namespace ap1.Controllers
{
    public class EventsController : Controller
    {
       
        public ActionResult Index()
        {
            List<string> ListaMiast = new List<string>();
            ListaMiast.Add("Wszystkie");
            ListaMiast.Add("Augustów");
            ListaMiast.Add("Bialystok");
            ListaMiast.Add("Choroszcz");
            ListaMiast.Add("Chrzanowo");
            ListaMiast.Add("Dotnetowo");
            ListaMiast.Add("Katowice");
            ListaMiast.Add("Kieliszkowo");
            ListaMiast.Add("Kraków");
            ListaMiast.Add("Lublin");
            ListaMiast.Add("Łódz");
            ListaMiast.Add("Opole");
            ListaMiast.Add("Siedlce");
            ListaMiast.Add("Suwalki");
            ListaMiast.Add("Warszawa");
            ListaMiast.Add("Wroclaw");
            ListaMiast.Add("Zakopane");
            ViewBag.Miasta = new SelectList(ListaMiast);

            MyDatabaseEntities db = new MyDatabaseEntities();

            Serch sr = new Serch();
            var przedSortem = db.Events.ToList();
            var posortowana = new List<Event>();
            DateTime dt;
            Event item; 
            while(przedSortem.Count()>0)
            {
                item = przedSortem.First();
                dt = item.Date;

                foreach (Event j in przedSortem)
                {
                    if(DateTime.Compare(j.Date,dt) < 0)
                    {
                        item = j;
                        dt = j.Date;
                        
                    }
                    
                }
                przedSortem.Remove(item);
                posortowana.Add(item);
            }

            sr.list = posortowana;
            return View(sr);
        }

        [HttpPost]
        public ActionResult Index(Serch model)
        {
            List<string> ListaMiast = new List<string>();
            ListaMiast.Add("Wszystkie");
            ListaMiast.Add("Augustów");
            ListaMiast.Add("Bialystok");
            ListaMiast.Add("Choroszcz");
            ListaMiast.Add("Chrzanowo");
            ListaMiast.Add("Dotnetowo");
            ListaMiast.Add("Katowice");
            ListaMiast.Add("Kieliszkowo");
            ListaMiast.Add("Kraków");
            ListaMiast.Add("Lublin");
            ListaMiast.Add("Łódz");
            ListaMiast.Add("Opole");
            ListaMiast.Add("Siedlce");
            ListaMiast.Add("Suwalki");
            ListaMiast.Add("Warszawa");
            ListaMiast.Add("Wroclaw");
            ListaMiast.Add("Zakopane");
            ViewBag.Miasta = new SelectList(ListaMiast);

            MyDatabaseEntities db = new MyDatabaseEntities();

            Serch sr = new Serch();
            var przedSortem = db.Events.ToList();
            var posortowana = new List<Event>();
            DateTime dt;
            Event item;
            while (przedSortem.Count() > 0)
            {
                item = przedSortem.First();
                dt = item.Date;

                foreach (Event j in przedSortem)
                {
                    if (DateTime.Compare(j.Date, dt) < 0)
                    {
                        item = j;
                        dt = j.Date;

                    }

                }
                przedSortem.Remove(item);
                posortowana.Add(item);
            }

            var nowalista = new List<Event>();
            sr.Localization = model.Localization;
            sr.MinDate = model.MinDate;
            sr.MaxDate = model.MaxDate;
            foreach(Event ite in posortowana)
            {
                if (model.Localization == "Wszystkie")
                {
                    if (sr.MinDate==sr.MaxDate) {
                        nowalista.Add(ite);
                    }
                    else if (DateTime.Compare(ite.Date, model.MinDate)>0 && DateTime.Compare(ite.Date,model.MaxDate)<0)
                    {
                        nowalista.Add(ite);
                    }

                }
                else if(ite.Localization == model.Localization)
                {
                    if (sr.MinDate == sr.MaxDate)
                    {
                        nowalista.Add(ite);
                    }
                    else if (DateTime.Compare(ite.Date, model.MinDate) > 0 && DateTime.Compare(ite.Date, model.MaxDate) < 0)
                    {
                        nowalista.Add(ite);
                    }


                }

               
            }

            sr.list = nowalista;
            return View(sr);
        }


        public ActionResult AddEvent()
        {
            List<string> ListaTypow = new List<string>();
            ListaTypow.Add("platne");
            ListaTypow.Add("bezplatne");
            ViewBag.TypyWydarzen = new SelectList(ListaTypow);

            List<string> ListaMiast = new List<string>();
            ListaMiast.Add("Augustów");
            ListaMiast.Add("Bialystok");
            ListaMiast.Add("Choroszcz");
            ListaMiast.Add("Chrzanowo");
            ListaMiast.Add("Dotnetowo");
            ListaMiast.Add("Katowice");
            ListaMiast.Add("Kieliszkowo");
            ListaMiast.Add("Kraków");
            ListaMiast.Add("Lublin");
            ListaMiast.Add("Łódz");
            ListaMiast.Add("Opole");
            ListaMiast.Add("Siedlce");
            ListaMiast.Add("Suwalki");
            ListaMiast.Add("Warszawa");
            ListaMiast.Add("Wroclaw");
            ListaMiast.Add("Zakopane");
            ViewBag.Miasta = new SelectList(ListaMiast);
            return View();
        }

        [HttpPost]
        public ActionResult AddEvent(Event model)
        {
            List<string> ListaTypow = new List<string>();
            ListaTypow.Add("platne");
            ListaTypow.Add("bezplatne");
            ViewBag.TypyWydarzen = new SelectList(ListaTypow);

            List<string> ListaMiast = new List<string>();
            ListaMiast.Add("Augustów");
            ListaMiast.Add("Bialystok");
            ListaMiast.Add("Choroszcz");
            ListaMiast.Add("Chrzanowo");
            ListaMiast.Add("Dotnetowo");
            ListaMiast.Add("Katowice");
            ListaMiast.Add("Kieliszkowo");
            ListaMiast.Add("Kraków");
            ListaMiast.Add("Lublin");
            ListaMiast.Add("Łódz");
            ListaMiast.Add("Opole");
            ListaMiast.Add("Siedlce");
            ListaMiast.Add("Suwalki");
            ListaMiast.Add("Warszawa");
            ListaMiast.Add("Wroclaw");
            ListaMiast.Add("Zakopane");
            ViewBag.Miasta = new SelectList(ListaMiast);
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {

                var isExist = IsEventExists(model.Name);
                if (isExist)
                {
                    ModelState.AddModelError("NameExist", "Taka nazwa już istnieje!");
                    return View(model);
                }

                if (DateTime.Compare(model.Date, DateTime.Now) < 0)
                {
                    ModelState.AddModelError("ToErly", "Nie wiedziałem, że potrafisz się cofać w czasie");
                    return View(model);
                }

                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    model.OrganiserEmail = HttpContext.User.Identity.Name;
                    db.Events.Add(model);
                    db.SaveChanges();

                }
                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {

                    int eveID = db.Events.Max(a => a.EventId);
                    var eve = db.Events.Where(a => a.EventId == eveID).FirstOrDefault();
                    var user = db.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();

                    Enrollment env = new Enrollment();

                    env.User = user;
                    env.Event = eve;
                    db.Enrollments.Add(env);

                    db.SaveChanges();
                    message = "Dodanie nowego Eventu powiodło się!";
                    Status = true;
                }
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Status = Status;
            ViewBag.Message = message;
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitEvent(Event model) //dodawanie eventu do bazy
        {
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult EditEvent(int EventId)
        {
            List<string> ListaTypow = new List<string>();
            ListaTypow.Add("platne");
            ListaTypow.Add("bezplatne");
            ViewBag.TypyWydarzen = new SelectList(ListaTypow);

            List<string> ListaMiast = new List<string>();
            ListaMiast.Add("Augustów");
            ListaMiast.Add("Bialystok");
            ListaMiast.Add("Choroszcz");
            ListaMiast.Add("Chrzanowo");
            ListaMiast.Add("Dotnetowo");
            ListaMiast.Add("Katowice");
            ListaMiast.Add("Kieliszkowo");
            ListaMiast.Add("Kraków");
            ListaMiast.Add("Lublin");
            ListaMiast.Add("Łódz");
            ListaMiast.Add("Opole");
            ListaMiast.Add("Siedlce");
            ListaMiast.Add("Suwalki");
            ListaMiast.Add("Warszawa");
            ListaMiast.Add("Wroclaw");
            ListaMiast.Add("Zakopane");
            ViewBag.Miasta = new SelectList(ListaMiast);


            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
               Event eve = new Event();
               eve = db.Events.Where(a => a.EventId == EventId).FirstOrDefault();
               return View(eve);
              
            }
            
        }

        [HttpPost]
        public ActionResult EditEvent(Event model)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                List<string> ListaTypow = new List<string>();
                ListaTypow.Add("platne");
                ListaTypow.Add("bezplatne");
                ViewBag.TypyWydarzen = new SelectList(ListaTypow);

                List<string> ListaMiast = new List<string>();
                ListaMiast.Add("Augustów");
                ListaMiast.Add("Bialystok");
                ListaMiast.Add("Choroszcz");
                ListaMiast.Add("Chrzanowo");
                ListaMiast.Add("Dotnetowo");
                ListaMiast.Add("Katowice");
                ListaMiast.Add("Kieliszkowo");
                ListaMiast.Add("Kraków");
                ListaMiast.Add("Lublin");
                ListaMiast.Add("Łódz");
                ListaMiast.Add("Opole");
                ListaMiast.Add("Siedlce");
                ListaMiast.Add("Suwalki");
                ListaMiast.Add("Warszawa");
                ListaMiast.Add("Wroclaw");
                ListaMiast.Add("Zakopane");
                ViewBag.Miasta = new SelectList(ListaMiast);
                bool Status = false;
                string message = "";

                

                 var isExist = IsEventExists(model.Name);
                var eve = db.Events.Where(a => a.EventId == model.EventId).FirstOrDefault();
                if (isExist && eve.Name != model.Name )
                    {
                        ModelState.AddModelError("NameExist", "Taka nazwa już istnieje!");
                        return View(model);
                    }
                    else
                    {
                        eve.Date = model.Date;
                        eve.ExpectedTicketAmount = model.ExpectedTicketAmount;
                        eve.Localization = model.Localization;
                        eve.MaxTicketAmountForOnePerson = model.MaxTicketAmountForOnePerson;
                        eve.Name = model.Name;
                        eve.Time = model.Time;
                        eve.Type = model.Type;
                        db.SaveChanges();
                       
                    }
                
               
                ViewBag.Status = Status;
                ViewBag.Message = message;
                return RedirectToAction("AccountEvent", "Users");

            }

        }

        public ActionResult Delete(int EventId)
        {
            try
            {
                MyDatabaseEntities db = new MyDatabaseEntities();
                Event ev = db.Events.Where(a => a.EventId == EventId).FirstOrDefault();
                foreach (var item in db.Enrollments)
                {
                    if(item.EventId == EventId)
                    {
                        db.Enrollments.Remove(item);
                    }
                }
                db.Events.Remove(ev);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AccountEvent","Users");
        }
        [HttpGet]
        public ActionResult Assign(int EventId)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var eve = db.Events.Where(a => a.EventId == EventId).FirstOrDefault();
                var user = db.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                bool status = false;
                Assign asig = new Assign();

                if (eve.Type == "platne")
                {
                    status = true;
                }

                asig.EmailId = User.Identity.Name;
                asig.Name = user.FirstName;
                asig.LastName = user.LastName;
                asig.NIP = user.NIP;
                asig.EventId = eve.EventId;

                ViewBag.isPlatne = status;
                return View(asig);
            }
        }

        [HttpPost]
        public ActionResult Assign(Assign model)
        {
            bool Status = false;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var eve = db.Events.Where(a => a.EventId == model.EventId).FirstOrDefault();
                var user = db.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();

                if (eve.Type == "platne")
                {
                    Status = true;
                }

                Enrollment env = new Enrollment();

                
               
                int counter = 0;
                if (eve.MaxTicketAmountForOnePerson >= model.Tickets) {
                    foreach (var a in db.Enrollments)
                    {
                        if (a.EventId == model.EventId)
                            counter += a.TicketAmount;
                    }

                    if ( eve.ExpectedTicketAmount >= model.Tickets + counter ) {
                        env.UserId = user.UserId;
                        env.EventId = model.EventId;
                        env.TicketAmount = model.Tickets;
                        db.Enrollments.Add(env);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Events", new { EventId = model.EventId });
                    }
                    else
                    {
                        ModelState.AddModelError("nullTickets", "brak biletów");
                    }
                }
                else
                {
                    ModelState.AddModelError("toManyTikets", "Nie możesz wziąć tylu biletów");
                }
                

               
            }

            ViewBag.isPlatne = Status;
            return View(model);
        }

        public ActionResult Details(int? EventId)
        {
            if(EventId == null)
            {
                return RedirectToAction("Unauthorized", "Events");
            }
            ViewBag.Assigned = false;
            ViewBag.IsOrganiser = false;
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                int count = 0;
                var eve = db.Events.Where(a => a.EventId == EventId).FirstOrDefault();
                if (User.Identity.IsAuthenticated)
                {
                    var user = db.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                    foreach (var item in db.Enrollments)
                    {
                        if(item.EventId == eve.EventId)
                        {
                            if (item.UserId == user.UserId)
                            {
                                ViewBag.Assigned = true;
                                if (eve.OrganiserEmail == user.EmailID)
                                {
                                    ViewBag.IsOrganiser = true;
                                }
                            }
                            count = item.TicketAmount;
                        }
                       
                    }
                }
                ViewBag.ilosc = eve.ExpectedTicketAmount - count;
                return View(eve);
            }
            
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult SignOut(int EventId, string redirect,string controler)
        {

            using(MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var currentUser = db.Users.Where(x => x.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                
                foreach(var en in db.Enrollments)
                {
                    if(en.UserId == currentUser.UserId && en.EventId == EventId)
                    {
                        db.Enrollments.Remove(en);
                        break;
                    }
                }
                db.SaveChanges();
            }
                return RedirectToAction(redirect, controler, new { EventId = EventId});
        }

        [NonAction]
        public bool IsEventExists(string name)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Events.Where(a => a.Name == name).FirstOrDefault();
                return v != null;

            }
        }
    }
}