using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Disertatie.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Disertatie.Models
{
    public class CreeazaProiectModel
    {
        [Required(ErrorMessage="Titlul este obligatoriu!")]
        [StringLength(100,ErrorMessage="Titlul nu poate avea mai mult de 100 de caractere!")]
        public string Titlu { get; set; }

        [StringLength(500, ErrorMessage = "Detaliile nu pot avea mai mult de 250 de caractere!")]
        public string Detalii { get; set; }

        [Required(ErrorMessage = "Durata este obligatorie!")]
        [Range(1,120,ErrorMessage="Durata trebuie sa fie mai mare de o luna!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Valoarea trebuie sa fie numerica!")]
        public int Durata { get; set; }
        public IList<ObiectivProiect> ObiectiveGenerale { get; set; }
        public IList<ObiectivProiect> ObiectiveSpecifice { get; set; }
        [Required(ErrorMessage = "Numarul de membrii este obligatoriu!")]
        [Range(1, 20, ErrorMessage = "Un proiect trebuie sa aiba cel putin un membru!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Valoarea trebuie sa fie numerica!")]
        public int NumarMembrii { get; set; }
    }
}