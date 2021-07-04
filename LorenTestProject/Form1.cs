using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LorenTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FromDateBase.dbconnect();
            FromDateBase.dbexport();
            var k = FromDateBase.dbimport();

        }
    }
}
