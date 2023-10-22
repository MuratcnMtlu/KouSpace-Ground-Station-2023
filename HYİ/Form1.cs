using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Timer = System.Windows.Forms.Timer;

namespace HYİ
{
    public partial class Form1 : Form
    {
        string[] ports = SerialPort.GetPortNames();
        string[] bauds = { "9600", "19200", "38400", "115200" };



        String data;
        Random rnd = new Random();

        double locLat = 41;
        double locLng = 29;
        double ltn;
        double lng;
        double ltn_old;
        double lng_old;
        double count2 = 0;

        bool ReadData;

        int syc = 0;
        byte say = 0;

        double dEnlem = 38.39839d;
        double dBoylam = 33.71109d;

        byte[] veri = new byte[78];
        byte[] toDouble = new byte[8];

        int j = 0;

        Thread T1;

        byte durum;
        byte angleY;
        byte angleZ;
        byte pil;
        byte accel;
        int alt;
        int vel;
        byte cs;
        double mesafe;
        GMapMarker marker3;
        bool connected = false;

        List<string> splittedPacket = new List<string>();
        List<SavingData> savingData = new List<SavingData>();
        bool startDataCollectionStatus = false;

        byte packetCounter = 0;

        Timer uiTimer, hyiTimer;



        public Form1()
        {
            InitializeComponent();

            foreach (string port in ports)
            {
                port1Box.Items.Add(port);
                port2Box.Items.Add(port);
            }

            foreach (string baud in bauds)
            {
                baud1Box.Items.Add(baud);
                baud2Box.Items.Add(baud);
            }

        }

        public ChartValues<ObservableValue> Values { get; set; }

        GMapOverlay markers = new GMapOverlay("markers");
        private void Form1_Load(object sender, EventArgs e)
        {
            savingData.Add(new SavingData());
            map.DragButton = MouseButtons.Left;
            map.MinZoom = 4;
            map.MaxZoom = 19;
            map.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.Position = new GMap.NET.PointLatLng(40.762234, 29.929693);
            map.ShowCenter = false;
            map.ShowTileGridLines = false;
            map.Zoom = 4;
            PointLatLng point = new PointLatLng(dEnlem, dBoylam);
            GMapMarker marker1 = new GMarkerGoogle(point, GMarkerGoogleType.blue_dot);

            markers.Markers.Add(marker1);
            map.Overlays.Add(markers);

            sg_pil.Uses360Mode = false;
            sg_pil.From = 0;
            sg_pil.To = 100;
            sg_pil.Value = 0;
            sg_pil.FromColor = Colors.Red;
            sg_pil.ToColor = Colors.DarkRed;

            sg_pil.Base.Background = System.Windows.Media.Brushes.Transparent;

            Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(0),
                new ObservableValue(0)
            };

            irtifaGrafik.LegendLocation = LegendLocation.Right;
            irtifaGrafik.Zoom = ZoomingOptions.X;
            irtifaGrafik.Series.Add(new LineSeries
            {
                Title = "İrtifa",
                Values = Values,
                StrokeThickness = 3,
                Stroke = System.Windows.Media.Brushes.DarkRed,
                Fill = System.Windows.Media.Brushes.AntiqueWhite,
                PointGeometrySize = 0,
                DataLabels = false
            });

            uiTimer = new Timer();
            uiTimer.Interval = 500;
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            hyiTimer = new Timer();
            hyiTimer.Interval = 500;
            hyiTimer.Tick += HyiTimer_Tick;
            hyiTimer.Start();
        }

