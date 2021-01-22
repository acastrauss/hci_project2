using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using NetworkService.Model;
using System.Collections.ObjectModel;
using System.Windows;


namespace NetworkService.ViewModel
{
    public class View1Model : BindableBase, IDataErrorInfo
    {
        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }
        public MyICommand SearchCommand { get; set; }
        public MyICommand CancelCommand { get; set; }
        public MyICommand TipCommand { get; set; }
        public MyICommand ImeCommand { get; set; }

        public static ObservableCollection<Parking> Parkinzi { get; set; } = new ObservableCollection<Parking>();
        

        public static ObservableCollection<Parking> ParkinziRezerva { get; set; } = new ObservableCollection<Parking>();
        private ObservableCollection<Parking> TrazeniParkinzi { get; set; } = new ObservableCollection<Parking>();

        private bool CanValidateIme = false;
        private bool CanValidateId = false;
        private bool CanValidateStrSearch = false;

        public static List<string> Tipovi_parkinga { get; set; } = new List<string> { "Otvoreni sa naplatom", "Otvoreni bez naplate", "Poluzatvoreni", "Zatvoreni" };
        public static List<string> Slike_parkinga { get; set; } = new List<string> { "Pictures/otvoreni_sanaplatom.jpg", "Pictures/otvoreni_beznaplate.jpg", "Pictures/zatvoreni_polu.jpg", "Pictures/zatvoreni_zgrada.jpg" };
 
        private bool trazi_cancel = false;

        private Parking trenutniParking = new Parking();

        private int tip_ime = -1; // 1 je za tip, 2 je za ime
        private string str_search = string.Empty;        
        
        private string izabrani_tip = string.Empty;

