using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using NetworkService.Model;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace NetworkService.ViewModel
{
    public class View2Model : BindableBase, IDataErrorInfo
    {
        public static Parking draggItm = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex = 0;

        private ListView lv;
        public BindingList<Parking> Items { get; set; }
        //komande 

        static List<Parking> list_park = new List<Parking>(View1Model.Parkinzi);

        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<Parking> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }//on drop
        public MyICommand<Canvas> FreeCommand { get; set; }//on free
        public MyICommand<Canvas> DOCommand { get; set; }//drag over
        public MyICommand<Canvas> DLCommand { get; set; }//drag over leavew
        public MyICommand<Canvas> LCommand { get; set; }//on load
        public MyICommand<ListView> LLWCommand { get; set; }
        //public static MyICommand<object> BaseRemove { get; set; }

        public List<Canvas> CanvasList { get; set; } = new List<Canvas>();

        //obj koji su dodati u canvas
        public static Dictionary<string, Parking> CanvasObj { get; set; } = new Dictionary<string, Parking>();
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");

            }
        }
        //napraviti komande  

        public View2Model()
        {
            Items = new BindingList<Parking>();

            foreach (Parking re in View1Model.Parkinzi)
            {
                exists = false;
                foreach (Parking ReCn in CanvasObj.Values)
                {
                    if (re.Id == ReCn.Id)
                    {
                        exists = true;
                        break;
                    }

                }
                if (exists == false)
                    Items.Add(new Parking(re));
            }
            //komande 
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<Parking>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            DOCommand = new MyICommand<Canvas>(OnDragOver);
            DLCommand = new MyICommand<Canvas>(OnDragLeave);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);
            //BaseRemove = new MyICommand<object>(OnBaseRemove);
        }
        /*
        public void OnBaseRemove(object obj)
        {
            //Items.Remove((Parking)obj);

            // nadji ga u canvasu 
            //FreeCommand.Execute(obj);

            list_park = new BindingList<Parking>(View1Model.Parkinzi).ToList();

            List<Parking> curr = new List<Parking>(Items);
            Items = new BindingList<Parking>();
            foreach (Parking item in list_park)
            {
                if (curr.Contains(item))
                    Items.Add(item);
            }
        }
        */
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        public void OnLLW(ListView listview)
        {
            //lv dobija vrednost ListView-a gde su obj
            lv = listview;
        }
        public void OnLoad(Canvas c)
        {
            if (CanvasObj.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();//za jednostavniji prikaz slike
                logo.BeginInit();//signal za pocetak inicijalizacije
                string temp = CanvasObj[c.Name].TipParkinga.Ime + ".png";
                //MessageBox.Show(temp);
                logo.UriSource = new Uri(draggItm.TipParkinga.Slika, UriKind.Absolute);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                ((TextBlock)(c).Children[1]).Text = "";
                c.Resources.Add("taken", true);
                CheckValue(c);
            }
            if (!CanvasList.Contains(c))
            {
                CanvasList.Add(c);
            }
        }

        private void CheckValue(Canvas c)
        {
            Dictionary<int, Parking> temp = new Dictionary<int, Parking>();
            foreach (var r in View1Model.Parkinzi)
            {
                temp.Add(r.Id, r);
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;

                    if (CanvasObj.Count != 0)
                    {
                        if (CanvasObj.ContainsKey(c.Name))
                        {
                            if (temp[CanvasObj[c.Name].Id].Val <= 250 || temp[CanvasObj[c.Name].Id].Val >= 350)
                            {
                                ((Border)(c).Children[0]).BorderBrush = Brushes.Red;
                            }
                        }
                    }
                });
                CheckValue(c);
            });
        }
        public void OnDragLeave(Canvas c)
        {//kad se skloni slika sa kanvasa koja se prevlaci
            if (((TextBlock)(c).Children[1]).Text == "!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Background = Brushes.Transparent;
            }
        }
        public void OnDragOver(Canvas c)
        {//prilikom prelaska preko zauzete povrsine
            if (c.Resources["taken"] != null)
            {
                ((TextBlock)(c).Children[1]).Text = "!";
                ((TextBlock)(c).Children[1]).Background = Brushes.Salmon;
            }
        }
        public void OnFree(Canvas c)
        {
            try
            {
                if (c.Resources["taken"] != null)
                {
                    //ako je
                    c.Background = Brushes.White;
                    foreach (Parking r in View1Model.Parkinzi)
                    {
                        if (!Items.Contains(r) && CanvasObj[c.Name].Id == r.Id)
                            Items.Add(new Parking(r));
                    }
                    c.Resources.Remove("taken");
                    CanvasObj.Remove(c.Name);
                }
            }
            catch (Exception) { }

        }
        public void OnDrop(Canvas c)
        {
            if (((TextBlock)(c).Children[1]).Text == "!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Foreground = Brushes.White;
            }
            if (draggItm != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    
                    string temp = draggItm.TipParkinga.Ime + ".png";
                    //MessageBox.Show(draggItm.TipParkinga.Slika);
                    logo.UriSource = new Uri(draggItm.TipParkinga.Slika, UriKind.Absolute);

                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    CanvasObj[c.Name] = draggItm;
                    c.Resources.Add("taken", true);
                    Items.Remove(Items.Single(x => x.Id == draggItm.Id));
                    SelectedIndex = 0;
                    CheckValue(c);
                }
                dragging = false;
            }
        }
        public void OnMLBU(ListView lw)
        {//podizanje klika, sve stavi na null/false ->ponisti sve
            draggItm = null;
            lw.SelectedItem = null;
            dragging = false;
        }
        public void SelectionChange(Parking r)
        {
            if (!dragging)
            {//izvrsi promenu ako nema pomeranja
                dragging = true;
                draggItm = new Parking(r);
                DragDrop.DoDragDrop(lv, draggItm, DragDropEffects.Move);
            }
        }

    }
}
