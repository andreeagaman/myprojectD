using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Disertatie.Core.Models;

namespace Disertatie.Models
{
    public class TrimiteInvitatieModel
    {
        public Proiect Proiect {get; set;}
        public Utilizator Expeditor { get; set; }
        public List<Utilizator> Destinatari { get; set; }
        public List<Utilizator> Membrii { get; set; }
    }
}