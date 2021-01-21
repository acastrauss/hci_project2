using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using NetworkService.Model;
using GalaSoft.MvvmLight.Messaging;

namespace NetworkService.ViewModel
{
    public class View3Model : BindableBase, IDataErrorInfo
    {
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();


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
                    OnPropertyChanged("Y4");
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
            Y1 = 150;
            Y2 = 150;
            Y3 = 150;
            Y4 = 150;
            Y5 = 150;
            Messenger.Default.Register<Dictionary<string, Parking>>(this, Crtaj);
        }

        public void Crtaj(Dictionary<string, Parking> auti)
        {
            int i = 0;
            foreach (KeyValuePair<string, Parking> entry in auti)
            {
                if (i == 0)
                {
                    //entry.Value.Value = 15000.0;
                    Y5 = (int)entry.Value.Val / 100;
                    Time5 = entry.Key;
                    Type5 = entry.Value.TipParkinga.Ime;

                }
                else if (i == 1)
                {
                    // entry.Value.Value = 7000.0;
                    Y4 = (int)entry.Value.Val / 100;
                    Time4 = entry.Key;
                    Type4 = entry.Value.TipParkinga.Ime;
                }
                else if (i == 2)
                {
                    // entry.Value.Value = 15000.0;
                    Y3 = (int)entry.Value.Val / 100;
                    Time3 = entry.Key;
                    Type3 = entry.Value.TipParkinga.Ime;
                }
                else if (i == 3)
                {
                    // entry.Value.Value = 7000.0;
                    Y2 = (int)entry.Value.Val / 100;
                    Time2 = entry.Key;
                    Type2 = entry.Value.TipParkinga.Ime;
                }
                else if (i == 4)
                {
                    //entry.Value.Value = 15000.0;
                    Y1 = (int)entry.Value.Val / 100;
                    Time1 = entry.Key;
                    Type1 = entry.Value.TipParkinga.Ime;
                }
                i++;
            }

            i = 0;
            foreach (KeyValuePair<string, Parking> entry in auti)
            {
                if (entry.Value.TipParkinga.Ime == "IA")
                {
                    if (entry.Value.Val > 15000.0)
                    {
                        switch (i)
                        {
                            case 0:
                                Color5 = "#ff0000";
                                break;
                            case 1:
                                Color4 = "#ff0000";
                                break;
                            case 2:
                                Color3 = "#ff0000";
                                break;
                            case 3:
                                Color2 = "#ff0000";
                                break;
                            case 4:
                                Color1 = "#ff0000";
                                break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                Color5 = "#000000";
                                break;
                            case 1:
                                Color4 = "#000000";
                                break;
                            case 2:
                                Color3 = "#000000";
                                break;
                            case 3:
                                Color2 = "#000000";
                                break;
                            case 4:
                                Color1 = "#000000";
                                break;
                        }
                    }
                }
                else if (entry.Value.TipParkinga.Ime == "IB")
                {
                    if (entry.Value.Val > 7000.0)
                    {
                        switch (i)
                        {
                            case 0:
                                Color5 = "#ff0000";
                                break;
                            case 1:
                                Color4 = "#ff0000";
                                break;
                            case 2:
                                Color3 = "#ff0000";
                                break;
                            case 3:
                                Color2 = "#ff0000";
                                break;
                            case 4:
                                Color1 = "#ff0000";
                                break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                Color5 = "#000000";
                                break;
                            case 1:
                                Color4 = "#000000";
                                break;
                            case 2:
                                Color3 = "#000000";
                                break;
                            case 3:
                                Color2 = "#000000";
                                break;
                            case 4:
                                Color1 = "#000000";
                                break;
                        }
                    }
                }
                i++;
            }
        }

    }
}
