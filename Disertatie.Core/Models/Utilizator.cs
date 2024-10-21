using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class Utilizator
    {
        public virtual long Id { get; set; }
        public virtual DateTime DataCreare { get; set; }
        public virtual DateTime DataModificare { get; set; }
        public virtual string Username { get; set; }
        public virtual string Parola { get; set; }        
        public virtual string IntrebareParola { get; set; }
        public virtual string RaspunsParola { get; set; }
        public virtual string Nume { get; set; }
        public virtual string Prenume { get; set; }
        public virtual string PozaProfil { get; set; }
        public virtual DateTime? DataNasterii { get; set; }
        public virtual string Telefon { get; set; }
        public virtual string Email { get; set; }
        
        public virtual string Comentariu { get; set; }
        public virtual string NumeAplicatie { get; set; }
        public virtual bool Aprobat { get; set; }
        public virtual DateTime? DataUltimeiActivitati { get; set; }
        public virtual DateTime? DataUltimeiAutentificari { get; set; }
        public virtual DateTime? DataUltimeiModificariParola { get; set; }
        public virtual bool Online { get; set; }
        public virtual bool Blocat { get; set; }
        public virtual DateTime? DataUltimeiBlocari { get; set; }
        public virtual int NrAutentificariEsuate { get; set; }
        public virtual DateTime? DataIncepereFereastraAutentificareEsuata { get; set; }
        public virtual int NrRaspunsuriEsuate { get; set; }
        public virtual DateTime? DataIncepereFereastraRaspunsEsuat { get; set; }
        public virtual DateTime? DataAutentificariiAnterioare { get; set; }
        public virtual ICollection<Rol> Roluri { get; set; }
        public virtual Colectiv Colectiv { get; set; }
        public virtual string Titlu { get; set; }
        public Utilizator() {
            this.DataCreare = DateTime.Now;
        }
        public virtual string NumeComplet
        {
            get
            {
                return Nume + ' ' + Prenume;
            }
            protected set
            {
                
            }
        }

        public Utilizator(string username, string email) {
            this.Username = username;
            this.Email = email;
            this.DataCreare = DateTime.Now;
            this.DataModificare = DateTime.Now;
        }

        public Utilizator(string email) : this(email, email) { }

        public override string ToString() {
            return this.Username;
        }



        public virtual bool IsInRole(string roleName)
        {
            return this.Roluri.Any(r => r.NumeRol == roleName);
        }

        #region Static Members
        private static readonly string chars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string GenereazaParolaAleatorie(int length) {
            var rnd = new Random();
            var rndChars = new char[length];
            for (int i = 0; i < length; i++) {
                rndChars[i] = chars[rnd.Next(chars.Length)];
            }
            return new string(rndChars);
        }
        public static string GenereazaParolaAleatorie()
        {
            return GenereazaParolaAleatorie(6);
        } 
        #endregion
    }
}
