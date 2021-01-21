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
using static NetworkService.MyICommand;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private ViewModel1 networkDisplayModel = new ViewModel1();
        private ViewModel2 measurementGraphModel = new ViewModel2();
        private ViewModel3 networkEntitiesModel = new ViewModel3();
        private BindableBase currentViewModel;

        private const int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                      // ######### ZAMENITI stvarnim brojem elemenata
                                      //           zavisno od broja entiteta u listi
                                      //private BindingList<Parking> parkinzi;
        static string fname = "logs.txt";

        public MainWindowViewModel()
        {
            //createListener();
            NavCommand = new MyICommand<String>(OnNav);
            CurrentViewModel = networkDisplayModel;
        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "Network Data":
                    CurrentViewModel = networkDisplayModel;
                    break;
                case "Network View":
                    CurrentViewModel = networkDisplayModel;
                    break;
                case "Data Chart":
                    CurrentViewModel = measurementGraphModel;
                    break;

            }
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25591);
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
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            string[] msg = incomming.Split(':');
                            string[] ime = msg[0].Split('_');
                            int id = Int32.Parse(ime[1]);
                            double v = Double.Parse(msg[1]);
                            string ime_real = ime[0];

                            string send_msg = string.Format("{0}_{1}:{2}", ime_real, id, v);

                            using (StreamWriter sw = File.AppendText(fname))
                            {
                                sw.WriteLine(send_msg);
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
