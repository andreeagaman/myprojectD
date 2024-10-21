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

namespace Disertatie.Controllers
{
    public class MesajeController : Controller
    {
        //
        // GET: /Mesaje/

        public ActionResult Index()
        {
            return View();
        }
         [Authorize(Roles = "Administrator,Membru, Sef departament, Coordonator colectiv")]
        [NHibernateSession]
        public ActionResult StergeMesaj(long id, int? page1, int? page2, int? page3)
        {
            var mesajeRep = new Repository<long, Mesaj>();
            var mesaj = mesajeRep.Get(id);
            mesaj.Activ = false;
            mesajeRep.Update(mesaj);

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
            return View("MesajSters",model);
        }
        [NHibernateSession]
        public void MarcareCitit(string strId)
        {

            try
            {

                var mesajeRep = new Repository<long, Mesaj>();
                var mesaj = mesajeRep.Get(Convert.ToInt64(strId));
                mesaj.Citit = true;
                mesajeRep.Update(mesaj);

            }
            catch (Exception)
            {
            }

           
            
        }

    }
}
