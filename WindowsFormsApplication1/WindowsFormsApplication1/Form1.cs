using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        public Form1()
        { 
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = RandomNumber(8);
        }
        //DES非對稱式加密
        public static string EncryptDES(string original, string key, string iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = Encoding.UTF8.GetBytes(iv);
                byte[] s = Encoding.UTF8.GetBytes(original);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                return BitConverter.ToString(desencrypt.TransformFinalBlock(s, 0, s.Length)).Replace("-", string.Empty);
            }
            catch 
            {
                return original;
            }
        }
        
        //RSA對稱式解密，公私鑰
        public static string EncryptRSA(string original, string xmlString)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlString);
                byte[] s = Encoding.UTF8.GetBytes(original);
                return BitConverter.ToString(rsa.Encrypt(s, false)).Replace("-", string.Empty);
            }
            catch {
                return original;
            }
        }

        public string RandomNumber(int Count)
        {
            string strChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] VcArray = strChar.Split(',');//用，切割strChar 存入VcArray
            string VNum = "";
            int temp = -1;      //記錄上次亂數值，儘量避免產生幾個一樣的亂數

            //用一個簡單的演算法以保證生成亂數與前一個的不同
            Random random = new Random();
            for (int i = 1; i < Count + 1; i++)
            {
                if (temp != -1)
                    random = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                int t = random.Next(62);//random 0~61
                if (temp != -1 && temp == t)
                    return RandomNumber(Count);
                temp = t;
                VNum += VcArray[t];
            }
            return VNum; //返回生成的亂數
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = EncryptDES(textBox1.Text, textBox2.Text, "WinNie31");//使用對稱式加密成密文
            textBox4.Text = EncryptRSA(textBox2.Text, rsa.ToXmlString(false));//rsa.ToXmlString(false)為公鑰xml字串
            WriteTxt();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = RandomNumber(8);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Form2 f = new Form2(rsa.ToXmlString(true));
            //f.Show();
            //this.Hide();
        }
        //寫擋
        public void WriteTxt()
        {
            //建立檔案串流
            StreamWriter sw = new StreamWriter(@"D:\Dia.txt");
            sw.WriteLine(textBox3.Text);//寫入密文
            sw.WriteLine(textBox4.Text);//寫入加密過的key
            sw.Close();//關閉串流
            
            //建立檔案串流
            StreamWriter swp = new StreamWriter(@"D:\Privatekey.txt");
            swp.WriteLine(rsa.ToXmlString(true));//寫入密文
            swp.Close();//關閉串流
        }

    }
}
