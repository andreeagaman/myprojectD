using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class Rol : IEquatable<Rol>
    {
        public virtual int Id { get; set; }
        public virtual string NumeRol { get; set; }
        public virtual string NumeAplicatie { get; set; }
        public virtual bool Activ { get; set; }
        public virtual ICollection<Utilizator> RoluriUtilizator { get; set; }

        public Rol() {
            this.Activ = true;
        }

        public Rol(string roleName) {
            this.Activ = true;
            this.NumeRol = roleName;
        }

        public Rol(string numeRol, string numeAplicatie) : this(numeRol) {
            this.NumeAplicatie = numeAplicatie;
        }

        public override string ToString() {
            return string.Format("{0}/{1}", this.NumeAplicatie, this.NumeRol);
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as Rol);                       
        }

        public override int GetHashCode() {
            unchecked {
                return this.NumeRol.GetHashCode() ^ (97 * ((this.NumeAplicatie ?? string.Empty).GetHashCode()));
            }
        }

        #region IEquatable<Role> Members

        public virtual bool Equals(Rol other) {

            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.NumeAplicatie == other.NumeAplicatie && this.NumeRol == other.NumeRol;
        }

        #endregion
    }
}
