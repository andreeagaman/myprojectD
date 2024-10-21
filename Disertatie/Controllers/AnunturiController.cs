using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disertatie.Mvc;
using Disertatie.Core.Repositories;
using Disertatie.Core;
using Disertatie.Core.Models;
using Disertatie.Models;
using System.Globalization;

namespace Disertatie.Controllers
{
    public class AnunturiController : Controller
    {
        //
        // GET: /Anunturi/

        public ActionResult Index()
        {
            return View();
        }
       [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult Anunturi(int? page)
        {

            var repo = new UtilizatorRepository();
            var utilizatorRep = new UtilizatorRepository();
            var utilizatorCurent = utilizatorRep.GetUserByName(User.Identity.Name, "");
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var anunturi  = repo.GetAnunturi(zeroBasePageIndex, 10);
           
            return View((new MembruHomeModel
            {
                Anunturi = anunturi,
                Utilizator = utilizatorCurent
            }));
        }
       [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
       [NHibernateSession]
       public ActionResult DetaliiAnunt(long id)
       {
           var anuntRep = new Disertatie.Core.Repositories.Repository<long, Anunt>();
           var anunt = anuntRep.Get(id);
           var model = new MembruHomeModel();
           model.Anunt = anunt;
           model.Utilizator = SecurityContext.CurrentUser as Utilizator;
           return View (model);
       }
        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetAnunturileMele(int? page)
        {
            var repo = new UtilizatorRepository();
            var utilizatorRep = new UtilizatorRepository();
            var utilizatorCurent = utilizatorRep.GetUserByName(User.Identity.Name, "");
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var anunturi = repo.GetAnunturileMele(zeroBasePageIndex, 10, User.Identity.Name);

            return View((new MembruHomeModel
            {
                Anunturi = anunturi,
                Utilizator = utilizatorCurent
            }));
        }

        [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult GetToateAnunturile(int? page)
        {

            var repo = new UtilizatorRepository();
            var utilizatorRep = new UtilizatorRepository();
            var utilizatorCurent = utilizatorRep.GetUserByName(User.Identity.Name, "");
            var zeroBasePageIndex = Math.Max((page ?? 1) - 1, 0);
            var anunturi = repo.GetAnunturi(zeroBasePageIndex, 10);

            return View((new MembruHomeModel
            {
                Anunturi = anunturi,
                Utilizator = utilizatorCurent
            }));
        }
        [HttpGet]
        public ActionResult CreeazaAnunt()
        {
            var model = new CreeazaAnuntModel();

            if (Request.IsAjaxRequest())
            {

                return PartialView("CreeazaAnunt", model);
            }

            return View();
        }
        [HttpPost]
        public ActionResult CreeazaAnunt(CreeazaAnuntModel model)
        {
            var utilizatorRep = new UtilizatorRepository();
           // var model = new CreeazaAnuntModel();
            //model.Detalii = txtDetalii;
            //model.Titlu = txtTitlu;


            // Validate the model being submitted
            if (!ModelState.IsValid)
            {
                // If the incoming request is an Ajax Request
                // then we just return a partial view (snippet) of HTML
                // instead of the full page
                if (Request.IsAjaxRequest())
                    return PartialView("CreeazaAnunt", model);

                return View(model);
            }

            // TODO: A real app would send some sort of email here

            if (Request.IsAjaxRequest())
            {
                var utilizator = utilizatorRep.GetUserByName(User.Identity.Name, "");
                var anuntRep = new Disertatie.Core.Repositories.Repository<long, Anunt>();
                try
                {
                    DateTimeFormatInfo format = new DateTimeFormatInfo();
                    format.ShortDatePattern = "dd-MM-yyyy H:mm:ss";
                    format.DateSeparator = "-";

                    var anunt = new Disertatie.Core.Models.Anunt();
                    anunt.DataCreare = Convert.ToDateTime(DateTime.Now, format);
                    anunt.Nume = model.Titlu;
                    anunt.Detalii = model.Detalii;
                    anunt.Utilizator = utilizator;
                    anunt.Activ = true;
                    anuntRep.Save(anunt);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return PartialView("CreeazaAnuntSucces", model);
            }

            // A standard (non-Ajax) HTTP Post came in
            // set TempData and redirect the user back to the Home page
            TempData["Message"] = string.Format("Mesajul a fost trimis cu succes");
            return RedirectToAction("Anunturi");
        }
    }
}
