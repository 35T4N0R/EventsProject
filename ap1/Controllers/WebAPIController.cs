using ap1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;

namespace ap1.Controllers
{
    public class WebAPIController : ApiController
    {

        //MyDatabaseEntities db = new MyDatabaseEntities();
        List<Event> events = new List<Event>();

        public WebAPIController()
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                foreach (Event e in db.Events)
                {
                    Event em = new Event();
                    em.EventId = e.EventId;
                    em.Date = e.Date.Date;
                    em.Time = e.Time;
                    em.ExpectedTicketAmount = e.ExpectedTicketAmount;
                    em.Localization = e.Localization;
                    em.MaxTicketAmountForOnePerson = e.MaxTicketAmountForOnePerson;
                    em.Name = e.Name;
                    em.Type = e.Type;
                    em.OrganiserEmail = e.OrganiserEmail;
                    events.Add(em);
                }
            }
        }

        // GET api/<controller>
        [Route("api/webapi")]
        public List<Event> Get()
        {

            return events;
        }

        // GET api/<controller>/id
        //Details of Event searched by Id of this event
        [Route("api/webapi/{id}")]
        public HttpResponseMessage Get(int id)
        {

            int TicketCounter = 0;
            int FreeTicketsAmount = 0;

            using(MyDatabaseEntities db = new MyDatabaseEntities())
            {
                try
                {
                    Event e = db.Events.Where(x => x.EventId == id).FirstOrDefault();

                    foreach (Enrollment en in db.Enrollments)
                    {
                        if(en.EventId == e.EventId)
                        {
                            TicketCounter += en.TicketAmount; ;
                        }
                    }

                    FreeTicketsAmount = e.ExpectedTicketAmount - TicketCounter;
                    TimeSpan time = (TimeSpan)e.Time;

                    return Request.CreateResponse(HttpStatusCode.OK, new { Name = e.Name, EventId = e.EventId, Localization = e.Localization, Date = $"{e.Date.Day}.{e.Date.Month}.{e.Date.Year}", Time = time, AmountOfEnrolledPeople = TicketCounter, AmountOfAllPlacesForFestival = e.ExpectedTicketAmount, AmountOfFreePlacesForFestival = FreeTicketsAmount}, JsonMediaTypeFormatter.DefaultMediaType);
                }
                catch(NullReferenceException)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound ,"Wydarzenie o podanym numerze Id nie istnieje w bazie danych.");
                }
            }
        }

        // GET api/<controller>/<localization>/StartDate/EndDate
        //Formatem daty jest dzień.miesiąc.rok - liczby są poodzielane kropkami
        [Route("api/webapi/{localization}/{StartDate}/{EndDate}")]
        [Route("api/webapi/{localization}/{StartDate}")]
        public HttpResponseMessage Get(String localization, String StartDate, String EndDate = null)
        {
            try
            {
                string[] s;
                string[] s2;
                DateTime startDate;
                DateTime endDate;
                List<Event> temp = new List<Event>();

                using (MyDatabaseEntities db = new MyDatabaseEntities())
                {
                    if (EndDate == null)
                    {
                        s = StartDate.Split('.');

                        startDate = new DateTime(Convert.ToInt32(s[2]), Convert.ToInt32(s[1]), Convert.ToInt32(s[0]));

                        foreach (Event e in events)
                        {
                            if ((e.Localization == localization) && (e.Date.Date >= startDate))
                            {
                                temp.Add(events.Where(x => x.EventId == e.EventId).FirstOrDefault());
                            }
                        }
                    }
                    else
                    {
                        s = StartDate.Split('.');
                        s2 = EndDate.Split('.');

                        startDate = new DateTime(Convert.ToInt32(s[2]), Convert.ToInt32(s[1]), Convert.ToInt32(s[0]));
                        endDate = new DateTime(Convert.ToInt32(s2[2]), Convert.ToInt32(s2[1]), Convert.ToInt32(s2[0]));

                        foreach (Event e in events)
                        {
                            if ((e.Localization == localization) && (e.Date.Date >= startDate) && (e.Date.Date <= endDate))
                            {
                                temp.Add(events.Where(x => x.EventId == e.EventId).FirstOrDefault());
                            }
                        }
                    }
                }
            
                return Request.CreateResponse(HttpStatusCode.OK, temp, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch(ArgumentOutOfRangeException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Proszę trzymać się realiów tego świata. Miesiąc może mieć maksymalnie 31 dni. W roku jest maksymalnie 12 miesięcy");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Zły format ciągu wejściowego. Następujący format jest prawidowy : dd.mm.yyyy. Gdzie dd - dzień, mm - miesiąc, yyyy - rok");
            }
        }

        // POST api/<controller>/<UserId>/<EventId>
        [Route("api/webapi/{userId}/{eventId}/{ticketAmount}")]
        public HttpResponseMessage Post(int userId, int eventId, int ticketAmount)
        {
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                try
                {
                    if(db.Events.Where(x => x.EventId == eventId).FirstOrDefault().MaxTicketAmountForOnePerson < ticketAmount)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Podana ilość biletów z jaką zapisuje się użytkownik jest większa od dopuszczalnej ilości biletów przypadającej na jednego uczestnika");
                    }
                    if((db.Enrollments.Where(x => x.UserId == userId && x.EventId == eventId).FirstOrDefault() == null))
                    {
                        Enrollment en = new Enrollment();
                        en.UserId = userId;
                        en.EventId = eventId;
                        en.TicketAmount = ticketAmount;
                        db.Enrollments.Add(en);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, new { Message = $"Użytkownik o Id {userId} został poprawnie zapisany na wydarzenie o Id {eventId}" }, JsonMediaTypeFormatter.DefaultMediaType);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { Message = $"Użytkownik o Id {userId} jest już zapisany na wydarzenie o Id {eventId}" }, JsonMediaTypeFormatter.DefaultMediaType);
                    }
                
                }catch (Exception)
                {
                    if (db.Users.Where(x => x.UserId == userId).FirstOrDefault() == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Użytkownika o podanym numerze Id nie ma w bazie danych");
                    }else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Wydarzenia o podanym adresie Id nie ma w bazie danych");
                    }
                }
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
