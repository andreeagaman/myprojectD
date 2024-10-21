using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class UtilizatorAutentificari
    {
        public virtual int Id { get; protected set; }
        public virtual Utilizator Utilizator { get; protected set; }
        public virtual DateTime DataAutentificarii { get; set; }
        public virtual string UtilizatorAdresaHost { get; set; }
        public virtual string UtilizatorNumeHost { get; set; }
        public virtual string AgentUtilizat { get; set; }

        // NHibernate Proxy use only
        protected UtilizatorAutentificari() {
            
        }

        public UtilizatorAutentificari(Utilizator utilizator) {
            if (utilizator == null)
                throw new ArgumentNullException("utilizator");
            Utilizator = utilizator;
            DataAutentificarii = DateTime.Now;
        }

        public UtilizatorAutentificari(Utilizator utilizator, string adresaHost, string numeHost, string agentUilizat)
            : this(utilizator)
        {
            if (adresaHost == null)
                throw new ArgumentNullException("adresaHost");

            UtilizatorAdresaHost = adresaHost;
            UtilizatorNumeHost = numeHost;
            AgentUtilizat = agentUilizat;
        }
    }
}
