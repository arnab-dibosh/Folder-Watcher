using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Watcher
{
    public partial class Form1 : Form
    {
        FileSystemWatcher _watchFolder = new FileSystemWatcher();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startActivityMonitoring(@"D:\Shared Folder");
        }

        private void startActivityMonitoring(string sPath)
        {
            _watchFolder.Path = sPath;
        
            _watchFolder.NotifyFilter = System.IO.NotifyFilters.DirectoryName;

            _watchFolder.NotifyFilter =
            _watchFolder.NotifyFilter | System.IO.NotifyFilters.FileName;
            _watchFolder.NotifyFilter =
            _watchFolder.NotifyFilter | System.IO.NotifyFilters.Attributes;

            _watchFolder.Created += new FileSystemEventHandler(eventRaised);
           
            try
            {
                _watchFolder.EnableRaisingEvents = true;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        
        private void eventRaised(object sender, FileSystemEventArgs e)
        {
            List<string> file_list = new List<string>();
            
        
            foreach (string file_name in Directory.GetFiles(@"D:\Shared Folder"))
                file_list.Add(file_name);

            Thread thread = new Thread(() => 
                    {
                        Clipboard.Clear();

                        if (file_list.Count == 1 && file_list.First().Substring(file_list.First().LastIndexOf('\\')+1) == "Board.txt")
                            Clipboard.SetText(File.ReadAllText(file_list.First()));
                        else
                            Clipboard.SetData(DataFormats.FileDrop, file_list.ToArray());
                    }
                );

            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();     
        }


    }
}
