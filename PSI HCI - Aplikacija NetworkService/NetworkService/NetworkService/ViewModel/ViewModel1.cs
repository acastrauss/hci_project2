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
        public static List<TipParkinga> tipovi { get; set; } = new List<TipParkinga>
        {
            new TipParkinga("otvoreni_sanaplatom", "otvoreni_sanaplatom.jpg"),
            new TipParkinga("otvoreni_beznaplate", "otvoreni_beznaplate.jpg"),
            new TipParkinga("zatvoreni_polu", "zatvoreni_polu.jpg"),
            new TipParkinga("zatvoreni_zgrada", "zatvoreni_zgrada.jpg")
        };

        private ObservableCollection<Parking> TrazeniParkinzi { get; set; } = new ObservableCollection<Parking>();

        private bool trazi_cancel = false;
        private int tip_ime = -1; // 1 je za tip, 2 je za ime
        private string str_search = string.Empty;
        private Parking trenutniParking = new Parking();

        public ViewModel1()
        {
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete);
            SearchCommand = new MyICommand(OnSearch, CanSearch);
            TipCommand = new MyICommand(OnTip);
            ImeCommand = new MyICommand(OnIme);
            CancelCommand = new MyICommand(OnCancel, CanCancel);
        }

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        public int Tip_ime { get => tip_ime; set => tip_ime = value; }
        public string Str_search { get => str_search; set => str_search = value; }
        public Parking TrenutniParking { get => trenutniParking; set => trenutniParking = value; }

        private void OnAdd()
        {
            TrenutniParking.Validate();
            if (TrenutniParking.IsValid)
            {
                Parkinzi.Add(new Parking()
                {
                    Id = TrenutniParking.Id,
                    Naziv = TrenutniParking.Naziv,
                    TipParkinga = TrenutniParking.TipParkinga,
                    Val = TrenutniParking.Val
                });
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
    }
}