        private string izabrano_ime = string.Empty;
        private int izabrani_id;
        private string path_to_pic = string.Empty;
        public View1Model()
        {
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete);
            SearchCommand = new MyICommand(OnSearch, CanSearch);
            TipCommand = new MyICommand(OnTip);
            ImeCommand = new MyICommand(OnIme);
            CancelCommand = new MyICommand(OnCancel, CanCancel);
            
        }

        public Parking TrenutniParking 
        {
            get { return trenutniParking; }
            set
            {
                if (TrenutniParking != value)
                {
                    trenutniParking = value;
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public void change_value_by_id(int id, double val) 
        {
            if (Parkinzi.Count != 0)
                Parkinzi[id].Val = val;
        }

        public int Tip_ime 
        { 
            get { return tip_ime; }
            set
            {
                tip_ime = value;
                SearchCommand.RaiseCanExecuteChanged();
            } 
        }
        public string Str_search { get => str_search; 
            set
            {
                str_search = value;
                OnPropertyChanged("Str_search");
                SearchCommand.RaiseCanExecuteChanged();
                CanValidateStrSearch = true;
            } 
        }
        public string Izabrani_tip 
        {
            get { return izabrani_tip; }
            set
            {
                if ( izabrani_tip != value)
                {
                    izabrani_tip = value;
                    Path_to_pic = get_me_path(izabrani_tip);
                    OnPropertyChanged("Izabrani_tip");
                }
            } 
        }

        private string get_me_path(string tip)
        {
            string[] naz = tip.Split(' ');
            string temp = String.Join("_", naz);
            //string ret = Directory.GetCurrentDirectory() + temp + ".png";
            //MessageBox.Show(Directory.GetCurrentDirectory()+ "\\..\\..\\" + temp + ".png");
            return Directory.GetCurrentDirectory() + "\\..\\..\\" + temp + ".png";
        }

        public string Izabrano_ime 
        {
            get { return izabrano_ime; }
            set 
            { 
                if (izabrano_ime != value)
                {
                    izabrano_ime = value;
                    OnPropertyChanged("Izabrano_ime");
                    AddCommand.RaiseCanExecuteChanged();
                    CanValidateIme = true;
                }
            } 
        }
        public int Izabrani_id
        {
            get { return izabrani_id; }
            set
            {
                if (izabrani_id != value)
                {
                    izabrani_id = value;
                    OnPropertyChanged("Izabrani_id");
                    AddCommand.RaiseCanExecuteChanged();
                    CanValidateId = true;
                }
            }
        }

        public string Path_to_pic 
        {
            get { return path_to_pic; } 
            set 
            { 
                if (path_to_pic != value)
                {
                    path_to_pic = value;
                    OnPropertyChanged("Path_to_pic");
                    AddCommand.RaiseCanExecuteChanged();
                }
            } 
        }

        private void OnAdd()
        {
            Parking novi = new Parking(Izabrani_id, Izabrano_ime, new TipParkinga(Izabrani_tip, Path_to_pic));
            
            if(Parkinzi.Any<Parking>(x => x.Id == Izabrani_id))
            {
                MessageBox.Show("ID vec postoji!");
                return;
            }

            novi.Validate();
            if (novi.IsValid)
            {
                Parkinzi.Add(novi);
                View3Model.set_all_ids(Parkinzi);
            }


            CanValidateId = false;
            CanValidateIme = false;
        }

        private void OnDelete()
        {
            if (!TrenutniParking.is_empty()) 
                Parkinzi.Remove(TrenutniParking);

            //View2Model.BaseRemove.Execute((TrenutniParking));
        }

        private bool CanSearch()
        {
            if ((Tip_ime != -1) || string.IsNullOrEmpty(str_search))
            {
                return true;
            }
            else return false;
        }

        private void OnSearch()
        {
            TrazeniParkinzi.Clear();

            sacuvaj_parkinge();

            if (tip_ime == 1)
            {
                foreach (Parking parking in Parkinzi)
                {
                    if (str_search == parking.TipParkinga.Ime)
                    {
                        TrazeniParkinzi.Add(parking);
                    }
                }
            }
            else if (tip_ime == 2)
            {
                foreach (Parking parking in Parkinzi)
                {
                    if (str_search == parking.Naziv)
                    {
                        TrazeniParkinzi.Add(parking);
                    }
                }
            }
            zameni_parkinge();
            trazi_cancel = true;
            OnPropertyChanged("Parkinzi");
            CancelCommand.RaiseCanExecuteChanged();
        }

        private void sacuvaj_parkinge()
        {
            ParkinziRezerva.Clear();
            foreach (Parking parking in Parkinzi)
            {
                ParkinziRezerva.Add(parking);
            }
        }

        private void vrati_parkinge()
        {
            Parkinzi.Clear();
            foreach (Parking parking in ParkinziRezerva)
            {
                Parkinzi.Add(parking);
            }
            OnPropertyChanged("Parkinzi");
        }

        private void zameni_parkinge()
        {
            Parkinzi.Clear();
            foreach (Parking parking in TrazeniParkinzi)
            {
                Parkinzi.Add(parking);
            }
            OnPropertyChanged("Parkinzi");
        }

        private void OnTip()
        {
            tip_ime = 1;
            SearchCommand.RaiseCanExecuteChanged();
        }
        private void OnIme()
        {
            tip_ime = 2;
            SearchCommand.RaiseCanExecuteChanged();
        }

        private bool CanCancel() 
        { 
            return trazi_cancel;
        }
    
        private void OnCancel()
        {
            TrazeniParkinzi.Clear();
            vrati_parkinge();
            OnPropertyChanged("TrazeniParkinzi");
            OnPropertyChanged("Parkinzi");
            trazi_cancel = false;
            CanValidateStrSearch = true;
            CancelCommand.RaiseCanExecuteChanged();
            SearchCommand.RaiseCanExecuteChanged();
            AddCommand.RaiseCanExecuteChanged();
        }
        
        public string Error { get { return null; } }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "Ime":
                        if (CanValidateIme)
                            result = this.ValidateAddIme();
                        break;
                    case "Id":
                        if (CanValidateId)
                            result = this.ValidateAddID();
                        break;

                    case "Str_search":
                        if (CanValidateStrSearch)
                            result = this.ValidateSearchValue();
                        break;
                }

                return result;
            }
        }

        private string ValidateSearchValue()
        {
            string result = null;
            double num = 0;

            if (string.IsNullOrEmpty(Str_search) || string.IsNullOrWhiteSpace(Str_search))
            {
                result = "Morate uneti vrednost za pretragu!";
            }
            
            return result;
        }


        private string ValidateAddIme()
        {
            string result = null;
            MessageBox.Show(Izabrano_ime);
            if (string.IsNullOrEmpty(Izabrano_ime) || string.IsNullOrWhiteSpace(Izabrano_ime))
                result = "Ime ne moze biti prazno!";

            return result;
        }

        private string ValidateAddID()
        {
            string result = null;

            MessageBox.Show(izabrani_id.ToString());

            if (Izabrani_id <= 0)
                result = "ID mora biti pozitivan broj!";
            else
            {
                if (Parkinzi.Any<Parking>(x => x.Id == Izabrani_id))
                    result = "ID vec postoji!";

            }
            
            return result;
        }

    }
}
