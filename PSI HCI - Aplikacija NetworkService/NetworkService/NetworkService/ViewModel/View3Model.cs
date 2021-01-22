using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using NetworkService.Model;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace NetworkService.ViewModel
{
    public class View3Model : BindableBase, IDataErrorInfo
    {
        private const string fname = "logs.txt"; 

        public MyICommand SearchCommand { get; set; }
        public MyICommand CancelCommand { get; set; }

        private bool CanSearch()
        {
            return selected_id != 0;
        }

        private bool CanCancel()
        {
            return Y1 != 0 || Y2 != 0 || Y3 != 0 || Y4 != 0 || Y5 != 0; 
        }
        private void OnCancel()
        {
            Y1 = 0;
            Y2 = 0;
            Y3 = 0;
            Y4 = 0;
            Y5 = 0;
            selected_id = 0;
            SearchCommand.RaiseCanExecuteChanged();
        }

        private void OnSearch()
        {

            if (!load_data()) return;
            for (int i = vals.Count - 1; i >= 0; i--)
            {
                if (i == 4)
                {
                    Y1 = (int)(vals[i] / 90.0 * (220 - 67)); // skalirano
                }
                else if (i == 3)
                {
                    Y2 = (int)(vals[i] / 90.0 * (220 - 67));
                }
                else if (i == 2)
                {
                    Y3 = (int)(vals[i] / 90.0 * (220 - 67));
                }
                else if(i == 1)
                {
                    Y4 = (int)(vals[i] / 90.0 * (220 - 67));
                }
                else if(i == 0)
                {
                    Y5 = (int)(vals[0] / 90.0 * (220 - 67));
                }
            }
        }

        private bool load_data()
        {
            vals.Clear();

            int indx = 0;
            for (int i = 0; i < all_parkings.Count; i++)
            {
                if (all_parkings[i].Id == selected_id)
                {
                    indx = i;
                    break;
                }
            }

            string[] lines = System.IO.File.ReadAllLines(fname);

            int found = 0;
            
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string temp = lines[i].Split('_')[1];
                string[] arr_s = temp.Split(':');
                int id_f = int.Parse(arr_s[0]);
                int val_f = int.Parse(arr_s[1]);

                if (id_f == indx)
                {
                    vals.Add(val_f);
                    if (++found == 5) break;
                }
            }

            return found != 0;
        }

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();
        
        private List<int> vals = new List<int>();

        private static List<int> all_ids = new List<int>();
        private int selected_id = 0;

        private static List<Parking> all_parkings = new List<Parking>();


        public List<int> All_ids 
        { 
            get => all_ids; 
            set 
            {
                if(all_ids != value)
                {
                    all_ids = value;
                    OnPropertyChanged("All_ids");
                }
            }
        }
        public int Selected_id 
        { 
            get => selected_id;
            set
            {
                if(selected_id != value)
                {
                    selected_id = value;
                    OnPropertyChanged("Selected_id");
                    SearchCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public static void set_all_ids(ObservableCollection<Parking> parkings)
        {
            all_parkings = new List<Parking>(parkings.ToList());
            all_ids.Clear();
            foreach (Parking parking in all_parkings)
            {
                all_ids.Add(parking.Id);
            }
        }

        private int y1;
        private int y2;
        private int y3;
        private int y4;
        private int y5;

        private string color1;
        private string color2;
        private string color3;
        private string color4;
        private string color5;

        private string time1;
        private string time2;
        private string time3;
        private string time4;
        private string time5;

        private string type1;
        private string type2;
        private string type3;
        private string type4;
        private string type5;

        public int Y1
        {
            get { return y1; }
            set
            {
                if (y1 != value)
                {
                    y1 = value;
                    OnPropertyChanged("Y1");
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public int Y2
        {
            get { return y2; }
            set
            {
                if (y2 != value)
                {
                    y2 = value;
                    OnPropertyChanged("Y2");
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public int Y3
        {
            get { return y3; }
            set
            {
                if (y3 != value)
                {
                    y3 = value;
                    OnPropertyChanged("Y3");
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public int Y4
        {
            get { return y4; }
            set
            {
                if (y4 != value)
                {
                    y4 = value;
                    OnPropertyChanged("Y4");
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public int Y5
        {
            get { return y5; }
            set
            {
                if (y5 != value)
                {
                    y5 = value;
                    OnPropertyChanged("Y5");
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Color1
        {
            get
            {
                return color1;
            }
            set
            {
                if (color1 != value)
                {
                    color1 = value;
                    OnPropertyChanged("Color1");
                }
            }
        }
        public string Color2
        {
            get
            {
                return color2;
            }
            set
            {
                if (color2 != value)
                {
                    color2 = value;
                    OnPropertyChanged("Color2");
                }
            }
        }
        public string Color3
        {
            get
            {
                return color3;
            }
            set
            {
                if (color3 != value)
                {
                    color3 = value;
                    OnPropertyChanged("Color3");
                }
            }
        }
        public string Color4
        {
            get
            {
                return color4;
            }
            set
            {
                if (color4 != value)
                {
                    color4 = value;
                    OnPropertyChanged("Color4");
                }
            }
        }
        public string Color5
        {
            get
            {
                return color5;
            }
            set
            {
                if (color5 != value)
                {
                    color5 = value;
                    OnPropertyChanged("Color5");
                }
            }
        }

        public string Time1
        {
            get { return time1; }
            set
            {
                if (time1 != value)
                {
                    time1 = value;
                    OnPropertyChanged("Time1");
                }
            }
        }
        public string Time2
        {
            get { return time2; }
            set
            {
                if (time2 != value)
                {
                    time2 = value;
                    OnPropertyChanged("Time2");
                }
            }
        }
        public string Time3
        {
            get { return time3; }
            set
            {
                if (time3 != value)
                {
                    time3 = value;
                    OnPropertyChanged("Time3");
                }
            }
        }
        public string Time4
        {
            get { return time4; }
            set
            {
                if (time4 != value)
                {
                    time4 = value;
                    OnPropertyChanged("Time4");
                }
            }
        }
        public string Time5
        {
            get { return time5; }
            set
            {
                if (time5 != value)
                {
                    time5 = value;
                    OnPropertyChanged("Time5");
                }
            }
        }

        public string Type1
        {
            get { return type1; }
            set
            {
                if (type1 != value)
                {
                    type1 = value;
                    OnPropertyChanged("Type1");
                }
            }
        }
        public string Type2
        {
            get { return type2; }
            set
            {
                if (type2 != value)
                {
                    type2 = value;
                    OnPropertyChanged("Type2");
                }
            }
        }
        public string Type3
        {
            get { return type3; }
            set
            {
                if (type3 != value)
                {
                    type3 = value;
                    OnPropertyChanged("Type3");
                }
            }
        }
        public string Type4
        {
            get { return type4; }
            set
            {
                if (type4 != value)
                {
                    type4 = value;
                    OnPropertyChanged("Type4");
                }
            }
        }
        public string Type5
        {
            get { return type5; }
            set
            {
                if (type5 != value)
                {
                    type5 = value;
                    OnPropertyChanged("Type5");
                }
            }
        }

        public View3Model()
        {
            Y1 = 0;
            Y2 = 0;
            Y3 = 0;
            Y4 = 0;
            Y5 = 0;
            Color1 = "#ff0000";
            Color2 = "#ff0000";
            Color3 = "#ff0000";
            Color4 = "#ff0000";
            Color5 = "#ff0000";
            
            SearchCommand = new MyICommand(OnSearch, CanSearch);
            CancelCommand = new MyICommand(OnCancel, CanCancel);
        }


    }
}
