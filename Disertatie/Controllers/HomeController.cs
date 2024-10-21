using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disertatie.Mvc;
using Disertatie.Core;
using Disertatie.Models;
using System.Linq.Expressions;
using Disertatie.Helpers;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.ServiceModel.Channels;
using System.IO;
using Disertatie.Core.Repositories;
using Disertatie.Core.Models;
using System.Globalization;

namespace Disertatie.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [NHibernateSession]
        public ActionResult Index()
        {

            if (User.EsteMembru() || User.EsteCoordonatorColectiv() || User.EsteSefDepartament())
            {
                return RedirectToRoute("Membru_Home");
            }
            if (User.EsteAdministrator())
            {
                return RedirectToAction("Index", "Users", new { area = "Admin" });
            }
            
            ViewBag.Message = "Homepage cannot be found";
            return View();
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Membru()
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();

            model.Utilizator = user;
            var proiecteRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
            var proiecte = proiecteRep.QueryAll().Where(a => a.Initiator != user).OrderByDescending(a => a.DataModificareStare).Take(10).ToList();
            model.NoutatiProiecte = proiecte;
            var anunturiRep = new Disertatie.Core.Repositories.Repository<long, Anunt>();
            var anunt=anunturiRep.QueryAll().Where(a=> a.Activ==true).OrderByDescending(a => a.DataCreare ).Take(5).ToList();
            model.UltimeleAnunturi = anunt;
            return View(model);
        }
        [NHibernateSession]
        public ActionResult Organigrama()
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();

            model.Utilizator= user;
            model.MembriiDepartament = repo.GetMembriiDepartament(user.Username);
            model.TotiMembrii = repo.GetTotiMembriiDepartament();
            model.SefDepartament = repo.QueryAll(). Where(a => (a.Roluri.Any(r => r.NumeRol == "Sef departament") == true)).FirstOrDefault();
            model.CoordonatoriColetive = repo.QueryAll().Where(a => (a.Roluri.Any(r => r.NumeRol == "Coordonator colectiv")==true)).ToList();
            return View(model);
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult ProfilulMeu()
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();

            model.Utilizator = user;
            model.EstePaginaDeProfil = true;
            return View(model);
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
         [NHibernateSession]
         public ActionResult DetaliiMembru(long id)
         {
             var utilizatorRep = new UtilizatorRepository();
             var membru = utilizatorRep.Get(id);
             var model = new MembruHomeModel();
             var user = SecurityContext.CurrentUser as Utilizator;
             model.Utilizator = user;
             model.Membru = membru;
             return View(model);

         }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Mesaje(int? page1, int? page2, int? page3)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.MesajeNoi = repo.GetMesajeNoi(zeroBasePageIndex1,10,user);
            model.MesajePrimite = repo.GetMesajePrimite(zeroBasePageIndex2,10,user);
            model.MesajeTrimise = repo.GetMesajeTrimise(zeroBasePageIndex3,10,user);
            model.Utilizator = user;
            model.EstePaginaDeProfil = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Proiecte(int? page)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex, 10, user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [NHibernateSession]
        public ActionResult ModificaMembru(long id)
        {
            var s = new UtilizatorRepository();

            var utilizator = s.Get(id);
            var model = new Models.MembruHomeModel();
            model.Utilizator = utilizator;
            return View("ModificaMembru", model);
        }
        [NHibernateSession]
        [HttpPost]
        public ActionResult ModificaMembru(long id, string txtNume, string txtPrenume, string txtDataNasterii, string txtTelefon, string txtEmail, string ddlColectiv, string txtTitlu)
        {
            try
            {
               
                var u = new UtilizatorRepository();
                var colectivRep=new Repository<int,Colectiv>();
                var user = u.Get(id);
                user.Nume = txtNume;
                user.Prenume = txtPrenume;
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy";
                format.DateSeparator = "-";
                if (txtDataNasterii != null && txtDataNasterii != "")
                {
                    DateTime dataNasterii = Convert.ToDateTime(txtDataNasterii, format);
                    user.DataNasterii = dataNasterii;
                }
                else
                {
                    user.DataNasterii = (DateTime?)(null);

                }
                user.Telefon = txtTelefon;
                user.Email = txtEmail;
                user.Colectiv = colectivRep.Get(Convert.ToInt32(ddlColectiv));
                user.Titlu = txtTitlu;
                u.Update(user);
            }
            catch (Exception) { TempData["exc"] = "A aparut o exceptie la selectia datelor!"; }
            var model = new Models.MembruHomeModel();
            model.Utilizator = SecurityContext.CurrentUser as Utilizator;
            return View("ModificaMembruSucces",model);
        }
    
        [NHibernateSession]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadPozaProfil(long id, HttpPostedFileBase File)
        {
            var utilizatorRep = new UtilizatorRepository();
            var utilizator = utilizatorRep.GetAndLoadProxies(id);
            if (utilizator == null) return HttpNotFound();
            try
            {
                string message = "";
                if (!ValidImage(File, ref message))
                {
                    TempData["Image"] = message;

                }
                else
                {
                    var path = Path.Combine(ConfigurationManager.AppSettings["ImagesPath"], Path.GetFileName(File.FileName));
                    var pathSave=Path.Combine(Server.MapPath("~"+ConfigurationManager.AppSettings["ImagesPath"].ToString().Substring(27)), Path.GetFileName(File.FileName));
                    File.SaveAs(pathSave);
                    utilizator.PozaProfil = path;
                    utilizatorRep.Update(utilizator);
                }
            }
            catch (Exception) { TempData["err"] = "err"; }
            return RedirectToAction("ModificaMembru", "Home", new { id = utilizator.Id });

        }
        public bool ValidImage(HttpPostedFileBase img, ref string message)
        {

            int maxFileSize = 250;
            int fileSize = img.ContentLength;
            if (fileSize > (maxFileSize * 1024))
            {

                message += "Dimensiunea imaginii " + img.FileName + " depaseste limita admisa si nu a fost incarcata. Limita maxima este de " + maxFileSize + "KB.";
                return false;
            }
            string fileExtension = Path.GetExtension(img.FileName);
            fileExtension = fileExtension.ToLower();

            string[] acceptedFileTypes = new string[7];

            acceptedFileTypes[0] = ".jpg";
            acceptedFileTypes[1] = ".jpeg";
            acceptedFileTypes[2] = ".gif";
            acceptedFileTypes[3] = ".png";

            bool acceptFile = false;

            for (int i = 0; i <= 3; i++)
            {
                if (fileExtension == acceptedFileTypes[i])
                {

                    acceptFile = true;
                }
            }

            if (!acceptFile)
            {

                message += "Tipul imaginii " + img.FileName + " nu este permis.Variantele acceptate sunt :.jpg, .jpeg, .gif, .png";
                return false;
            }

            return true;
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult MembriiDepartament(int? page)
        {

            var repo = new UtilizatorRepository();
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDepartament(zeroBasePageIndex, 10,User.Identity.Name);
            var utilizatorRep = new UtilizatorRepository();
            var utilizatorCurent = utilizatorRep.GetUserByName(User.Identity.Name,"");
            return View((new MembruHomeModel
            {
                MembriiDepartament=membriiDepartament,
                Utilizator = utilizatorCurent
            }));
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectiv(int? page,long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDepartamentDupaColectiv(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();
            
            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivBP(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDepartamentDupaColectivBP(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivPA(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivPA(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivBD(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivBD(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivSCO(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivSCO(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivC1(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivC1(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivC2(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivC2(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetMembriiDupaColectivBazeleTIC(int? page, long id)
        {

            var repo = new UtilizatorRepository();
            var utilizator = repo.GetAndLoadProxies(id);
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membriiDepartament = repo.GetMembriiDupaColectivBazeleTic(zeroBasePageIndex, 10, utilizator.Username);
            var utilizatorRep = new UtilizatorRepository();

            return View((new MembruHomeModel
            {
                MembriiDepartament = membriiDepartament,
                Utilizator = utilizator
            }));
        }
        [HttpGet]
        public ActionResult TrimiteMesaj(long idDestinatar)
        {
            var model = new TrimiteMesajModel();
            var utilizatorRep=new UtilizatorRepository();
            if (Request.IsAjaxRequest())
            {
                model.Destinatar = utilizatorRep.Get(idDestinatar);

                return PartialView("TrimiteMesaj",model);
            }

            return View();
        }
        [HttpPost]
        public ActionResult TrimiteMesaj(TrimiteMesajModel model)
        {
            var utilizatorRep=new UtilizatorRepository();
            //var model = new TrimiteMesajModel();
            //model.Detalii = txtDetalii;
            //model.Titlu = txtTitlu;
            //model.Destinatar = utilizatorRep.Get(Convert.ToInt64(txtIdDestinatar));
        
            // Validate the model being submitted
            if (!ModelState.IsValid)
            {
                // If the incoming request is an Ajax Request
                // then we just return a partial view (snippet) of HTML
                // instead of the full page
                if (Request.IsAjaxRequest())
                    return PartialView("TrimiteMesaj", model);

                return View(model);
            }

            // TODO: A real app would send some sort of email here
           
            if (Request.IsAjaxRequest())
            {
                var expeditor = utilizatorRep.GetUserByName(User.Identity.Name, "");
                var mesajRep = new Disertatie.Core.Repositories.Repository<long, Mesaj>();
                try
                {
                    DateTimeFormatInfo format = new DateTimeFormatInfo();
                    format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                    format.DateSeparator = "-";

                    var mesaj = new Disertatie.Core.Models.Mesaj();
                    mesaj.DataTrimitere =Convert.ToDateTime(DateTime.Now,format);
                    mesaj.Destinatar = utilizatorRep.Get(model.Destinatar.Id);
                    mesaj.Titlu = model.Titlu;
                    mesaj.Detalii = model.Detalii;
                    mesaj.Expeditor = expeditor;
                    mesaj.Citit = false;
                    mesaj.Activ = true;
                    mesajRep.Save(mesaj);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("TrimiteMesajSucces", model);
            }

            // A standard (non-Ajax) HTTP Post came in
            // set TempData and redirect the user back to the Home page
            TempData["Message"] = string.Format("Mesajul a fost trimis cu succes");
            return RedirectToAction("Index");
        }
        public ActionResult About()

        {
            return View();
        }
    }
}