        private void HyiTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen && splittedPacket.Count > 0)
            {
                byte[] olusturalacak_paket = createHYIPacket(splittedPacket);
                serialPort2.Write(olusturalacak_paket, 0, olusturalacak_paket.Length);

            }
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!connected)
            {
                return;
            }

            txt_durum.Text = Convert.ToString(durum);
            txt_y_aci.Text = Convert.ToString(angleY);
            txt_aci_z.Text = Convert.ToString(angleZ);
            txt_accel.Text = Convert.ToString(accel);
            txt_alt.Text = Convert.ToString(alt);
            txt_vel.Text = Convert.ToString(vel);
            txt_lat.Text = "Enlem: " + Convert.ToString(locLat);
            txt_lng.Text = "Boylam: " + Convert.ToString(locLng);
            txt_mesafe.Text = " M:" + mesafe.ToString() + " m";
            MonitorAdd(data.ToString());


            Values.Add(new ObservableValue(Convert.ToDouble(alt)));


            if (Values.Count > 20)
            {
                Values.RemoveAt(0);
            }
            map.Position = new PointLatLng(locLat, locLng);

            PointLatLng point3 = new PointLatLng(locLat, locLng);

            if (marker3 != null)
            {
                markers.Markers.Remove(marker3);
            }

            marker3 = new GMarkerGoogle(point3, GMarkerGoogleType.red_dot);

            markers.Markers.Add(marker3);
            map.Overlays.Add(markers);


            sg_pil.Value = (pil);
            //cp_vel.Value = (vel);
            //    System.Threading.Thread.Sleep(10);
            cp_pitch.Value = (angleY);
            //   System.Threading.Thread.Sleep(10);
            cp_yaw.Value = (angleZ);
            cp_yaw.Update();
            //   System.Threading.Thread.Sleep(10);
            cp_ivme.Value = (accel);
            cp_yaw.Update();
            //   System.Threading.Thread.Sleep(10);
        }

        public void MonitorAdd(string text)
        {
            monitor.SelectionStart = monitor.TextLength;
            monitor.Text = text;
            //monitor.ScrollToCaret();
        }



        private async void timer1_Tick(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {



                #region Veri Paketi


                //
                // check sum eklenecek

                try
                {
                    data = serialPort1.ReadLine();

                    if (data.Length == 35)
                    {
                        List<string> splittedPacket = splitPacket(data);
                        paket_ayristir(splittedPacket);
                        this.splittedPacket = splittedPacket;
                        if (startDataCollectionStatus)
                        {
                            savingData.Add(new SavingData(durum, angleY, angleZ, accel, alt, vel, ltn, lng, mesafe));
                        }
                        

                        if (computeCS(splittedPacket))
                        {
                            paket_ayristir(splittedPacket);
                            this.splittedPacket = splittedPacket;

                        }


                    }


                }
                catch (Exception ex)
                {

                    // MessageBox.Show("Paket Hatası", "UYARI");
                }



                //if (true)
                //{
                //    konum_bul(data);
                //    GPS();
                //}
                #endregion
            });

        }

        private byte[] createHYIPacket(List<string> splittedPacket)
        {
            byte[] olusturalacak_paket = new byte[78];

            byte[] latByteArray = BitConverter.GetBytes((float)ltn);
            byte[] lngByteArray = BitConverter.GetBytes((float)lng);

            double gyroY = angleY * Math.PI / 180.0d;
            double gyroZ = angleZ * Math.PI / 180.0d;

            byte[] gyroYByteArray = BitConverter.GetBytes((float)gyroY);
            byte[] gyroZByteArray = BitConverter.GetBytes((float)gyroZ);

            byte[] altByteArray = BitConverter.GetBytes((float)alt);

            byte[] accelY = BitConverter.GetBytes((float)ConvertPacket(splittedPacket[5]));

            //Sabit
            olusturalacak_paket[0] = 0xFF;
            olusturalacak_paket[1] = 0xFF;
            olusturalacak_paket[2] = 0x54;
            olusturalacak_paket[3] = 0x52;

            //Takim ID
            olusturalacak_paket[4] = 0x10;

            //Sayac
            olusturalacak_paket[5] = packetCounter++;

            //Barometre irtifa
            olusturalacak_paket[6] = altByteArray[0];
            olusturalacak_paket[7] = altByteArray[1];
            olusturalacak_paket[8] = altByteArray[2];
            olusturalacak_paket[9] = altByteArray[3];

            //GPS irtifa
            olusturalacak_paket[10] = 0x00;
            olusturalacak_paket[11] = 0x00;
            olusturalacak_paket[12] = 0x00;
            olusturalacak_paket[13] = 0x00;

            //Enlem
            olusturalacak_paket[14] = latByteArray[0];
            olusturalacak_paket[15] = latByteArray[1];
            olusturalacak_paket[16] = latByteArray[2];
            olusturalacak_paket[17] = latByteArray[3];

            //Boylam
            olusturalacak_paket[18] = lngByteArray[0];
            olusturalacak_paket[19] = lngByteArray[1];
            olusturalacak_paket[20] = lngByteArray[2];
            olusturalacak_paket[21] = lngByteArray[3];

            //Gorev yuku irtifa
            olusturalacak_paket[22] = 0x00;
            olusturalacak_paket[23] = 0x00;
            olusturalacak_paket[24] = 0x00;
            olusturalacak_paket[25] = 0x00;

            //Gorev yuku enlem
            olusturalacak_paket[26] = 0x00;
            olusturalacak_paket[27] = 0x00;
            olusturalacak_paket[28] = 0x00;
            olusturalacak_paket[29] = 0x00;

            //Gorev yuku boylam
            olusturalacak_paket[30] = 0x00;
            olusturalacak_paket[31] = 0x00;
            olusturalacak_paket[32] = 0x00;
            olusturalacak_paket[33] = 0x00;

            //Kademe gps irtifa
            olusturalacak_paket[34] = 0x00;
            olusturalacak_paket[35] = 0x00;
            olusturalacak_paket[36] = 0x00;
            olusturalacak_paket[37] = 0x00;

            //Kademe enlem
            olusturalacak_paket[38] = 0x00;
            olusturalacak_paket[39] = 0x00;
            olusturalacak_paket[40] = 0x00;
            olusturalacak_paket[41] = 0x00;

            //Kademe boylam
            olusturalacak_paket[42] = 0x00;
            olusturalacak_paket[43] = 0x00;
            olusturalacak_paket[44] = 0x00;
            olusturalacak_paket[45] = 0x00;

            //Jiroskop X
            olusturalacak_paket[46] = 0x00;
            olusturalacak_paket[47] = 0x00;
            olusturalacak_paket[48] = 0x00;
            olusturalacak_paket[49] = 0x00;

            //Jiroskop Y
            olusturalacak_paket[50] = gyroYByteArray[0];
            olusturalacak_paket[51] = gyroYByteArray[1];
            olusturalacak_paket[52] = gyroYByteArray[2];
            olusturalacak_paket[53] = gyroYByteArray[3];

            //Jiroskop Z
            olusturalacak_paket[54] = gyroZByteArray[0];
            olusturalacak_paket[55] = gyroZByteArray[1];
            olusturalacak_paket[56] = gyroZByteArray[2];
            olusturalacak_paket[57] = gyroZByteArray[3];

            //Ivme X
            olusturalacak_paket[58] = 0x00;
            olusturalacak_paket[59] = 0x00;
            olusturalacak_paket[60] = 0x00;
            olusturalacak_paket[61] = 0x00;

            //Ivme Y
            olusturalacak_paket[62] = accelY[0];
            olusturalacak_paket[63] = accelY[1];
            olusturalacak_paket[64] = accelY[2];
            olusturalacak_paket[65] = accelY[3];

            //Ivme Z
            olusturalacak_paket[66] = 0x00;
            olusturalacak_paket[67] = 0x00;
            olusturalacak_paket[68] = 0x00;
            olusturalacak_paket[69] = 0x00;

            //Aci
            olusturalacak_paket[70] = 0x00;
            olusturalacak_paket[71] = 0x00;
            olusturalacak_paket[72] = 0x00;
            olusturalacak_paket[73] = 0x00;

            //Durum
            olusturalacak_paket[74] = durum == 0 ? (byte)1 : durum == 1 ? (byte)2 : (byte)4;

            //Checksum
            olusturalacak_paket[75] = check_sum_hesapla(olusturalacak_paket);

            //Sabit
            olusturalacak_paket[76] = 0x0D;
            olusturalacak_paket[77] = 0x0A;

            return olusturalacak_paket;
        }



        private bool computeCS(List<string> splittedPacket)
        {
            long sum = 0;
            for (int i = 0; i < splittedPacket.Count - 1; i++)
            {
                sum += Convert.ToInt32(splittedPacket[i], 16);
            }

            byte csCalculated = BitConverter.GetBytes(1 + ((byte)255) ^ sum)[0];
            byte csReceived = ConvertPacket(splittedPacket.Last());

            return csCalculated == csReceived;
        }


        private void konum_bul(string veri)
        {
            if (data.IndexOf("rl") != -1)
            {
                string[] loc = { " ", " " };


                loc = data.Substring(2).Split(',');

                txt_lat.Text = loc[0];
                txt_lng.Text = loc[1];

                ltn = Convert.ToDouble(loc[0].Replace(".", ","));
                lng = Convert.ToDouble(loc[1].Replace(".", ","));
                map.Position = new GMap.NET.PointLatLng(ltn, lng);
            }
        }

        private List<string> splitPacket(string data)
        {
            List<string> splittedData = new List<string>();

            for (int i = 0; i < (int)(data.Length * 0.5d); i++)
            {
                int index = 2 * i;
                splittedData.Add(data.Substring(index, 2));
            }

            return splittedData;
        }

        private void paket_ayristir(List<string> splittedPacket)
        {
            if (!connected)
            {
                connected = true;
            }

            durum = ConvertPacket(splittedPacket[1]);

            switch (durum)
            {
                case (byte)1:
                    kurtarma1State.BackColor = System.Drawing.Color.Green;
                    break;
                case (byte)2:
                    kurtarma1State.BackColor = System.Drawing.Color.Orange;
                    kurtarma2State.BackColor = System.Drawing.Color.Orange;
                    break;
            }


            //Y AÇISI
            angleY = ConvertPacket(splittedPacket[2]);

            //Z AÇISI
            angleZ = ConvertPacket(splittedPacket[3]);


            //PİL
            pil = ConvertPacket(splittedPacket[4]);
            //txt_pil.Text = Convert.ToString(pil);
            if (pil > 100)
            {
                pil = 100;
            }

            //İVME
            accel = ConvertPacket(splittedPacket[5]);


            //YÜKSEKLİK
            alt = ConvertPacket(splittedPacket[6], splittedPacket[7]);

            //HIZ
            vel = ConvertPacket(splittedPacket[8], splittedPacket[9]);

            //ENLEM
            locLat = ConvertPacket(splittedPacket[10], splittedPacket[11], splittedPacket[12]);
            //txt_lat.Text = Convert.ToString(locLat);

            //BOYLAM
            locLng = ConvertPacket(splittedPacket[13], splittedPacket[14], splittedPacket[15]);
            //txt_lng.Text = Convert.ToString(locLng);

            //CS
            byte cs = ConvertPacket(splittedPacket[16]);

            mesafe = range(dEnlem, locLat, dBoylam, locLng);
            mesafe = Math.Round(mesafe, 2);
        }

        private async void Wait()
        {
            await Task.Delay(20);
        }

        private byte ConvertPacket(string val1)
        {
            return Convert.ToByte(val1, 16);
        }

        private int ConvertPacket(string val1, string val2)
        {
            string value = val1 + val2;

            int intValue = Convert.ToInt32(value, 16);

            return intValue > 60000 ? intValue - 65536 : intValue;
        }

        private double ConvertPacket(string val1, string val2, string val3)
        {
            byte integralPortion = Convert.ToByte(val1, 16);
            int decimalPortion = Convert.ToInt32(val2 + val3, 16);

            return integralPortion + decimalPortion * 1e-4d;
        }

        private async void Deneme()
        {
            await Task.Delay(10);
        }
        public double range(double lat1, double lat2, double lon1, double lon2)
        {
            double R = 6371000;
            double fi_1 = lat1 * Math.PI / 180;
            double fi_2 = lat2 * Math.PI / 180;
            double delta_fi = (lat2 - lat1) * Math.PI / 180;
            double delta_lamda = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(delta_fi / 2) * Math.Sin(delta_fi / 2) + Math.Cos(fi_1) * Math.Cos(fi_2) * Math.Sin(delta_lamda / 2) * Math.Sin(delta_lamda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;

            return d;
        }

        private void Start_Stop_Click_1(object sender, EventArgs e)
        {


            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.PortName = port1Box.Text;
                    serialPort1.BaudRate = Convert.ToInt32(baud1Box.Text);
                    serialPort1.Open();



                    timer1.Start();


                    Start_SerialCom.Text = "DURDUR";
                }
                catch (Exception)
                {
                    MessageBox.Show("SERİ PORT BAŞLATILAMADI", "UYARI");
                }

            }
            else if (serialPort1.IsOpen)
            {
                Start_SerialCom.Text = "COM BAŞLAT";
                serialPort1.Close();
                timer1.Stop();
            }
            else
            {
                serialPort1.Close();
                timer1.Stop();

            }



        }

        private void hyi_start_Click(object sender, EventArgs e)
        {
            if (!serialPort2.IsOpen)
            {
                try
                {
                    serialPort2.PortName = port2Box.Text;
                    serialPort2.BaudRate = Convert.ToInt32(baud2Box.Text);
                    serialPort2.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("HYİ BAŞLATILAMADI", "UYARI");
                }
            }
            else
            {

                serialPort2.Close();

            }
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void btn_konumbul_Click(object sender, EventArgs e)
        {
            map.MapProvider = GMapProviders.GoogleSatelliteMap;
            double lt = Convert.ToDouble(txt_Enlem.Text);
            double ln = Convert.ToDouble(txt_Boylam.Text);
            map.Position = new PointLatLng(lt, ln);
            map.MinZoom = 5;
            map.MaxZoom = 500;
            map.Zoom = 10;
            PointLatLng point = new PointLatLng(lt, ln);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_dot);

            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
        }

        private void btn_mapZoom_Click(object sender, EventArgs e)
        {
            map.Zoom++;
        }

        private void btn_mapZoomOut_Click(object sender, EventArgs e)
        {
            map.Zoom--;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {


        }


        public void GPS()
        {
            map.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.DragButton = MouseButtons.Left;
            map.MinZoom = 4;
            map.MaxZoom = 20;
            map.Zoom = 15;
            map.Position = new PointLatLng(ltn, lng);
            PointLatLng point = new PointLatLng(locLat, locLng);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);





        }





        private byte sayac_hesapla()
        {
            say += 1;
            if (say == 255)
            {
                say = 0;
            }
            return say;

        }


        byte check_sum_hesapla()
        {
            int check_sum = 0;

            for (int i = 4; i < 75; i++)
            {
                check_sum += veri[i];
            }

            return (byte)(check_sum % 256);
        }

        byte check_sum_hesapla(byte[] olusturalacak_paket)
        {
            int check_sum = 0;

            for (int i = 4; i < 75; i++)
            {
                check_sum += olusturalacak_paket[i];
            }

            return (byte)(check_sum % 256);
        }

        private void guna2TextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel9_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            map.Overlays.Clear();
        }

        private void txt_aci_z_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cp_pitch_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();

            foreach (SavingData savedDate in savingData)
            {
                data.Add(savedDate.ToString());
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pusula_veriler.txt");

            File.WriteAllLines(path, data.ToArray());
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            startDataCollectionStatus = true;
        }

        private class SavingData
        {
            bool nullValue = false;
            byte durum;
            byte angleY;
            byte angleZ;
            byte accel;
            int alt;
            int vel;
            double mesafe;
            DateTime dateTime;
            string time;
            double lat;
            double lng;

            public SavingData()
            {
                nullValue = true;
            }
            public SavingData(byte durum, byte angleY, byte angleZ, byte accel, int alt, int vel, double lat, double lng, double mesafe)
            {
                this.durum = durum;
                this.angleY = angleY;
                this.angleZ = angleZ;
                this.accel = accel;
                this.alt = alt;
                this.vel = vel;
                this.mesafe = mesafe;
                this.lat = lat;
                this.lng = lng;
                dateTime = DateTime.Now;
                time = AddZero(dateTime.Hour.ToString()) + ":" + AddZero(dateTime.Minute.ToString()) + ":"
                    + AddZero(dateTime.Minute.ToString()) + ":" + AddZero(dateTime.Second.ToString()) + ":" + AddZero(dateTime.Millisecond.ToString());
            }

            public byte Durum { get => durum; set => durum = value; }
            public byte AngleY { get => angleY; set => angleY = value; }
            public byte AngleZ { get => angleZ; set => angleZ = value; }
            public byte Accel { get => accel; set => accel = value; }
            public int Alt { get => alt; set => alt = value; }
            public int Vel { get => vel; set => vel = value; }
            public double Mesafe { get => mesafe; set => mesafe = value; }
            public double Lat { get => lat; set => lat = value; }
            public double Lng { get => lng; set => lng = value; }

            public override string ToString()
            {
                if (nullValue)
                {
                    return "Time\t\t\t|\tDurum\t|\tAngle-Y\t|\tAngle-Z\t|\tAccel\t|\tAlt\t|\tVel\t|\tLatitude\t|\tLongitude\t|\tMesafe";
                }
                return time + "\t\t|\t" + Durum.ToString() + "\t|\t" + AngleY.ToString() + "\t|\t" + AngleZ.ToString() + "\t|\t" +
                    Accel.ToString() + "\t|\t" + Alt.ToString() + "\t|\t" + Vel.ToString() + "\t|\t" + Lat.ToString("0.0000") + "\t\t|\t" + Lng.ToString("0.0000") + "\t\t|\t" + Mesafe.ToString("0.000");
            }

            private string AddZero(string value)
            {
                return value.Length < 2 ? "0" + value : value;
            }
        }
    }





}




