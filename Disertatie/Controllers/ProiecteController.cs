using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disertatie.Models;
using Disertatie.Core.Repositories;
using Disertatie.Core.Models;
using System.Globalization;
using Disertatie.Mvc;
using Disertatie.Core;
using System.IO;
using System.Configuration;

namespace Disertatie.Controllers
{
    public class ProiecteController : Controller
    {
        //
        // GET: /Proiecte/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Proiecte(int? page1, int? page2, int? page3, int? page4)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10,user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetProiecteleMele(int? page1, int? page2, int? page3, int? page4)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10, user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetProiecteleInColaborare(int? page1, int? page2, int? page3, int? page4)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiecteInColaborarePropuse(zeroBasePageIndex1, 10, user);
            model.ProiecteInchise = repo.GetProiecteInColaborareInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInColaborareInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteInColaborareAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult ProiecteMembru(string idUtilizator, int? page)
        {
            if (idUtilizator != null)
                Session["idUtilizator"] = idUtilizator;
            var repo = new UtilizatorRepository();
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var membru = repo.Get(Convert.ToInt64(idUtilizator == null ? Session["idUtilizator"] : idUtilizator));
            model.ProiecteMembru = repo.GetProiecteMembru(zeroBasePageIndex, 10, membru);
            model.Utilizator = user;
            model.MembruCuProiecte = membru;
            model.EstePaginaCuProiecte = true;
            return View(model);

        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Detaliiproiect(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ==true).Select(a => a.Utilizator).ToList();
            var membruExistent = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ==true && a.Utilizator==user).FirstOrDefault();
            model.EsteDejaMembru = membruExistent != null ? true : false;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep=new Repository<long,ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip==1 && a.Activ==true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip==2 && a.Activ==true).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult DetaliiProiectInvitatii(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).Select(a => a.Utilizator).ToList();
            var membruExistent = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true && a.Utilizator == user).FirstOrDefault();
            model.EsteDejaMembru = membruExistent != null ? true : false;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult DetaliiProiectCereri(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).Select(a => a.Utilizator).ToList();
            var membruExistent = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true && a.Utilizator == user).FirstOrDefault();
            model.EsteDejaMembru = membruExistent != null ? true : false;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult DetaliiProiectAnunturi(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).Select(a => a.Utilizator).ToList();
            var membruExistent = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true && a.Utilizator == user).FirstOrDefault();
            model.EsteDejaMembru = membruExistent != null ? true : false;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = new List<DocumentIncarcat>();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult DetaliiProiecteAnulate(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect).Select(a=>a.Utilizator).ToList();
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            model.EstePaginaCuProiecte = true;
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = new List<DocumentIncarcat>();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
           
            return View(model);
        }
     
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult DetaliiProiecteInColaborare(string id)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(Convert.ToInt64(id));
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect).Select(a => a.Utilizator).ToList();
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = new List<DocumentIncarcat>();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [NHibernateSession]
        public ActionResult CreeazaProiect()
        {
            var model = new MembruHomeModel();
            model.CreeazaProiectModel = new CreeazaProiectModel();
            model.CreeazaProiectModel.ObiectiveGenerale = new List<ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveSpecifice = new List<ObiectivProiect>();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            return View("CreeazaProiect",model);
        }
        [NHibernateSession]
        [HttpPost]
        public ActionResult CreeazaProiect( MembruHomeModel model)
        {
            try
            {
                var utilizatorRep = new UtilizatorRepository();
                var user = SecurityContext.CurrentUser as Utilizator;
                model.Utilizator = user;
                var utilizator = utilizatorRep.GetUserByName(User.Identity.Name, "");
                var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";

                var proiect = new Disertatie.Core.Models.Proiect();
                proiect.DataCreare = Convert.ToDateTime(DateTime.Now, format);
                proiect.DataModificareStare = Convert.ToDateTime(DateTime.Now, format);
                proiect.Titlu = model.CreeazaProiectModel.Titlu;
                proiect.Detalii = model.CreeazaProiectModel.Detalii;
                proiect.Durata = model.CreeazaProiectModel.Durata;
                proiect.NumarMembriiProiect = model.CreeazaProiectModel.NumarMembrii;
                proiect.MembriiNecesari = model.CreeazaProiectModel.NumarMembrii - 1;
                proiect.Initiator = utilizator;
                proiect.Status = 1;
                proiectRep.Save(proiect);
                var obiectiveRep = new Repository<long, ObiectivProiect>();
                if (model.CreeazaProiectModel.ObiectiveGenerale != null)
                {
                    foreach (var obiectiv in model.CreeazaProiectModel.ObiectiveGenerale)
                    {
                        obiectiv.Proiect = proiect;
                        obiectiv.Tip = 1;
                        obiectiv.Activ = true;
                        obiectiveRep.Save(obiectiv);
                    }
                }
                if (model.CreeazaProiectModel.ObiectiveSpecifice != null)
                {
                    foreach (var obiectiv in model.CreeazaProiectModel.ObiectiveSpecifice)
                    {
                        obiectiv.Proiect = proiect;
                        obiectiv.Tip = 2;
                        obiectiv.Activ = true;
                        obiectiveRep.Save(obiectiv);
                    }
                }
            }
            catch (Exception) { TempData["exc"] = "A aparut o exceptie la selectia datelor!"; }

            return View("CreeazaProiectSucces",model);
        }
        //[HttpPost]
        //public ActionResult CreeazaProiect(string txtTitlu, string txtDetalii)
        //{
        //    var utilizatorRep = new UtilizatorRepository();
        //    var model = new CreeazaProiectModel();
        //    model.Detalii = txtDetalii;
        //    model.Titlu = txtTitlu;
            

        //    // Validate the model being submitted
        //    if (!ModelState.IsValid)
        //    {
        //        // If the incoming request is an Ajax Request
        //        // then we just return a partial view (snippet) of HTML
        //        // instead of the full page
        //        if (Request.IsAjaxRequest())
        //            return PartialView("CreeazaProiect", model);

        //        return View(model);
        //    }

        //    // TODO: A real app would send some sort of email here

        //    if (Request.IsAjaxRequest())
        //    {
        //        var utilizator = utilizatorRep.GetUserByName(User.Identity.Name, "");
        //        var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
        //        try
        //        {
        //            DateTimeFormatInfo format = new DateTimeFormatInfo();
        //            format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
        //            format.DateSeparator = "-";

        //            var proiect = new Disertatie.Core.Models.Proiect();
        //            proiect.DataCreare = Convert.ToDateTime(DateTime.Now, format);
        //            proiect.Titlu= model.Titlu;
        //            proiect.Detalii = model.Detalii;
        //            proiect.Initiator = utilizator;
        //            proiect.Status = 1;
        //            proiectRep.Save(proiect);
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }

        //        return PartialView("CreeazaProiectSucces", model);
        //    }

        //    // A standard (non-Ajax) HTTP Post came in
        //    // set TempData and redirect the user back to the Home page
        //    TempData["Message"] = string.Format("Mesajul a fost trimis cu succes");
        //    return RedirectToAction("Index");
        //}
         [HttpPost]
        public ActionResult AdaugaObiectivGeneral(string txtDescriereOG,MembruHomeModel model)
        {
            
           var obiectivGeneral= new ObiectivProiect();
           obiectivGeneral.Descriere = txtDescriereOG;
           obiectivGeneral.Tip = 1;
           if (model.CreeazaProiectModel == null)
           {
               model.CreeazaProiectModel = new CreeazaProiectModel();
               if (model.CreeazaProiectModel.ObiectiveGenerale == null)
                   model.CreeazaProiectModel.ObiectiveGenerale = new List<ObiectivProiect>();
           }
           if (model.CreeazaProiectModel.ObiectiveGenerale == null)
               model.CreeazaProiectModel.ObiectiveGenerale = new List<ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale.Add(obiectivGeneral);
            ModelState.Clear();
            return Request.IsAjaxRequest()
		   ? (ActionResult)PartialView("ObiectiveGenerale", model)
		   : View("CreeazaProiect", model);
           
        }
         [HttpPost]
         public ActionResult CreeazaComentariuProiect(string txtDescriereComentariu,string txtProiectId)
         {
             var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
             var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
             var proiect= proiectRep.Get(Convert.ToInt64(txtProiectId));
             var user = SecurityContext.CurrentUser as Utilizator;
             try
             {
                 var comentariu = new ComentariuProiect();
                 comentariu.Descriere = txtDescriereComentariu.ToString();
                 comentariu.Proiect =proiect  ;// model.Proiect;//proiectRep.Get(Convert.ToInt64(form["txtIdProiect"]));
                 comentariu.Activ = true;
                 DateTimeFormatInfo format = new DateTimeFormatInfo();
                 format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                 format.DateSeparator = "-";
                 comentariu.DataCrearii = Convert.ToDateTime(DateTime.Now, format);
                 comentariu.Utilizator = user;
                 comentariuRep.Save(comentariu);
             }
             catch (Exception)
             {
             }
             var model = new MembruHomeModel();
             model.Proiect = proiect;
             model.ComentariiProiect = new List<ComentariuProiect>();
             model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).OrderBy(a=>a.DataCrearii).ToList();
             model.Utilizator = user;
             ModelState.Clear();
             return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("ComentariiProiect", model)
            : View("DetaliiProiect", model);

         }
         [HttpPost]
         public ActionResult Evaluare(FormCollection form)
         {
             var rate = Convert.ToInt32(form["Score"]);
             var id = Convert.ToInt32(form["IdProiect"]);
             if (Request.Cookies["rating" + id] != null)
                 return Content("false");
             Response.Cookies["rating" + id].Value = DateTime.Now.ToString();
             Response.Cookies["rating" + id].Expires = DateTime.Now.AddYears(1);
             EvaluareProiect ar = IncrementArticleRating(rate, id);
             return Json(ar);
         }
         public EvaluareProiect IncrementArticleRating(int rate, int id)
         {
             var proiectRepository = new Disertatie.Core.Repositories.Repository<long, Proiect>();

             var proiect = proiectRepository.Get(id);
             try
             {
                 proiect.Evaluare += rate;
                 proiect.TotalEvaluatori += 1;
                 proiectRepository.SaveOrUpdate(proiect);
                 proiectRepository.Session.Flush();
                
             }
             catch (Exception)
             {
             }
             
             var ar = new EvaluareProiect()
             {
                 IdProiect = proiect.Id,
                 Evaluare = proiect.Evaluare,
                 TotalEvaluatori = proiect.TotalEvaluatori,
                 EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori)
             };

             return ar;
             
         }
        [NHibernateSession]
         public ActionResult StergeInvitatie(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var invitatieRep = new Repository<long, InvitatiiProiect>();
                var invitatie = invitatieRep.Get(Convert.ToInt64(id));
                invitatie.ActivaExpeditor = false;
                invitatie.DataInactivariiExpeditor = DateTime.Now;
                invitatieRep.Update(invitatie);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InviitatiiProiectTrimiseInAsteptare = repo.GetInvitatiiProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.InviitatiiProiectTrimiseAcceptate = repo.GetInvitatiiProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InviitatiiProiectTrimiseRespinse = repo.GetInvitatiiProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("InvitatiiProiect", model);
        }
        [NHibernateSession]
        public ActionResult StergeDocument(long id)
        {
            var documenteIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var document = documenteIncarcateRep.Get(id);
            try
            {

               
                document.Activ = false;
                documenteIncarcateRep.Update(document);


            }
            catch (Exception)
            {
            }
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            model.Utilizator = user;
            model.Proiect = document.Proiect;
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == document.Proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = doc;
            return View("UploadDocumenteProiect",model);

        }
        [NHibernateSession]
        public ActionResult StergeCerere(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var cerereRep = new Repository<long, CereriProiect>();
                var cerere = cerereRep.Get(Convert.ToInt64(id));
                cerere.ActivaExpeditor = false;
                cerere.DataInactivariiExpeditor = DateTime.Now;
                cerereRep.Update(cerere);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriProiectTrimiseInAsteptare = repo.GetCereriProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriProiectTrimiseAcceptate = repo.GetCereriProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.CereriProiectTrimiseRespinse = repo.GetCereriProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("CereriProiect", model);
        }
        [NHibernateSession]
        public ActionResult StergeInvitatiePrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var invitatieRep = new Repository<long, InvitatiiProiect>();
                var invitatie = invitatieRep.Get(Convert.ToInt64(id));
                invitatie.ActivaDestinatar = false;
                invitatie.DataInactivariiDestinatar = DateTime.Now;
                invitatieRep.Update(invitatie);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InvitatiiPrimiteProiectInAsteptare = repo.GetInvitatiiPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.InvitatiiPrimiteProiectAcceptate = repo.GetInvitatiiPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InvitatiiPrimiteProiectRespinse = repo.GetInvitatiiPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("InvitatiiPrimiteProiect", model);
        }
        [NHibernateSession]
        public ActionResult StergeCererePrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var cerereRep = new Repository<long, CereriProiect>();
                var cerere = cerereRep.Get(Convert.ToInt64(id));
                cerere.ActivaDestinatar = false;
                cerere.DataInactivariiDestinatar = DateTime.Now;
                cerereRep.Update(cerere);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriPrimiteProiectInAsteptare = repo.GetCereriPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriPrimiteProiectAcceptate = repo.GetCereriPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.CereriPrimiteProiectRespinse = repo.GetCereriPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user; 
            model.EstePaginaCuProiecte = true;
            return View("CereriPrimiteProiect", model);
        }
        [NHibernateSession]
        public ActionResult AcceptaInvitatiaPrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var invitatieRep = new Repository<long, InvitatiiProiect>();
                var invitatie = invitatieRep.Get(Convert.ToInt64(id));
                if (invitatie.Proiect.MembriiNecesari == 0)
                {
                    TempData["Message"] = string.Format("Proiectul nu mai are locuri disponibile!");
                }
                else
                {
                    var utilizator = SecurityContext.CurrentUser as Utilizator;
                    var membriiProiectRep = new Disertatie.Core.Repositories.Repository<long, MembriiProiect>();
                    var membruProiectExistent = membriiProiectRep.QueryAll().Where(p => p.Proiect == invitatie.Proiect && p.Activ == true && p.Utilizator == utilizator).FirstOrDefault();
                    if (membruProiectExistent != null)
                    {
                        TempData["Message"] = string.Format("Sunteti deja membru al proiectului!");
                    }
                    else
                    {
                        invitatie.Status = 2;
                        invitatie.DataAcceptarii = DateTime.Now;
                        invitatieRep.Update(invitatie);
                        //var membriiProiectRep = new Disertatie.Core.Repositories.Repository<long, MembriiProiect>();
                        var membruProiect = new MembriiProiect();
                        membruProiect.Proiect = invitatie.Proiect;
                        membruProiect.Utilizator = utilizator;
                        membruProiect.Activ = true;
                        membriiProiectRep.Save(membruProiect);
                        var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
                        var proiect = proiectRep.Get(invitatie.Proiect.Id);
                        proiect.MembriiNecesari--;
                        
                        proiectRep.Update(proiect);
                    }
                }
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InvitatiiPrimiteProiectInAsteptare = repo.GetInvitatiiPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.InvitatiiPrimiteProiectAcceptate = repo.GetInvitatiiPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InvitatiiPrimiteProiectRespinse = repo.GetInvitatiiPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("InvitatiiPrimiteProiect", model);
        }
        [NHibernateSession]
        public ActionResult AcceptaCererePrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var cerereRep = new Repository<long, CereriProiect>();
                var cerere = cerereRep.Get(Convert.ToInt64(id));
                if (cerere.Proiect.MembriiNecesari == 0)
                {
                    TempData["Message"] = string.Format("Proiectul nu mai are locuri disponibile!");
                }
                else
                {

                    var membriiProiectRep = new Disertatie.Core.Repositories.Repository<long, MembriiProiect>();
                    var membruProiectExistent = membriiProiectRep.QueryAll().Where(p => p.Proiect == cerere.Proiect && p.Activ == true && p.Utilizator == cerere.Expeditor).FirstOrDefault();
                    if (membruProiectExistent != null)
                    {
                        TempData["Message"] = string.Format("Sunteti deja membru al proiectului!");
                    }
                    else
                    {
                        cerere.Status = 2;
                        cerere.DataAcceptarii = DateTime.Now;
                        cerereRep.Update(cerere);

                        var membruProiect = new MembriiProiect();
                        membruProiect.Proiect = cerere.Proiect;
                        membruProiect.Utilizator = cerere.Expeditor;
                        membruProiect.Activ = true;
                        membriiProiectRep.Save(membruProiect);
                        var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
                        var proiect = proiectRep.Get(cerere.Proiect.Id);
                        proiect.MembriiNecesari--;
                        proiectRep.Update(proiect);
                    }
                }
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriPrimiteProiectInAsteptare = repo.GetCereriPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriPrimiteProiectAcceptate = repo.GetCereriPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.CereriPrimiteProiectRespinse = repo.GetCereriPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("CereriPrimiteProiect", model);
        }
        [NHibernateSession]
        public ActionResult RespingeInvitatiaPrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var invitatieRep = new Repository<long, InvitatiiProiect>();
                var invitatie = invitatieRep.Get(Convert.ToInt64(id));
                invitatie.Status = 3;
                invitatie.DataRespingerii = DateTime.Now;
                invitatieRep.Update(invitatie);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InvitatiiPrimiteProiectInAsteptare = repo.GetInvitatiiPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.InvitatiiPrimiteProiectAcceptate = repo.GetInvitatiiPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InvitatiiPrimiteProiectRespinse = repo.GetInvitatiiPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("InvitatiiPrimiteProiect", model);
        }
        [NHibernateSession]
        public ActionResult RespingeCererPrimita(string id, int? page1, int? page2, int? page3)
        {
            try
            {
                var cerereRep = new Repository<long, CereriProiect>();
                var cerere = cerereRep.Get(Convert.ToInt64(id));
                cerere.Status = 3;
                cerere.DataRespingerii = DateTime.Now;
                cerereRep.Update(cerere);
            }

            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriPrimiteProiectInAsteptare = repo.GetCereriPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriPrimiteProiectAcceptate = repo.GetCereriPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.CereriPrimiteProiectRespinse = repo.GetCereriPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("CereriPrimiteProiect", model);
        }
         [HttpPost]
        public ActionResult StergeObiectivGeneral(MembruHomeModel model, int indexOG)
         {
             model.CreeazaProiectModel.ObiectiveGenerale.RemoveAt(indexOG);
             ModelState.Clear();
             return Request.IsAjaxRequest()
                 ? (ActionResult)PartialView("ObiectiveGenerale", model)
                 : View("CreeazaProiect", model);

         }
         [HttpPost]
         public ActionResult StergeComentariuProiect(long indexC)
         {
            var model = new MembruHomeModel();
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            var comentariu = comentariuRep.Get(indexC);
            var user = SecurityContext.CurrentUser as Utilizator;
             try
             {
                 
                 comentariu.Activ = false;
                 comentariuRep.Update(comentariu);
                 comentariuRep.Session.Flush();
             }
             catch (Exception)
             {
             }
             model.ComentariiProiect = new List<ComentariuProiect>();
             model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == comentariu.Proiect && a.Activ == true).OrderByDescending(a => a.DataCrearii).ToList();
             model.Utilizator = user;
             ModelState.Clear();
             return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("ComentariiProiect", model)
            : View("DetaliiProiect", model);


            
         }
         [HttpPost]
         public ActionResult AdaugaObiectivSpecific(string txtDescriereOS, MembruHomeModel model)
         {

             var obiectivSpecific = new ObiectivProiect();
             obiectivSpecific.Descriere = txtDescriereOS;
             obiectivSpecific.Tip = 2;
             if (model.CreeazaProiectModel == null)
             {
                 model.CreeazaProiectModel = new CreeazaProiectModel();
                 if (model.CreeazaProiectModel.ObiectiveSpecifice == null)
                     model.CreeazaProiectModel.ObiectiveSpecifice = new List<ObiectivProiect>();
             }
             if (model.CreeazaProiectModel.ObiectiveSpecifice == null)
                 model.CreeazaProiectModel.ObiectiveSpecifice = new List<ObiectivProiect>();
             model.CreeazaProiectModel.ObiectiveSpecifice.Add(obiectivSpecific);
             ModelState.Clear();
             return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("ObiectiveSpecifice", model)
            : View("CreeazaProiect", model);

         }
         [HttpPost]
         public ActionResult StergeObiectivSpecific(MembruHomeModel model, int indexOS)
         {
             model.CreeazaProiectModel.ObiectiveSpecifice.RemoveAt(indexOS);
             ModelState.Clear();
             return Request.IsAjaxRequest()
                 ? (ActionResult)PartialView("ObiectiveSpecifice", model)
                 : View("CreeazaProiect", model);

         }
        [NHibernateSession]
         public ActionResult InchideProiect(long id, int? page1, int? page2, int? page3, int? page4)
        {
            try
            {
                var proiecteRep = new Repository<long, Proiect>();
                var proiect = proiecteRep.Get(id);
                proiect.Status = 3;
                 DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";
                proiect.DataModificareStare = Convert.ToDateTime(DateTime.Now, format);
                proiecteRep.Update(proiect);
                }
            catch (Exception)
            {
            }
                var repo = new UtilizatorRepository();
                var user = SecurityContext.CurrentUser as Utilizator;
                var model = new MembruHomeModel();
                var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
                var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
                var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
                var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
                model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10, user);
                model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
                model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
                model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
                model.Utilizator = user;
                model.EstePaginaCuProiecte = true;
                return View("Proiecte",model);

            

        }
       
        [NHibernateSession]
        public ActionResult InLucruProiect(long id, int? page1, int? page2, int? page3, int? page4)
        {
            try
            {
                
                var proiecteRep = new Repository<long, Proiect>();
                var proiect = proiecteRep.Get(id);
                if (proiect.MembriiNecesari == 0)
                {
                    proiect.Status = 2;
                    DateTimeFormatInfo format = new DateTimeFormatInfo();
                    format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                    format.DateSeparator = "-";
                    proiect.DataModificareStare = Convert.ToDateTime(DateTime.Now, format);
                    proiecteRep.Update(proiect);
                   
                }
                else
                {
                    TempData["Message"] = string.Format("Proiectul nu poate trece in starea in lucru deoarece nu are toti membrii necesari!");
                }
            }
            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10, user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("Proiecte", model);


        }
        [NHibernateSession]
        public ActionResult MembriiProiect(int idProiect)
        {
            var model = new MembruHomeModel();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.Utilizator = user;
            var proiecteRep = new Repository<long, Proiect>();
            var proiect = proiecteRep.Get(idProiect);
            var membriiRep=new Repository<long,Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect).Select(a => a.Utilizator).ToList();
            model.Proiect = proiect;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [NHibernateSession]
        public ActionResult DeschideProiect(long id, int? page1, int? page2, int? page3, int? page4)
        {
            try
            {
                var proiecteRep = new Repository<long, Proiect>();
                var proiect = proiecteRep.Get(id);
                proiect.Status = 1;
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";
                proiect.DataModificareStare = Convert.ToDateTime(DateTime.Now, format);

                proiecteRep.Update(proiect);
            }
            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10, user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("Proiecte", model);



        }

        [NHibernateSession]
        public ActionResult AnuleazaProiect(long id, int? page1, int? page2, int? page3, int? page4)
        {
            try
            {
                var proiecteRep = new Repository<long, Proiect>();
                var proiect = proiecteRep.Get(id);
                proiect.Status = 4;
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";
                proiect.DataModificareStare = Convert.ToDateTime(DateTime.Now, format);
                proiecteRep.Update(proiect);
            }
            catch (Exception)
            {
            }
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            var zeroBasePageIndex4 = Math.Max((page4 ?? 1) - 1, 0);
            model.ProiectePropuse = repo.GetProiectePropuse(zeroBasePageIndex1, 10, user);
            model.ProiecteInchise = repo.GetProiecteInchise(zeroBasePageIndex2, 10, user);
            model.ProiecteInLucru = repo.GetProiecteInLucru(zeroBasePageIndex3, 10, user);
            model.ProiecteAnulate = repo.GetProiecteAnulate(zeroBasePageIndex4, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("Proiecte", model);



        }
        [HttpGet]
        public ActionResult TrimiteInvitatie(long id)
        {
            var model = new TrimiteInvitatieModel();
            var proiectRep = new Repository<long,Proiect>();
            var utilizatorRep = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            if (Request.IsAjaxRequest())
            {
                model.Proiect = proiectRep.Get(id);
                model.Membrii = utilizatorRep.GetMembriiDepartament(user.Username);
                return PartialView("TrimiteInvitatie", model);
            }

            return View();
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult InvitatiiProiect(int? page1, int? page2, int? page3)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InviitatiiProiectTrimiseInAsteptare = repo.GetInvitatiiProiectInAsteptare(zeroBasePageIndex1,10,user);
            model.InviitatiiProiectTrimiseAcceptate = repo.GetInvitatiiProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InviitatiiProiectTrimiseRespinse = repo.GetInvitatiiProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult TrimiteCerereProiect(long id)
        {
            var user = SecurityContext.CurrentUser as Utilizator;
            var repo = new UtilizatorRepository();
               
                var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();
                var proiect = proiectRep.Get(id);
            try
            {
                
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";
                var cerereRep = new Disertatie.Core.Repositories.Repository<long, CereriProiect>();
                var cerere = new CereriProiect();
                cerere.Proiect = proiect;
                cerere.Expeditor = user;
                cerere.Destinatar = proiect.Initiator;
                cerere.DataCreare = Convert.ToDateTime(DateTime.Now, format);
                cerere.ActivaDestinatar = true;
                cerere.ActivaExpeditor = true;
                cerere.Status = 1;
                cerereRep.Save(cerere);
                TempData["SuccessMessage"] = "Cererea a fost trimisa cu succes!";
            }
            catch (Exception)
            {
            }
            var model = new MembruHomeModel();
            model.Utilizator = user;
            model.Proiect = proiect;
            var membriiRep = new Repository<long, Disertatie.Core.Models.MembriiProiect>();
            model.MembriiProiect = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).Select(a => a.Utilizator).ToList();
            var membruExistent = membriiRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true && a.Utilizator == user).FirstOrDefault();
            model.EsteDejaMembru = membruExistent != null ? true : false;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1 && a.Activ == true).ToList();
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2 && a.Activ == true).ToList();
            model.EvaluareProiect = new EvaluareProiect();
            model.EvaluareProiect.IdProiect = proiect.Id;
            model.EvaluareProiect.Evaluare = proiect.Evaluare;
            model.EvaluareProiect.TotalEvaluatori = proiect.TotalEvaluatori;
            model.EvaluareProiect.EvaluareMedie = Convert.ToDouble(proiect.Evaluare) / Convert.ToDouble(proiect.TotalEvaluatori);
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ == true).ToList();
            model.DocumenteIncarcate = doc;
            var comentariuRep = new Disertatie.Core.Repositories.Repository<long, ComentariuProiect>();
            model.ComentariiProiect = new List<ComentariuProiect>();
            model.ComentariiProiect = comentariuRep.QueryAll().Where(a => a.Proiect == model.Proiect && a.Activ == true).OrderBy(a => a.DataCrearii).ToList();
            model.EstePaginaCuProiecte = true;
            return View("DetaliiProiect", model);

        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult CereriProiect(int? page1, int? page2, int? page3)
        {

            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriProiectTrimiseInAsteptare = repo.GetCereriProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriProiectTrimiseAcceptate = repo.GetCereriProiectInAsteptare(zeroBasePageIndex2, 10, user);
            model.CereriProiectTrimiseRespinse = repo.GetCereriProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult CereriPrimiteProiect(int? page1, int? page2, int? page3)
        {
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.CereriPrimiteProiectInAsteptare = repo.GetCereriPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.CereriPrimiteProiectAcceptate = repo.GetCereriPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.CereriPrimiteProiectRespinse = repo.GetCereriPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult InvitatiiPrimiteProiect(int? page1, int? page2, int? page3)
        {
            var repo = new UtilizatorRepository();
            var user = SecurityContext.CurrentUser as Utilizator;
            var model = new MembruHomeModel();
            var zeroBasePageIndex1 = Math.Max((page1 ?? 1) - 1, 0);
            var zeroBasePageIndex2 = Math.Max((page2 ?? 1) - 1, 0);
            var zeroBasePageIndex3 = Math.Max((page3 ?? 1) - 1, 0);
            model.InvitatiiPrimiteProiectInAsteptare = repo.GetInvitatiiPrimiteProiectInAsteptare(zeroBasePageIndex1, 10, user);
            model.InvitatiiPrimiteProiectAcceptate = repo.GetInvitatiiPrimiteProiectAcceptate(zeroBasePageIndex2, 10, user);
            model.InvitatiiPrimiteProiectRespinse = repo.GetInvitatiiPrimiteProiectRespinse(zeroBasePageIndex3, 10, user);
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View(model);
        }
        [HttpPost]
        public ActionResult TrimiteInvitatie(string txtIdProiect, FormCollection form)
        {
            var utilizatorRep = new UtilizatorRepository();
            var model = new TrimiteInvitatieModel();
            var proiectRep = new Repository<long, Proiect>();
            string selectedValues= form["lbMembrii"];
            model.Proiect = proiectRep.Get(Convert.ToInt64(txtIdProiect));
            model.Expeditor=  SecurityContext.CurrentUser as Utilizator;
            // Validate the model being submitted
            if (!ModelState.IsValid)
            {
                // If the incoming request is an Ajax Request
                // then we just return a partial view (snippet) of HTML
                // instead of the full page
                if (Request.IsAjaxRequest())
                    return PartialView("TrimiteInvitatie", model);

                return View(model);
            }

            // TODO: A real app would send some sort of email here

            if (Request.IsAjaxRequest())
            {
                var expeditor = utilizatorRep.GetUserByName(User.Identity.Name, "");
                var invitatiiProiectRep = new Disertatie.Core.Repositories.Repository<long, InvitatiiProiect>();
                try
                {
                    DateTimeFormatInfo format = new DateTimeFormatInfo();
                    format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                    format.DateSeparator = "-";
                    foreach (var idMembru in selectedValues.Split(','))
                    {
                    var invitatie = new Disertatie.Core.Models.InvitatiiProiect();
                    invitatie.DataCreare =Convert.ToDateTime(DateTime.Now, format);
                    invitatie.Proiect = model.Proiect;
                    invitatie.Expeditor = expeditor;
                    invitatie.Destinatar = utilizatorRep.Get(Convert.ToInt64(idMembru));
                    invitatie.Status= 1;
                    invitatie.ActivaDestinatar = true;
                    invitatie.ActivaExpeditor = true;
                    invitatiiProiectRep.Save(invitatie);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("TrimiteInvitatieSucces", model);
            }

            // A standard (non-Ajax) HTTP Post came in
            // set TempData and redirect the user back to the Home page
            TempData["Message"] = string.Format("Mesajul a fost trimis cu succes");
            return RedirectToAction("Index");
        }
        [NHibernateSession]
        public ActionResult EditeazProiect(long id)
        {
            var s = new Disertatie.Core.Repositories.Repository<long,Proiect>();

            var proiect = s.Get(id);
            var model = new Models.MembruHomeModel();
            model.Proiect = proiect;
            model.CreeazaProiectModel = new CreeazaProiectModel();
            model.CreeazaProiectModel.ObiectiveGenerale = new List<ObiectivProiect>();
            model.CreeazaProiectModel.ObiectiveSpecifice = new List<ObiectivProiect>();
            var user = SecurityContext.CurrentUser as Utilizator;
            model.CreeazaProiectModel.Titlu = proiect.Titlu;
            model.CreeazaProiectModel.Detalii = proiect.Detalii;
            model.CreeazaProiectModel.Durata = proiect.Durata.Value;
            model.CreeazaProiectModel.NumarMembrii = proiect.NumarMembriiProiect;
            var obiectiveRep = new Repository<long, ObiectivProiect>();
            var obiectigeGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1).ToList();
            var obiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2).ToList();
            model.CreeazaProiectModel.ObiectiveGenerale = obiectigeGenerale;
            model.CreeazaProiectModel.ObiectiveSpecifice = obiectiveSpecifice;
            model.Utilizator = user;
            model.EstePaginaCuProiecte = true;
            return View("EditeazProiect", model);
        }
        [NHibernateSession]
        [HttpPost]
        public ActionResult EditeazProiect(long id, MembruHomeModel model)
        {
            try
            {

                var proiectRep = new Disertatie.Core.Repositories.Repository<long,Proiect>();

                var proiect = proiectRep.Get(id);
                var obiectiveRep = new Repository<long, ObiectivProiect>();
                var obiectigeGenerale = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 1).ToList();
                var obiectiveSpecifice = obiectiveRep.QueryAll().Where(a => a.Proiect == proiect && a.Tip == 2).ToList();
                foreach (var obiectivGeneral in obiectigeGenerale)
                {
                    obiectiveRep.Delete(obiectivGeneral.Id);
                }
                foreach (var obiectivSpecific in obiectiveSpecifice)
                {
                    obiectiveRep.Delete(obiectivSpecific.Id);
                }
                var utilizatorRep = new UtilizatorRepository();
                var user = SecurityContext.CurrentUser as Utilizator;
                model.Utilizator = user;
                var utilizator = utilizatorRep.GetUserByName(User.Identity.Name, "");

                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                format.DateSeparator = "-";
                proiect.Titlu = model.CreeazaProiectModel.Titlu;
                proiect.Detalii = model.CreeazaProiectModel.Detalii;
                proiect.Durata = model.CreeazaProiectModel.Durata;
                proiect.NumarMembriiProiect = model.CreeazaProiectModel.NumarMembrii;
                proiect.MembriiNecesari = model.CreeazaProiectModel.NumarMembrii - 1;
                proiectRep.Update(proiect);

              
                if (model.CreeazaProiectModel.ObiectiveGenerale != null)
                {
                    foreach (var obiectiv in model.CreeazaProiectModel.ObiectiveGenerale)
                    {
                        obiectiv.Proiect = proiect;
                        obiectiv.Tip = 1;
                        obiectiv.Activ = true;
                        obiectiveRep.Save(obiectiv);
                    }
                }
                if (model.CreeazaProiectModel.ObiectiveSpecifice != null)
                {
                    foreach (var obiectiv in model.CreeazaProiectModel.ObiectiveSpecifice)
                    {
                        obiectiv.Proiect = proiect;
                        obiectiv.Tip = 2;
                        obiectiv.Activ = true;
                        obiectiveRep.Save(obiectiv);
                    }
                }
            }
            catch (Exception) { TempData["exc"] = "A aparut o exceptie la selectia datelor!"; }

            return View("EditeazaProiectSucces",model);
        }
        [NHibernateSession]
        public ActionResult UploadDocumenteProiect(long id)
        {
            var user = SecurityContext.CurrentUser as Utilizator;
            var proiectRep = new Disertatie.Core.Repositories.Repository<long, Proiect>();

            var proiect = proiectRep.Get(id);
            var model = new MembruHomeModel();
            model.Utilizator = user;
            model.Proiect = proiect;
            var docIncarcateRep = new Disertatie.Core.Repositories.Repository<long, DocumentIncarcat>();
            var doc = docIncarcateRep.QueryAll().Where(a => a.Proiect == proiect && a.Activ==true).ToList();
            model.DocumenteIncarcate = doc;
            return View(model);

           
        }
        [NHibernateSession]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadDocument(long id, HttpPostedFileBase File)
        {
            var proiecteRep = new Disertatie.Core.Repositories.Repository<long,Proiect>();
            var documenteIncarcateRep=new Disertatie.Core.Repositories.Repository<long,DocumentIncarcat>();
            var proiect = proiecteRep.Get(id);
            var documentIncarcat=new DocumentIncarcat();
            documentIncarcat.Proiect=proiect;
            var user=SecurityContext.CurrentUser as Utilizator;
            documentIncarcat.Utilizator=user;
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
            format.DateSeparator = "-";
            documentIncarcat.DataIncarcarii=Convert.ToDateTime(DateTime.Now,format);
            if (proiect == null) return HttpNotFound();
            try
            {
                List<string> errors = new List<string>();
                if (!IsValidFile(File, ref errors))
                {
                    //TempData["Message"] += "<ul>";
                    foreach (var error in errors)
                    {
                        TempData["Message"] += string.Format("{0}", error);
                    }
                    //TempData["Message"] += "</ul>";
                   

                }
                else
                {
                    var path = Path.Combine(ConfigurationManager.AppSettings["DocumentsPath"], Path.GetFileName(File.FileName));
                    var pathSave = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["DocumentsPath"].ToString().Substring(27)), Path.GetFileName(File.FileName));
                    File.SaveAs(pathSave);
                    documentIncarcat.Cale = path;
                    documentIncarcat.Nume = Path.GetFileName(File.FileName);
                    documentIncarcat.Activ = true;
                    documenteIncarcateRep.Save(documentIncarcat);
                }
            }
            catch (Exception) { TempData["err"] = "err"; }
            return RedirectToAction("UploadDocumenteProiect", "Proiecte", new { id = proiect.Id });

        }
        public bool IsValidFile(HttpPostedFileBase file, ref  List<string> errors)
        {
            if (errors == null) errors = new List<string>();
            string maxSize = "2048";

            if (!string.IsNullOrEmpty(maxSize))
            {
                long maxKb;
                if (long.TryParse(maxSize, out maxKb) && (file.ContentLength / 1024) > maxKb)
                    errors.Add(string.Format("Fisierul {0} este prea mare. Se pot incarca maxim {1} KB.", file.FileName, maxKb));

            }

            if (Path.GetExtension(file.FileName).Length == 0)
                errors.Add(string.Format("Fisierul {0} nu poate fi incarcat!", file.FileName));

            if (file.ContentLength == 0)
                errors.Add(string.Format("Fisierul {0} este gol", file.FileName));

            string valid_extension = ".pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .txt";

            if (!string.IsNullOrEmpty(valid_extension) && !valid_extension.ToLower().Contains(System.IO.Path.GetExtension(file.FileName).ToLower()))
                errors.Add(string.Format("Extensia fisierului {0} nu este buna. Extensiile valide sunt: {1}", file.FileName, valid_extension));
            return (errors.Count == 0);
        }

    }
}
