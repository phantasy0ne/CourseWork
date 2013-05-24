using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace EDS_File
{

	public partial class MainForm : Form
	{
		OpenFileDialog openfile = new OpenFileDialog();
		SaveFileDialog save_encrypt = new SaveFileDialog();
		byte[] key;
		byte[] iv;
		public MainForm()
		{
			InitializeComponent();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//if (save_encrypt.ShowDialog() == DialogResult.OK)
			//{
				FileStream encrypt_file = File.Create(openfile.FileName+"_encrypt");
				AesCryptoServiceProvider AES_ALGORITHM = new AesCryptoServiceProvider();
				CryptoStream csEncrypt = new CryptoStream(encrypt_file, AES_ALGORITHM.CreateEncryptor(),CryptoStreamMode.Write);
				StreamWriter swEncStream = new StreamWriter(csEncrypt);
				StreamReader srFile = new StreamReader(textBox1.Text);
				string currLine = srFile.ReadLine();
				while(currLine != null)
				{
					swEncStream.WriteLine(currLine);
                    currLine = srFile.ReadLine();
				}
				
				srFile.Close();
				swEncStream.Flush();
				swEncStream.Close();
				
				FileStream keyfile = File.Create(openfile.FileName+".key");
				BinaryWriter binFile = new BinaryWriter(keyfile);
				key = AES_ALGORITHM.Key;
				iv = AES_ALGORITHM.IV;
				binFile.Write(AES_ALGORITHM.Key);
				binFile.Write(AES_ALGORITHM.IV);
                binFile.Flush();
                binFile.Close();
				
			//}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if (openfile.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = openfile.FileName;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if (openfile.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = openfile.FileName;
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if (openfile.ShowDialog() == DialogResult.OK)
			{
			        FileStream fsFileIn = File.OpenRead(textBox2.Text);

                    FileStream fsKeyFile = File.OpenRead(openfile.FileName);

                    FileStream fsFileOut = File.Create(textBox2.Text+"_decrypt");

                    AesCryptoServiceProvider cryptAlgorithm = new AesCryptoServiceProvider();
                    BinaryReader brFile = new BinaryReader(fsKeyFile);
                    //textBox1.Text = key.ToString();
                    StreamWriter writer = File.AppendText(@"key.txt");
                    writer.WriteLine(key.Length);
                    cryptAlgorithm.Key = key;
                    cryptAlgorithm.IV = iv;


                    CryptoStream csEncrypt = new CryptoStream(fsFileIn, cryptAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);


                    StreamReader srStream = new StreamReader(csEncrypt);
                    StreamWriter swStream = new StreamWriter(fsFileOut);
						
                    swStream.WriteLine(srStream.ReadToEnd());
                    
                    swStream.Close();
                    fsFileOut.Close();
                    srStream.Close();

			}
			        

		}
	}
}
