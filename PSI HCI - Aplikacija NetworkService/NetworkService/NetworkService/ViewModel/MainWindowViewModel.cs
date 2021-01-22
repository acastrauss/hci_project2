using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static NetworkService.MyICommand;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand UndoCommand { get; private set; }
        
        private View1Model podaci = new View1Model();
        private View2Model pregled = new View2Model();
        private View3Model grafik = new View3Model();
        
        private BindableBase currentViewModel;
        private BindableBase previous;

        private const int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                      // ######### ZAMENITI stvarnim brojem elemenata
                                      //           zavisno od broja entiteta u listi
                                      //private BindingList<Parking> parkinzi;
        static string fname = "logs.txt";

        public MainWindowViewModel()
        {
            createListener();
            NavCommand = new MyICommand<String>(OnNav);
            UndoCommand = new MyICommand(OnUndo);
            CurrentViewModel = podaci;
            Previous = CurrentViewModel;

            System.IO.File.WriteAllText(fname, string.Empty);
        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        public BindableBase Previous 
        {
            get { return previous; }
            set
            {
                SetProperty(ref previous, value);
            } 
        }

        private void OnNav(string destination)
        {
            Previous = currentViewModel;

            switch (destination)
            {
                case "Podaci Parkinga":
                    CurrentViewModel = podaci;                    
                    break;
                case "Pregled Parkinga":
                    CurrentViewModel = pregled;
                    break;
                case "Grafik":
                    CurrentViewModel = grafik;
                    break;
                
            }
        }

        private void OnUndo()
        {
            CurrentViewModel = Previous;    
        }
        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(View1Model.Parkinzi.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            /*
                            string[] parts = incomming.Split('_', ':'); //splitujemo string
                            Data.PowerConsumptions[int.Parse(parts[1])].Value = Math.Round(double.Parse(parts[2]), 2); // dodamo vrednost u bazu
                            File.AppendAllText("Log.txt", $"PowerId- {int.Parse(parts[1])}\t|PowerConsumption- {Math.Round(double.Parse(parts[2]), 2)}\t|Time- {DateTime.Now}" + Environment.NewLine); //U Log.txt
                            Tab3ViewModel.ValueChanged.Execute($"{int.Parse(parts[1])}_{Math.Round(double.Parse(parts[2]), 2)}");
                            */

                            string[] msg = incomming.Split(':');
                            string[] ime = msg[0].Split('_');
                            int id = Int32.Parse(ime[1]);
                            double v = Double.Parse(msg[1]);
                            string ime_real = ime[0];

                            string send_msg = string.Format("{0}_{1}:{2}", ime_real, id, v);

                            if (v >= 0 && v < 90)
                            {
                                using (StreamWriter sw = File.AppendText(fname))
                                {
                                    sw.WriteLine(send_msg);
                                }

                                podaci.change_value_by_id(id, v);
                                OnPropertyChanged("Parkizni");
                            }
                        }
                        
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}
