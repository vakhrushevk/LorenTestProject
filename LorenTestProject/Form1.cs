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
        List<item> items = new List<item>();
        public Form1()
        {
            InitializeComponent();
            FromDateBase.dbconnect();
            FromDateBase.dbexport();
          //  var k = FromDateBase.dbimport();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            items.Add(new item { name = "Миасс",Price=1,FinishPrice=2 });

            DataTable table = new DataTable();
            table.Columns.Add("Имя", typeof(string));
            table.Columns.Add("Цена", typeof(double));
            table.Columns.Add("Итоговая цена", typeof(double));

            table.Rows.Add(items[0].name, items[0].Price, items[0].FinishPrice);
            dataGridView1.DataSource = table;
        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
           
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            var value = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            var name = dataGridView1[e.ColumnIndex-1, e.RowIndex].Value;
            calc(Name, Convert.ToDouble(value));

        }

        private double calc(string name, double price)
        {
            var list = FromDateBase.dbimport();
            
            var objSalon = FromDateBase.dbImportId(name);
            return 0;
         
        }
        
    }
    public class item
    {
        public string name { get; set; }
        public double Price { get; set; }
        public double FinishPrice { get; set; }
    }
}
