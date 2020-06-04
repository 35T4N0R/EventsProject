using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ap1.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.IO;
using System.Net.Mime;

namespace ap1.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AccountHome()
        {

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var user = dc.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();

                return View(user);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountHome(User user)
        {

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var us = dc.Users.Where(a => a.UserId == user.UserId).FirstOrDefault();
                us.FirstName = user.FirstName;
                us.LastName = user.LastName;
                us.DateOfBirth = user.DateOfBirth;
                us.NIP = user.NIP;
                dc.Configuration.ValidateOnSaveEnabled = false;
                dc.SaveChanges();
                return View(us);
            }

        }

        public ActionResult AccountEvent()
        {
            List<Event> events = new List<Event>();

            var userName = HttpContext.User.Identity.Name;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var user = db.Users.Where(x => x.EmailID == userName).FirstOrDefault();

                foreach (var item in db.Enrollments)
                {
                    if (item.UserId == user.UserId)
                    {

                        Event e = new Event();
                        var ev = db.Events.Where(x => x.EventId == item.EventId).FirstOrDefault();
                        if (ev.OrganiserEmail == user.EmailID)
                        {
                            e.EventId = ev.EventId;
                            e.Date = ev.Date;
                            e.ExpectedTicketAmount = ev.ExpectedTicketAmount;
                            e.Localization = ev.Localization;
                            e.MaxTicketAmountForOnePerson = ev.MaxTicketAmountForOnePerson;
                            e.Name = ev.Name;
                            e.Type = ev.Type;
                            e.Time = ev.Time;
                            events.Add(e);
                        }
                    }
                }
            }

            return View(events);
        }

        public ActionResult AccountUsersEvents()
        {

            List<Event> events = new List<Event>();

            var userName = HttpContext.User.Identity.Name;

            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var user = db.Users.Where(x => x.EmailID == userName).FirstOrDefault();

                foreach (var item in db.Enrollments)
                {
                    if (item.UserId == user.UserId)
                    {
                        Event e = new Event();
                        var ev = db.Events.Where(x => x.EventId == item.EventId).FirstOrDefault();
                        if (ev.OrganiserEmail != user.EmailID) {
                            e.EventId = ev.EventId;
                            e.Date = ev.Date;
                            e.ExpectedTicketAmount = ev.ExpectedTicketAmount;
                            e.Localization = ev.Localization;
                            e.MaxTicketAmountForOnePerson = ev.MaxTicketAmountForOnePerson;
                            e.Name = ev.Name;
                            e.Type = ev.Type;
                            e.Time = ev.Time;
                            events.Add(e);
                        }

                    }
                }
            }

            return View(events);
        }

        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerf,ActivationCode")] User user)
        {

            bool Status = false;
            string message = "";
            //Model Validication
            if (ModelState.IsValid)
            {

                #region Checking if Email exist
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "E-mail już istnieje!");
                    return View(user);
                }
                #endregion
                #region generate activation link
                user.ActivationCode = Guid.NewGuid();
                #endregion
                #region Hashing password
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerf = false;
                #region Save to DataBase
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                    #region Sending email
                    SendVerfLink(user.EmailID, user.ActivationCode.ToString());
                    message = "Rejestracja przebiegła pomyślnie. Link aktywacyjny został wysłany na " +
                        "twoje konto email!";
                    Status = true;
                    #endregion
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // fix problem with confirm password

                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerf = true;
                    dc.SaveChanges();
                    Status = true;

                }
                else
                {
                    ViewBag.Message = "Invalid request";
                }

                ViewBag.Status = Status;
                return View();
            }

        }


        //Login

        public ActionResult Login() {
            return View();
        }

        // PDF
        public ActionResult PDF() {
            return View();
        }


        // Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnURL = "") {

            string message = "";
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnURL))
                        {
                            return Redirect(ReturnURL);
                        }
                        else
                        {



                            return RedirectToAction("Index", "Events");
                        }

                    }
                    else
                    {
                        message = "Hasło nieprawidłowe!";
                    }

                }
                else
                {
                    message = "Brak użytkownika o takim Emailu";
                }
            }

            ViewBag.Message = message;
            return View();
        }
        //Logout

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Users");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;

            }
        }

        [NonAction]
        public void SendVerfLink(string emailID, string activationCode, string emailFo = "VerifyAccount")
        {
            var url = "/Users/" + emailFo + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
            var fromEmail = new MailAddress("funfunpodlasie@gmail.com", "Wydarzenia");
            var toEmail = new MailAddress(emailID);
            var password = "admin1234.";
            string sub = "";
            string body = "";

            if (emailFo == "VerifyAccount")
            {

                sub = "Twoje konto zostało utworzone pomyślnie!";
                body = "<br/><br/> Informujemy że Twoje konto zostało utworzone pomyślnie. " +
                     "Proszę, naciśnij w link poniżej aby aktywować swoje konto" +
                     "<br/><br/><a href ='" + link + "'>" + link + "</a> ";

            }
            else if (emailFo == "ResetPassword")
            {

                sub = "Zresetuj swój adres Email!";
                body = "<br/><br/> Otrzymaliśmy prośbę o zmianę Twojego hasła " +
                     "Proszę, naciśnij w link poniżej aby zresetować swoje hasło" +
                     "<br/><br/><a href =" + link + "> Zresetuj!</a> ";

            }


            var smtp = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, password)


            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {

            string message = "";
            bool status = false;

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var account = dc.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                if (account != null)
                {
                    // sending email
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerfLink(account.EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();

                    status = true;
                    message = "Wysłaliśmy na Twoj adres link do zresetwoania hasła!";
                }
                else {
                    message = "W bazie nie ma takiego E-maila :(";
                }
            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();

                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else {
                    return HttpNotFound();
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ResetPassword(ResetPasswordModel model) {

            var message = "";
            if (ModelState.IsValid) {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "hasło zostało zmienione!";
                        ViewBag.Status = true;
                        ViewBag.Message = message;
                        return RedirectToAction("Login", "Users");
                    }
                    else
                    {

                        message = "Coś jest źle!";
                    }
                }
            }

            ViewBag.Message = message;

            return View(model);
        }

        public ActionResult DownloadPDF(int eventId)
        {
            CreateDocument(eventId);
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Ticket Example.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(TempData["PDF"]);
            Response.End();
            return View();
        }

        public MemoryStream CreateDocument(int eventId)
        {
            string CurrentUser = HttpContext.User.Identity.Name;

            MyDatabaseEntities db = new MyDatabaseEntities();

            var user = db.Users.Where(a => a.EmailID == CurrentUser).FirstOrDefault();
            var Event = db.Events.Where(a => a.EventId == eventId).FirstOrDefault();

            MemoryStream output = new MemoryStream();
            #region Creating PDF
            Chunk chunk; Paragraph line; PdfPTable table; PdfPCell cell; Image image;

            Document pdf = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriterMail = PdfWriter.GetInstance(pdf, output);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdf, Response.OutputStream);
            pdf.Open();

            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            cell = new PdfPCell();
            cell.Border = 0;
            image = Image.GetInstance(Server.MapPath("~\\ticket_example.jpg"));
            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);

            chunk = new Chunk($"{Event.Name}", FontFactory.GetFont("Arial", 30, Font.NORMAL, BaseColor.RED));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            pdf.Add(table);

            chunk = new Chunk("Event's Informations", FontFactory.GetFont("Arial", 15, Font.BOLDITALIC, BaseColor.GREEN));
            pdf.Add(chunk);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_RIGHT, 1)));
            pdf.Add(line);

            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            chunk = new Chunk($"EventId :{Event.EventId} \nType: {Event.Type}", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            TimeSpan time = (TimeSpan)Event.Time;

            chunk = new Chunk($"Date: {Event.Date.ToString("dd.MM.yyyy")}\nTime: {time.Hours}:{time.Minutes} \nLocalization: {Event.Localization}", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            pdf.Add(table);

            chunk = new Chunk("Participant's Personal Informations", FontFactory.GetFont("Arial", 15, Font.BOLDITALIC, BaseColor.GREEN));
            pdf.Add(chunk);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdf.Add(line);

            table = new PdfPTable(1);
            table.WidthPercentage = 50;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            chunk = new Chunk($"UserId :{user.UserId} \nName: {user.FirstName} \nLast Name: {user.LastName}", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            pdf.Add(table);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdf.Add(line);

            QRCodeMaker qrcm = new QRCodeMaker(Convert.ToInt32(Event.EventId), Event.Type, Event.Name, Event.Localization, Event.Date, time);
            byte[] byteImage = qrcm.GenerateQRCode();
            image = Image.GetInstance(byteImage);
            image.ScaleAbsolute(100, 100);
            image.Alignment = 2;
            pdf.Add(image);

            pdfWriter.CloseStream = false;
            pdfWriterMail.CloseStream = false;
            pdf.Close();
            TempData["PDF"] = pdf;
            #endregion

            output.Position = 0;

            return output;

        }

        public ActionResult SendPDFToEmail(int eventId)
        {

            var fromEmail = new MailAddress("funfunpodlasie@gmail.com", "Wydarzenia");
            var toEmail = new MailAddress(HttpContext.User.Identity.Name);
            var password = "admin1234.";
            string sub = "BILET";
            string body = "<br/><br/> W załączniku został wysłany twój bilet. " +
                "Administracja życzy udanej zabawy podczas wydarzenia. :)";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, password)
            };

            using (var message = new MailMessage(fromEmail, toEmail))
            {

                MemoryStream file = new MemoryStream(CreateDocument(eventId).ToArray());

                file.Seek(0, SeekOrigin.Begin);
                Attachment data = new Attachment(file, "Ticket Example.pdf", "application/pdf");
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = DateTime.Now;
                disposition.ModificationDate = DateTime.Now;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                message.Attachments.Add(data);

                message.Subject = sub;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }

            return Redirect("AccountUsersEvents");
        }
        public ActionResult EnrollQRCode(int userId, int eventId, int ticketAmount)
        {
            QRCodeMaker qrcm = new QRCodeMaker(userId, eventId, ticketAmount);

            byte[] bytes = qrcm.GenerateQRCode();
            ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(bytes);

            return View("PDF");
        }

        public ActionResult OrganiserEmailSending()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrganiserEmailSending(String Title, String Body, int eventId)
        {
            List<String> uczestnicy = new List<String>();

            var fromEmail = new MailAddress("funfunpodlasie@gmail.com", "Wydarzenia");
            string body;
            using (MyDatabaseEntities db = new MyDatabaseEntities())
            {
                var organizator = db.Users.Where(x => x.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                foreach(var en in db.Enrollments)
                {
                    if(en.EventId == eventId)
                    {
                        uczestnicy.Add(db.Users.Where(x => x.UserId == en.UserId).FirstOrDefault().EmailID);
                    }
                }
            body = "Numer Id Wydarzenia: " + eventId+"<br/>"+"Nazwa Wydarzenia: " + db.Events.Where(x => x.EventId == eventId).FirstOrDefault().Name + "<br/>" + "Imię i Nazwisko Organizatora Wydarzenia: " + organizator.FirstName + " " + organizator.LastName + " <br/><br/>" + Body;
            }
            var password = "admin1234.";
            string sub = Title;

            var smtp = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, password)


            };
            for (int i = 0; i < uczestnicy.Count(); i++)
            {
                var toEmail = new MailAddress(uczestnicy[i]);

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = sub,
                    Body = body,
                    IsBodyHtml = true
                })

                    smtp.Send(message);
            }


            return RedirectToAction("AccountEvent","Users");
        }
    }
}