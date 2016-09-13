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
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        string E, key;
        string privatekey;
        public Form2(string pri)
        {
            InitializeComponent();
            privatekey = pri;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // 建立檔案串流（@ 可取消跳脫字元 escape sequence）
            StreamReader sr = new StreamReader(@"D:\Dia.txt");
			// 每次讀取一行，直到檔尾
            E = sr.ReadLine();// 讀取文字
            key = sr.ReadLine();
            sr.Close();	// 關閉串流

            textBox1.Text ="密文 : " + E + Environment.NewLine + "加密過的key : " + key;
        }

        //RSA對稱式解密，公私鑰
        public static string DecryptRSA(string hexString, string xmlString)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlString);
                byte[] s = new byte[hexString.Length / 2];
                int j = 0;
                for (int i = 0; i < hexString.Length / 2; i++)
                {
                    s[i] = Byte.Parse(hexString[j].ToString() + hexString[j + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                    j += 2;
                }
                return Encoding.UTF8.GetString(rsa.Decrypt(s, false));
            }
            catch 
            {
                return hexString;
            }
        }
        //DES非對稱式解密
        public static string DecryptDES(string hexString, string key, string iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                byte[] s = new byte[hexString.Length / 2];
                int j = 0;
                for (int i = 0; i < hexString.Length / 2; i++)
                {
                    s[i] = Byte.Parse(hexString[j].ToString() + hexString[j + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                    j += 2;
                }
                ICryptoTransform desencrypt = des.CreateDecryptor();
                return Encoding.UTF8.GetString(desencrypt.TransformFinalBlock(s, 0, s.Length));
            }
            catch
            {
                return hexString;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Deskey = DecryptRSA(key, privatekey);
            textBox2.Text = DecryptDES(E, Deskey, "WinNie31");
        }

    }
}
