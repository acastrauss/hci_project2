using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class TipParkinga
    {
        private string ime;
        private string slika;


        public TipParkinga()
        {
            ime = string.Empty; 
            slika = string.Empty;
        }

        public TipParkinga(string ime, string slika)
        {
            this.ime = ime;
            this.slika = slika;
        }

        public TipParkinga(TipParkinga tp)
        {
            Ime = tp.ime;
            Slika = tp.slika;
        }
        public string Ime { get => ime; set => ime = value; }

        public string Slika { get => slika; set => slika = value; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            TipParkinga tp = (TipParkinga)obj;
            return tp.ime == this.ime && tp.slika == this.slika;
        }

        public bool is_valid() 
        {
            if (string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(slika))
            {
                return false;
            }
            else return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    
        public bool is_empty()
        {
            return string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(slika);
        }
    }
}
