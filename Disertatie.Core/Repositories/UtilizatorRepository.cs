using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disertatie.Core.Repositories;
using NHibernate;
using NHibernate.Criterion;
using System.Web;
using Disertatie.Core.Models;
using System.Web.UI.WebControls;
using System.IO;

namespace Disertatie.Core.Repositories
{
    public class UtilizatorRepository: RepositoryBase<long,Utilizator>
    {
        public UtilizatorRepository(ISession session) : base(session) { }
        public UtilizatorRepository() : base() { }

        #region Membership Utility Methods

        internal ICriteria GetUserByEmailCriteria(string email) {
            return Session.CreateCriteria<Utilizator>().Add(Restrictions.NaturalId().Set("Email", email));
        }

        internal void FlushSession() {
            this.Session.Flush();
        }

        public Utilizator GetByEmail(string email)
        {
            return GetUserByEmailCriteria(email).UniqueResult<Utilizator>();
        }

        public Utilizator GetByEmail(string email, string appName)
        {
            return GetUserByEmailCriteria(email)
                //.Add(Expression.Eq("ApplicationName", appName))
                .UniqueResult<Utilizator>();
        }

        public bool Authenticate(string email, string password, out Utilizator user)
        {

            user = GetUserByEmailCriteria(email)
                .Add(Restrictions.Eq("Parola", password))
                .UniqueResult<Utilizator>();

            if (user == null)
                return false;

            return true;
        }

        public bool CheckForEmail(string email) {
            return this.QueryAll().Any(r => r.Email == email);
        }

        public Utilizator GetUserByName(string userName, string appName)
        {
            return Session.CreateCriteria<Utilizator>()
                .Add(Restrictions.NaturalId().Set("Username", userName))
                //.Add(Expression.Eq("ApplicationName", appName))
                .UniqueResult<Utilizator>();
        }


        public Utilizator GetUserByN(string Name, string appName)
        {
            return Session.CreateCriteria<Utilizator>()
                .Add(Restrictions.NaturalId().Set("Name", Name))
                //.Add(Expression.Eq("ApplicationName", appName))
                .UniqueResult<Utilizator>();
        }

        public string GetUserNameByEmail(string email, string appName) {
            return Session.CreateCriteria<Utilizator>()
                .Add(Expression.Eq("Email", email))
                //.Add(Expression.Eq("ApplicationName", appName))
                .SetProjection(Projections.Property("Username"))
                .UniqueResult<string>();
        }

        public IList<Utilizator> FindUsersByEmail(string email, int pageIndex, int pageSize, string appName)
        {
            return Session.CreateCriteria<Utilizator>()
                .Add(Expression.Like("Email", email, MatchMode.Anywhere))
                //.Add(Expression.Eq("ApplicationName", appName))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .List<Utilizator>();
        }

        public IList<Utilizator> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, string appName)
        {
            return Session.CreateCriteria<Utilizator>()
                .Add(Expression.Like("Username", usernameToMatch, MatchMode.Anywhere))
                //.Add(Expression.Eq("ApplicationName", appName))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .List<Utilizator>();
        }

