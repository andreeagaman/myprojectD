using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Disertatie.Core;
using Disertatie.Core.Models;

namespace Disertatie.Models
{
    public class MembruHomeModel
    {
        public Utilizator Utilizator { get; set; }
        public IList<Utilizator> MembriiDepartament { get; set; }
        public virtual bool EstePaginaDeProfil { get; set; }
        public virtual bool EstePaginaCuProiecte { get; set; }
        public IList<Mesaj> MesajeNoi { get; set; }
        public IList<Mesaj> MesajePrimite { get; set; }
        public IList<Mesaj> MesajeTrimise { get; set; }
        public IList<Proiect> ProiectePropuse { get; set; }
        public IList<Proiect> ProiecteInchise { get; set; }
        public IList<Proiect> ProiecteInLucru { get; set; }
        public IList<Proiect> ProiecteAnulate { get; set; }
        public IList<Anunt> Anunturi { get; set; }
        public CreeazaProiectModel CreeazaProiectModel { get; set; }
        public Proiect Proiect { get; set; }
        public IList<Utilizator> MembriiProiect { get; set; }
        public IList<Utilizator> TotiMembrii { get; set; }
        public Utilizator SefDepartament { get; set; }
        public IList<Utilizator> CoordonatoriColetive { get; set; }
        public IList<InvitatiiProiect> InviitatiiProiectTrimiseInAsteptare { get; set; }
        public IList<InvitatiiProiect> InviitatiiProiectTrimiseAcceptate { get; set; }
        public IList<InvitatiiProiect> InviitatiiProiectTrimiseRespinse { get; set; }
        public IList<InvitatiiProiect> InvitatiiPrimiteProiectInAsteptare { get; set; }
        public IList<InvitatiiProiect> InvitatiiPrimiteProiectAcceptate { get; set; }
        public IList<InvitatiiProiect> InvitatiiPrimiteProiectRespinse { get; set; }
        public IList<CereriProiect> CereriProiectTrimiseInAsteptare { get; set; }
        public IList<CereriProiect> CereriProiectTrimiseAcceptate { get; set; }
        public IList<CereriProiect> CereriProiectTrimiseRespinse { get; set; }
        public IList<CereriProiect> CereriPrimiteProiectInAsteptare { get; set; }
        public IList<CereriProiect> CereriPrimiteProiectAcceptate { get; set; }
        public IList<CereriProiect> CereriPrimiteProiectRespinse { get; set; }
        public IList<Proiect> ProiecteMembru { get; set; }
        public Utilizator MembruCuProiecte { get; set; }
        public EvaluareProiect EvaluareProiect { get; set; }
        public bool EsteDejaMembru { get; set; }
        public IList<Proiect> NoutatiProiecte { get; set; }
        public IList<DocumentIncarcat> DocumenteIncarcate { get; set; }
        public IList<ComentariuProiect> ComentariiProiect { get; set; }
        public Utilizator Membru { get; set; }
        public IList<Anunt> UltimeleAnunturi { get; set; }
        public Anunt Anunt { get; set; }
    }
}