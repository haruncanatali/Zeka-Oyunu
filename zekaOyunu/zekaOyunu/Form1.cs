using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zekaOyunu
{
    public partial class Form1 : Form
    {

        private DateTime ilkTarih;//oyun başlama zamanı
        private DateTime suanKi;//oyun bittiğindeki zaman
        Random rnd = new Random();//rastgele atamalar için tanımladım
        //buradaki index dizisi her bir kutucuğu temsil eden 16 tane elamandan oluşuyor ki her biri bir eşe sahip!!
        //böylelikle 8 adet resmi 16 değere dağıtabilieceğiz
        private int[] indexDizisi = new int[] {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        Image[] resimler = new Image[8];//resim dizisi


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {//form başlatıldığındaki işlem kuyruğu
            butonlariOlustur();
            resimleriDiziyeAt(); 
            indexleriKaristir();
            zamaniBaslat();
        }

        private void zamaniBaslat()
        {
            ilkTarih = DateTime.Now;//başlama zamanını alıyorum
        }
        private void resimleriDiziyeAt()
        {

            for (int i = 0; i < 8; i++)
            {
                //form tasarımında ımageList oluşturdum ve içine resimleri attım
                //programda kullanmak için resimleri bir resim arrayine atadım
                resimler[i] = ımageList1.Images[i];
            }
        }
        //manuel olarak buton oluşturmaktansa form başladığında otomatik oluşturmayı tercih ettim
        private void butonlariOlustur()
        {
            for (int i = 0; i < 16; i++)
            {
                Button buton = new Button
                {
                    Name = i.ToString(),//herbirinin adı artan sırada i olacak
                    Text = null,//butonun üstünde yazı gözükmesin
                    Height = 116,//boyut
                    Width = 116,//boyut
                };
                buton.Click += btn_click;//her birinin click eventini tek fonksiyona bağladım
                flowLayoutPanel1.Controls.Add(buton);//oluşturulan butonu flowlayout panele ekliyorum
            }
        }


        List<Button> secilenlerListesi = new List<Button>();//tıklanan butonları bir listeye attım
        //program koşulunda max 2 adet button listeye girebilecek
        private void btn_click(object sender, EventArgs e)
        {
            Button buton = (Button) sender;//gelen sender bir Button sınıfının nesnesidir !!
            //böylelikle Button Sınıfının Özellliklerini Kullanabliriz

            if (buton.Image == null)//eğer butonda resim yok ise
            {
                int butonNumarasi = Convert.ToInt32(buton.Name);//buton numarasını al
                 //butonun resmine atama olarak,resimler dizisinin indexDizisinin Buton Numarasındaki indexine denk gelenn resmi al
                buton.Image = resimler[indexDizisi[butonNumarasi]];

                buton.Refresh();//butonu yenile YAPMASSAK OYUN AKIŞINDAN ÇIKIYOR
                secilenlerListesi.Add(buton);//işlem yapılan butonu listeye ekle
            }
            if(secilenlerListesi.Count==2)//eğer iki adet buton seçilmisse ki max değerdir
            {
                System.Threading.Thread.Sleep(200);//kullanıcı seçtiği iknci butonu kısa süreliğine görmesini sağla
                if (secilenlerListesi[0].Image == secilenlerListesi[1].Image)//eğer listedeki butonların resimleri aynıysa
                {
                    secilenlerListesi[0].Enabled = false;//butonu etkisiz kıl
                    secilenlerListesi[1].Enabled = false;//butonu etkisiz kıl
                }
                else//eğer butonların resimleri aynı değilse
                {
                    secilenlerListesi[0].Image = null;//resmi gizle
                    secilenlerListesi[1].Image = null;//resmi gizle
                }

                secilenlerListesi.Clear();//listeyi de bir sonraki karşılaştırmalar için temizle
            }

            int sayacOyunDurumu = 0;//oyunun bitip bitmedğini kontrol amaçlı
            foreach (Button Butonn in flowLayoutPanel1.Controls)
            {
                if (Butonn.Enabled == false) sayacOyunDurumu++;//işi biten butonların sayısını bul
            }

            if (sayacOyunDurumu == 16)//eğer bütün resimler bulunup tüm butonlar işini bitirdiyse
            {
                suanKi = DateTime.Now;//bitiş zamanını al
                TimeSpan span = suanKi.Subtract(ilkTarih);//farkı bul

                if ((MessageBox.Show("Tebrikler Oyunu " + span.Hours + " Saat " + span.Minutes + " Dakika " +
                                     span.Seconds + " " +  //bilgilendirmeyi yap
                                     "Saniye İçerisinde Bitirdiniz.Yeni Bir Oyuna Başlamak İstiyormusunuz",
                        "Bilgilendirme", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information)) == DialogResult.Yes) //oyuncu yeni oyun istiyor mu?
                {
                    Application.Restart();
                }
                else 
                {
                    Application.Exit(); //istemiyosa oyunu kapat
                }
            }
        }

        private void indexleriKaristir()
        {
            List<int> rastSayilar = new List<int>();
            //eğer resimlerin butonlara karışık bir şekilde dağılmasını istiyorsak bunu yapmalıyız
            for (int i = 0; i < 16; i++)
            {          
                int sayi = rnd.Next(8);
               
                //{0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7}
                int deger = indexDizisi[i];//dizinin i nci indexini bir değerde tut
                indexDizisi[i] = indexDizisi[sayi];//dizinin i nci indexinin değerini dizinin farkılı bir indexinin değeriyle değiştir
                indexDizisi[sayi] = deger;//değer aldığın indexin yeni değerine eski değerini koy
                //burada yaptığım karıştırma işlemiyle oyun her yeni başladığında farklı konumlandırmalar olacaktır!
            }
        } 
        
    }

 
}

