using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using FirebirdSql.Data.FirebirdClient;

namespace shuttle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            start();
        }

             public void start()
        {
            TcpClient prova = new TcpClient();
            if (prova.Connected != true)
            {
                prova.Connect("192.168.3.227", 9101);
            }
            var ns = prova.GetStream();
            NetworkStream stream = prova.GetStream();
            Byte[] output = new Byte[1024];
            string lettera = string.Empty;
            int bytes = ns.Read(output, 0, output.Length);
            lettera = Encoding.ASCII.GetString(output, 0, bytes);

            if (lettera.Length <= 8)
            {
                try
                {
                    char[] lettere = { '*' };
                    lettera = lettera.Trim(lettere);

                    string primo = lettera.Substring(0, 1);
                    string secondo = lettera.Substring(1, 1);
                    string terzo = lettera.Substring(2, 1);
                    string quarto = lettera.Substring(3, 1);
                    string quinto = lettera.Substring(4, 1);
                    string sesto = lettera.Substring(5, 1);

                    int primoNumero = int.Parse(conversione(primo)) * 33554432;
                    int secondoNumero = int.Parse(conversione(secondo)) * 1048576;
                    int terzoNumero = int.Parse(conversione(terzo)) * 32768;
                    int quartoNumero = int.Parse(conversione(quarto)) * 1024;
                    int quintoNumero = int.Parse(conversione(quinto)) * 32;
                    int sestoNumero = int.Parse(conversione(sesto)) * 1;

                    lettera = (primoNumero + secondoNumero + terzoNumero + quartoNumero + quintoNumero + sestoNumero).ToString();
                    if (lettera.Length == 8)
                    {
                        lettera = "0" + lettera.ToString();
                    }
                    else if (lettera.Length == 7)
                    {
                        lettera = "00" + lettera.ToString();
                    }
                    try
                    {
                        FbConnection connesione = new FbConnection("User=SYSDBA;Password=masterkey;Database=c:/program files (x86)/winfarm/archivi/arc2000.phs;DataSource=server;Port = 3050; Dialect = 3; Charset = NONE; Role =; Connection lifetime = 15; Pooling = true;MinPoolSize = 0; MaxPoolSize = 50; Packet Size = 8192; ServerType = 0");
                        connesione.Open();
                        FbCommand query = new FbCommand("select a.kdes,(select v_euro from vero_prezzo('TODAY',a.km10,2)) from anapro a where a.km10='" + lettera + "'", connesione);
                        FbDataReader lettore = query.ExecuteReader();
                        while (lettore.Read())
                        {

                            string prezzo = lettore[1].ToString();
                            string descrizione = lettore[0].ToString();

                            string ESC = Char.ConvertFromUtf32(27);
                            string pulisci = Char.ConvertFromUtf32(37);
                            string posizione = Char.ConvertFromUtf32(39);
                            string font = Char.ConvertFromUtf32(66);
                            string align = Char.ConvertFromUtf32(46);
                            //string euro = Char.ConvertFromUtf32(80);

                            byte[] dati = Encoding.UTF8.GetBytes(ESC + pulisci + ESC + font + "0 " + ESC + posizione + "0" + "0" + descrizione + ESC + font + "1 " + ESC + posizione + "2" + "1" + " Prezzo " + prezzo);
                            stream.Write(dati, 0, dati.Length);

                        }
                        lettore.Close();
                        stream.Close();
                        ns.Close();
                    }
                    catch (Exception)
                    {

                       
                        stream.Close();
                        ns.Close();
                        this.start();
                    }
                    //this.start();

                }
                catch
                {
                    stream.Close();
                    ns.Close();
                    //this.start();

                }

            }
            if (lettera.Length >= 10)
            {
                char[] lettere = { 'A', 'B', 'F' };
                lettera = lettera.Trim(lettere);

            }
            try
            {
                FbConnection connesione = new FbConnection("User=SYSDBA;Password=masterkey;Database=c:/program files (x86)/winfarm/archivi/arc2000.phs;DataSource=server;Port = 3050; Dialect = 3; Charset = NONE; Role =; Connection lifetime = 15; Pooling = true;MinPoolSize = 0; MaxPoolSize = 50; Packet Size = 8192; ServerType = 0");
                connesione.Open();
                FbCommand query = new FbCommand("select a.kdes,(select v_euro from vero_prezzo('TODAY',a.km10,2)) from anapro a where a.kean='" + lettera.Replace("\r", "").ToString() + "'", connesione);
                FbDataReader lettore = query.ExecuteReader();
                while (lettore.Read())
                {

                    string prezzo = lettore[1].ToString();
                    string descrizione = lettore[0].ToString();
                    string ESC = Char.ConvertFromUtf32(27);
                    string pulisci = Char.ConvertFromUtf32(37);
                    string posizione = Char.ConvertFromUtf32(39);
                    string font = Char.ConvertFromUtf32(66);
                    string align = Char.ConvertFromUtf32(46);
                    //string euro = Char.ConvertFromUtf32(80);

                    byte[] dati = Encoding.UTF8.GetBytes(ESC + pulisci + ESC + font + "0 " + ESC + posizione + "0" + "0" + descrizione + ESC + font + "1 " + ESC + posizione + "2" + "1" + " Prezzo " + prezzo);
                    stream.Write(dati, 0, dati.Length);



                }
                lettore.Close();
                stream.Close();
                ns.Close();
                this.start();

            }
            catch (Exception)
            {
                stream.Close();
                ns.Close();
                this.start();

            }
        }


        

        static string conversione(string lettera)
        {
            if (lettera == "0")
            {
                lettera = "0";
            }
            if (lettera == "1")
            {
                lettera = "1";
            }
            if (lettera == "2")
            {
                lettera = "2";
            }
            if (lettera == "3")
            {
                lettera = "3";
            }
            if (lettera == "4")
            {
                lettera = "4";
            }
            if (lettera == "5")
            {
                lettera = "5";
            }
            if (lettera == "6")
            {
                lettera = "6";
            }
            if (lettera == "7")
            {
                lettera = "7";
            }
            if (lettera == "8")
            {
                lettera = "8";
            }
            if (lettera == "9")
            {
                lettera = "9";
            }
            if (lettera == "B")
            {
                lettera = "10";
            }
            if (lettera == "C")
            {
                lettera = "11";
            }
            if (lettera == "D")
            {
                lettera = "12";
            }
            if (lettera == "F")
            {
                lettera = "13";
            }
            if (lettera == "G")
            {
                lettera = "14";
            }
            if (lettera == "H")
            {
                lettera = "15";
            }
            if (lettera == "J")
            {
                lettera = "16";
            }
            if (lettera == "K")
            {
                lettera = "17";
            }
            if (lettera == "L")
            {
                lettera = "18";
            }
            if (lettera == "M")
            {
                lettera = "19";
            }
            if (lettera == "N")
            {
                lettera = "20";
            }
            if (lettera == "P")
            {
                lettera = "21";
            }
            if (lettera == "Q")
            {
                lettera = "22";
            }
            if (lettera == "R")

            {
                lettera = "23";
            }
            if (lettera == "S")
            {
                lettera = "24";

            }
            if (lettera == "T")
            {
                lettera = "25";

            }
            if (lettera == "U")
            {
                lettera = "26";

            }
            if (lettera == "V")
            {
                lettera = "27";
            }
            if (lettera == "W")
            {
                lettera = "28";

            }
            if (lettera == "X")
            {
                lettera = "29";

            }
            if (lettera == "Y")
            {
                lettera = "30";
            }
            if (lettera == "Z")
            {
                lettera = "31";
            }
            return lettera;
        }
    }
}