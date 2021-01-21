using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Parking : ValidationBase
    {
        private int id;
        private string naziv;
        private TipParkinga tipParkinga;
        private double val;

        public Parking()
        {
            id = -1;
            naziv = string.Empty;
            tipParkinga = new TipParkinga();
            val = -1;
        }

        public Parking(int id, string naziv, TipParkinga tipParkinga)
        {
            this.id = id;
            this.naziv = naziv;
            this.tipParkinga = tipParkinga;
            val = -1;
        }

        public int Id
        {
            get { return id; }
            set
            {
                if(id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Naziv
        {
            get { return naziv; }
            set
            {
                if(naziv != value)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }

        public TipParkinga TipParkinga
        {
            get { return tipParkinga; }
            set
            {
                if (tipParkinga != value)
                {
                    tipParkinga = value;
                    OnPropertyChanged("TipParkinga");
                }
            }
        }

        public double Val 
        {
            get { return val; }
            set
            {
                if(val != value)
                {
                    val = value;
                    OnPropertyChanged("Val");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (this.id <= 0)
            {
                this.ValidationErrors["Id"] = "ID mora biti pozitivan broj!";
            }
            if (string.IsNullOrEmpty(this.naziv))
            {
                this.ValidationErrors["Naziv"] = "Naziv ne sme biti prazan!";
            }
            if(! this.tipParkinga.is_valid()) 
            {
                this.ValidationErrors["TipParkinga"] = "Nevalidan tip parkinga!";
            }
            if(val <= 0 || val >= 0.9)
            {
                this.ValidationErrors["Val"] = "Vrednost mora pozitivan broj do 90 procenata (0.9)!";
            }
        }

        public bool is_empty()
        {   // da li je prazan parking
            
            return id == -1 || string.IsNullOrEmpty(naziv) || tipParkinga.is_empty() || val == -1; 
        }

        public override bool Equals(object obj)
        {
            Parking temp = (Parking)obj;

            return temp.Id == Id && Naziv == temp.Naziv && TipParkinga == temp.TipParkinga && Val == temp.Val;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