        public IList<Utilizator> GetAllUsers(int pageIndex, int pageSize, string appName)
        {
            return Session.CreateCriteria<Utilizator>()
                //.Add(Expression.Eq("ApplicationName", appName))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .List<Utilizator>();
        }
        public IList<Utilizator> GetAllUsers()
        {
            return Session.CreateCriteria<Utilizator>()
                //.Add(Expression.Eq("ApplicationName", appName))

                .List<Utilizator>();
        }
        public PageList<Utilizator> GetMembriiDepartament(int pageIndex, int pageSize,string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Aprobat == true && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public List<Utilizator> GetMembriiDepartament(string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Aprobat==true && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToList();
        }
        public List<Utilizator> GetTotiMembriiDepartament()
        {
            var r = new UtilizatorRepository();

            return QueryAll()
                .Where(d => d.Aprobat == true && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToList();
        }
        public PageList<Anunt> GetAnunturi(int pageIndex, int pageSize)
        {
            var r = new Repository<long,Anunt>();

            return r.QueryAll()
                .Where(a=>a.Activ==true )
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.DataCreare)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDepartamentDupaColectiv(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Colectiv.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDepartamentDupaColectivBP(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 1 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetUtilizatoriNume(int pageIndex, string filter)
        {
            var r = new UtilizatorRepository();
            return r.QueryAll().Where(a => a.Nume==filter).OrderBy(a => a.Nume).ToPageList(pageIndex);
        }
        public IList<Colectiv> GetColective()
        {
            var colectiveRep = new Disertatie.Core.Repositories.Repository<long, Colectiv>();
            var colective = colectiveRep.QueryAll().ToList();
            return colective;
        }
        public IList<Rol> GetRoluri()
        {
            var roluriRep = new Disertatie.Core.Repositories.Repository<int, Rol>();
            var roluri = roluriRep.QueryAll().Where(a=>a.Activ==true).ToList();
            return roluri;
        }
        public PageList<Utilizator> GetMembriiDupaColectivPA(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 2 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDupaColectivBD(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 3 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDupaColectivSCO(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 4 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDupaColectivC1(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 5 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDupaColectivC2(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 6 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public PageList<Utilizator> GetMembriiDupaColectivBazeleTic(int pageIndex, int pageSize, string username)
        {
            var r = new UtilizatorRepository();

            return QueryAll().Where(d => d.Username != username && d.Colectiv.Id == 7 && (d.Roluri.Any(ro => ro.NumeRol != "Administrator") == true))
                //.OrderBy(d => d.DataDeschidere)
                .OrderBy(d => d.Nume)
                .ToPageList(pageIndex, pageSize);
        }
        public int GetNumberOfUsersOnline(DateTime compareTime, string appName) {
            return Session.CreateCriteria<Utilizator>()
                //.Add(Expression.Eq("ApplicationName", appName))
                .Add(Expression.Ge("LastActivityDate", compareTime))
                .SetProjection(Projections.Count("Id"))
                .UniqueResult<int>();
        }
        public Utilizator GetAndLoadProxies(long id)
        {
            var utilizator = this.Get(id);
            if (utilizator != null)
            {
                try
                {
                    NHibernateUtil.Initialize(utilizator.PozaProfil);
                    NHibernateUtil.Initialize(utilizator.Colectiv);
                    
                }
                catch (Exception) { }
            }
            return utilizator;
        }
        public long GetNrMesajeNoiPtUtilizator(string username)
        {
            return Session
                .GetNamedQuery("GetNrMesajeNoi")
                .SetAnsiString("username", username)
                .UniqueResult<long>();
        }
        #endregion

        public void AddLoginEntry(Utilizator user, HttpRequestBase request)
        {            
            Session.Save(new Disertatie.Core.Models.UtilizatorAutentificari(user, request.UserHostAddress, request.UserHostName, request.UserAgent));
        }

        public void AddLoginEntry(int userId, HttpRequestBase request) {
            AddLoginEntry(Session.Load<Utilizator>(userId), request);
        }

        public override System.Linq.Expressions.Expression<Func<Utilizator, object>> DefaultOrderBy
        {
            get { return u => u.Username; }
        }

        public IList<Mesaj> GetMesajeNoi(int pageIndex, int pageSize, Utilizator util)
        {
            var mesajeRep = new Repository<long, Mesaj>();
            var mesajeNoi = mesajeRep.QueryAll().Where(a => (a.Destinatar == util) && (a.Citit == false) && (a.Activ == true)).ToPageList(pageIndex, pageSize);
            return mesajeNoi;
        }
        public IList<Proiect> GetProiecteMembru(int pageIndex, int pageSize, Utilizator util)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiecteMembru = proiecteRep.QueryAll().Where(a => (a.Initiator == util));// OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            var membriiRep=new Repository<long,MembriiProiect>();
            var proiecte=membriiRep.QueryAll().Where(a=>a.Utilizator==util && a.Activ==true).Select(a=>a.Proiect);
            proiecteMembru.Union(proiecte);
            return proiecteMembru.OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
        }
        public IList<Proiect> GetProiectePropuse(int pageIndex, int pageSize, Utilizator util)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiecteDeschise = proiecteRep.QueryAll().Where(a => (a.Initiator == util) && (a.Status == 1)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteDeschise;
        }
        public IList<Proiect> GetProiecteInColaborarePropuse(int pageIndex, int pageSize, Utilizator util)
        {
            var membriiRep = new Repository<long, MembriiProiect>();
            var proiecteInColaborarePropuse = membriiRep.QueryAll().Where(a => a.Utilizator == util && a.Activ == true && a.Proiect.Status==1).Select(a => a.Proiect).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);

            return proiecteInColaborarePropuse;
        }
        public IList<Proiect> GetProiecteInColaborareInchise(int pageIndex, int pageSize, Utilizator util)
        {
            var membriiRep = new Repository<long, MembriiProiect>();
            var proiecteInColaborareInchise = membriiRep.QueryAll().Where(a => a.Utilizator == util && a.Activ == true && a.Proiect.Status == 3).Select(a => a.Proiect).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);

            return proiecteInColaborareInchise;
        }
        public IList<Proiect> GetProiecteInColaborareInLucru(int pageIndex, int pageSize, Utilizator util)
        {
            var membriiRep = new Repository<long, MembriiProiect>();
            var proiecteInColaborareInLucru = membriiRep.QueryAll().Where(a => a.Utilizator == util && a.Activ == true && a.Proiect.Status == 2).Select(a => a.Proiect).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteInColaborareInLucru;
        }
        public IList<Proiect> GetProiecteInColaborareAnulate(int pageIndex, int pageSize, Utilizator util)
        {
            var membriiRep = new Repository<long, MembriiProiect>();
            var proiecteInColaborareAnulate = membriiRep.QueryAll().Where(a => a.Utilizator == util && a.Activ == true && a.Proiect.Status == 4).Select(a => a.Proiect).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteInColaborareAnulate;
        }
        public IList<Proiect> GetProiecteInchise(int pageIndex, int pageSize, Utilizator util)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiecteInchise = proiecteRep.QueryAll().Where(a => (a.Initiator == util) && (a.Status == 3)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteInchise;
        }
        public IList<Proiect> GetProiecteInLucru(int pageIndex, int pageSize, Utilizator util)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiecteInAsteptare = proiecteRep.QueryAll().Where(a => (a.Initiator == util) && (a.Status == 2)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteInAsteptare;
        }
        public IList<Proiect> GetProiecteAnulate(int pageIndex, int pageSize, Utilizator util)
        {
            var proiecteRep = new Repository<long, Proiect>();
            var proiecteAnulate = proiecteRep.QueryAll().Where(a => (a.Initiator == util) && (a.Status == 4)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return proiecteAnulate;
        }
    
        public PageList<Anunt> GetAnunturileMele(int pageIndex, int pageSize, string username)
        {
            var r = new Repository<long,Anunt>();

            return r.QueryAll().Where(d => d.Utilizator.Username== username && d.Activ==true)
                //.OrderBy(d => d.DataDeschidere)
                .OrderByDescending(d => d.DataCreare)
                .ToPageList(pageIndex, pageSize);
        }

        public IList<Mesaj> GetMesajePrimite(int pageIndex, int pageSize, Utilizator util)
        {
            var mesajeRep = new Repository<long, Mesaj>();
            var mesajePrimite = mesajeRep.QueryAll().Where(a => (a.Destinatar == util) && (a.Citit == true) && (a.Activ == true)).ToPageList(pageIndex, pageSize);
            return mesajePrimite;
        }
        public IList<Mesaj> GetMesajeTrimise(int pageIndex, int pageSize, Utilizator util)
        {
            var mesajeRep = new Repository<long, Mesaj>();
            var mesajeTrimise = mesajeRep.QueryAll().Where(a => a.Expeditor == util && a.Activ == true).ToPageList(pageIndex, pageSize);
            return mesajeTrimise;
        }
        public IList<InvitatiiProiect> GetInvitatiiProiectInAsteptare(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiInAsteptare = invitatiiRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status ==1)).OrderByDescending(a=>a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiInAsteptare;
        }
        public IList<InvitatiiProiect> GetInvitatiiProiectAcceptate(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiAcceptate = invitatiiRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status == 2)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiAcceptate;
        }
        public IList<InvitatiiProiect> GetInvitatiiProiectRespinse(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiRespinse = invitatiiRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status == 3)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiRespinse;
        }
        public IList<CereriProiect> GetCereriProiectInAsteptare(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriInAsteptare = cereriRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status == 1)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriInAsteptare;
        }
        public IList<CereriProiect> GetCereriProiectAcceptate(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriAcceptate = cereriRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status == 2)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriAcceptate;
        }
        public IList<CereriProiect> GetCereriProiectRespinse(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriRespinse = cereriRep.QueryAll().Where(a => (a.Expeditor == util) && (a.ActivaExpeditor == true) && (a.Status == 3)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriRespinse;
        }
        public IList<InvitatiiProiect> GetInvitatiiPrimiteProiectInAsteptare(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiInAsteptare = invitatiiRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 1)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiInAsteptare;
        }
        public IList<InvitatiiProiect> GetInvitatiiPrimiteProiectAcceptate(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiAcceptate = invitatiiRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 2)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiAcceptate;
        }
        public IList<InvitatiiProiect> GetInvitatiiPrimiteProiectRespinse(int pageIndex, int pageSize, Utilizator util)
        {
            var invitatiiRep = new Repository<long, InvitatiiProiect>();
            var invitatiiRespinse = invitatiiRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 3)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return invitatiiRespinse;
        }
        public IList<CereriProiect> GetCereriPrimiteProiectInAsteptare(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriInAsteptare = cereriRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 1)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriInAsteptare;
        }
        public IList<CereriProiect> GetCereriPrimiteProiectAcceptate(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriAcceptate = cereriRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 2)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriAcceptate;
        }
        public IList<CereriProiect> GetCereriPrimiteProiectRespinse(int pageIndex, int pageSize, Utilizator util)
        {
            var cereriRep = new Repository<long, CereriProiect>();
            var cereriRespinse = cereriRep.QueryAll().Where(a => (a.Destinatar == util) && (a.ActivaDestinatar == true) && (a.Status == 3)).OrderByDescending(a => a.DataCreare).ToPageList(pageIndex, pageSize);
            return cereriRespinse;
        }
    }
}
