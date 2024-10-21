using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disertatie;
using Disertatie.Areas.Admin.Models;
using AutoMapper;
using System.Web.Security;
using Disertatie.Mvc;
using Disertatie.Core.Repositories;
using Disertatie.Core.Models;

namespace Disertatie.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {        

        [NHibernateSession]
        public ActionResult Index(int? page)
        {
            var repo = new UtilizatorRepository();
            var zeroBasePageIndex = (page ?? 1) - 1;
            repo.Session.Flush();
            var users = repo.GetPageList(zeroBasePageIndex,10);
            
            return View(users);
        }

        [NHibernateSession]
        public ActionResult Details(long id)
        {
            var repo = new UtilizatorRepository();
            var user = repo.Get(id);
            return View(user);
        }


        public ActionResult Create()
        {
            var model = new UserModel();
            var utilizatotRepository=new UtilizatorRepository();
            model.Colective = utilizatotRepository.GetColective();
            model.Roluri = utilizatotRepository.GetRoluri();
            return View(model);
        }
        [NHibernateSession, Authorize(Roles = "Administrator")]
        public ActionResult CautaUtilizator(int? page,string filter)
        {
            var r = new UtilizatorRepository();
            var zeroBasePageIndex = (page ?? 1) - 1;
            var users = r.GetUtilizatoriNume(zeroBasePageIndex, filter);
            return View("Index",users);
        }

        [HttpPost]
        [NHibernateSession]
        public ActionResult Create(UserModel userModel,string ddlColectiv,string ddlRol)
        {
            try

            {
                var colectivRep = new Repository<int, Colectiv>();
                userModel.Colectiv = colectivRep.Get(Convert.ToInt32(ddlColectiv));
                var roluriRep = new Repository<int, Rol>();
                var rol = roluriRep.Get(Convert.ToInt32(ddlRol));
                userModel.Rol = new List<Rol>();
                userModel.Rol.Add(rol);
                if (ModelState.IsValid) {
                    var user = Mapper.Map<UserModel, Utilizator>(userModel);
                    var repo = new UtilizatorRepository();
                    MembershipCreateStatus status = new MembershipCreateStatus();
                    Membership.CreateUser(user.Username, user.Parola, user.Email, user.IntrebareParola, user.RaspunsParola, user.Aprobat,null,out status);
                    if (status == MembershipCreateStatus.Success)
                    {
                        var userCreat = repo.GetUserByName(user.Username,"");
                        userCreat.Nume = user.Nume;
                        userCreat.Prenume = user.Prenume;
                        userCreat.Colectiv = user.Colectiv;
                        userCreat.Roluri = userModel.Rol;
                        userCreat.Titlu = userModel.Titlu;
                        repo.Update(userCreat);
                        repo.Session.Flush();
                        
                    }
                } else {
                    return View(userModel);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("create_failure", ex);
                return View(userModel);
            }
        }

        [NHibernateSession]
        public ActionResult Edit(long id)
        {
            var repo = new UtilizatorRepository();
            var user = repo.Get(id);
            var model = Mapper.Map<Utilizator, UserModel>(user);
            //var roles = Roles.GetAllRoles()
            //ViewBag.Roles = roles;
            model.Colective = repo.GetColective();
            model.Roluri = repo.GetRoluri();
            model.Colectiv = user.Colectiv;
            model.Rol = user.Roluri;
            return View(model);
        }

        [HttpPost]
        [NHibernateSession]
        public ActionResult Edit(long id, UserModel userModel,string ddlColectiv,string ddlRol)
        {
            try
            {
                if (ModelState.IsValid) {
                    var repo = new UtilizatorRepository();
                    var user = repo.Get(id);
                    Mapper.Map<UserModel, Utilizator>(userModel, user);
                    var colectivRep = new Repository<int, Colectiv>();
                    user.Colectiv = colectivRep.Get(Convert.ToInt32(ddlColectiv));
                    var roluriRep = new Repository<int, Rol>();
                    var rol = roluriRep.Get(Convert.ToInt32(ddlRol));
                    user.Roluri = new List<Rol>();
                    user.Roluri.Add(rol);
                    repo.Update(user);
                    repo.Session.Flush();
                } else {
                    return View(userModel);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userModel);
            }
        }
 
     
       
        [NHibernateSession]
        public ActionResult Delete(long id, int? page)
        {
            try {
 
                var utilizatorRep = new UtilizatorRepository();
                var util = utilizatorRep.Get(id);
                util.Aprobat = false;
                utilizatorRep.Update(util);
              
                return RedirectToAction("Index");

            } catch  {
                return View();
            }
        }
    }
}
