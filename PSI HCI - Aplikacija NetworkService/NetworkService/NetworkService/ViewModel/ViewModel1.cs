using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using NetworkService.Model;
using System.Collections.ObjectModel;

namespace NetworkService.ViewModel
{
    public class ViewModel1 : BindableBase, IDataErrorInfo
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

        public static List<string> Tipovi_parkinga { get; set; } = new List<string> { "otvoreni_sanaplatom", "otvoreni_beznaplate", "zatvoreni_polu", "zatvoreni_zgrada" };
        public static List<string> Slike_parkinga { get; set; } = new List<string> { "Pictures/otvoreni_sanaplatom.jpg", "Pictures/otvoreni_beznaplate.jpg", "Pictures/zatvoreni_polu.jpg", "Pictures/zatvoreni_zgrada.jpg" };

        
        private bool trazi_cancel = false;

        private Parking trenutniParking = new Parking();

        private int tip_ime = -1; // 1 je za tip, 2 je za ime
        private string str_search = string.Empty;        
        
        private string izabrani_tip = string.Empty;

        private string izabrano_ime = string.Empty;
        private int izabrani_id = 0;
        private string path_to_pic = string.Empty;
        public ViewModel1()
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

        public int Tip_ime 
        { 
            get { return tip_ime; }
          set => tip_ime = value; 
        }
        public string Str_search { get => str_search; set => str_search = value; }
        public string Izabrani_tip { get => izabrani_tip; set => izabrani_tip = value; }

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

        public string Path_to_pic 
        {
            get { return path_to_pic; } 
            set 
            { 
                if (path_to_pic != value)
                {
                    path_to_pic = value;
                    OnPropertyChanged("Path_to_pic");
                }
            } 
        }

        private void OnAdd()
        {
            Parking novi = new Parking(izabrani_id, Izabrano_ime, new TipParkinga(Izabrani_tip, Path_to_pic));

            novi.Validate();
            if (novi.IsValid)
            {
                Parkinzi.Add(novi);
            }
        }

        private void OnDelete()
        {
            if (!TrenutniParking.is_empty()) 
                Parkinzi.Remove(TrenutniParking);
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
        }

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

    }
}
