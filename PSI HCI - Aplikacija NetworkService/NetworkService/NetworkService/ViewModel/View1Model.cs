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
          set => tip_ime = value; 
        }
        public string Str_search { get => str_search; set => str_search = value; }
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
                }
            }
        }

        /*
        public ObservableCollection<Parking> Parkinzi
        {
            get { return parkinzi; }
            set
            {
                parkinzi = value;
                OnPropertyChanged("Parkinzi");
            }
        }
        */
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

            novi.Validate();
            if (novi.IsValid)
            {
                Parkinzi.Add(novi);
            }
            else
            {
                MessageBox.Show("Nije validan");
            }
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

            //MessageBox.Show(tip_ime.ToString());
            //MessageBox.Show(str_search);
            
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
                    //MessageBox.Show(parking.ToString());
                    if (str_search == parking.Naziv)
                    {
                        TrazeniParkinzi.Add(parking);
                    }
                }
            }
            zameni_parkinge();

            tip_ime = -1;
            str_search = string.Empty;
            trazi_cancel = true;
            OnPropertyChanged("Parkinzi");
            CancelCommand.RaiseCanExecuteChanged();
        }

        private void sacuvaj_parkinge()
        {
            ParkinziRezerva = new ObservableCollection<Parking>(Parkinzi);
        }

        private void vrati_parkinge()
        {
            Parkinzi = new ObservableCollection<Parking>(ParkinziRezerva);
        }

        private void zameni_parkinge()
        {
            Parkinzi = new ObservableCollection<Parking>(TrazeniParkinzi);
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
            OnPropertyChanged("TrazeniServeri");
            OnPropertyChanged("Parkinzi");
            trazi_cancel = false;
            CancelCommand.RaiseCanExecuteChanged();
            AddCommand.RaiseCanExecuteChanged();
        }

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

    }
}
