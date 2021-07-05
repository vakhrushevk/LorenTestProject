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
            var k = FromDateBase.dbimport();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            
            //Создание таблицы dataGridView, и заполнение её
            table.Columns.Add("Имя", typeof(string));
            table.Columns.Add("Цена", typeof(double));
            table.Columns.Add("Итоговая цена", typeof(double));
            foreach (Salons s in Salons.ListSalons())
            {
                items.Add(new item { name = s.name, Price = 0, FinishPrice = 0 });
            }
            foreach(item i in items)
            {
                table.Rows.Add(i.name, i.Price, i.FinishPrice);
            }
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
        }
        // Заполнение таблицы WF
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1) { 
                var value = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                var name = dataGridView1[e.ColumnIndex-1, e.RowIndex].Value;
                discount = 0;
                dataGridView1[e.ColumnIndex+1, e.RowIndex].Value = Calc(name.ToString(), Convert.ToDouble(value));
                
            }

        }
        double discount = 0;
        // Рекурсивный метод
        private double Calc(string name, double price)
        {
            var list = FromDateBase.dbImportStruct(name);  
            discount = discount + list.discount;
            if (list.discount_parent == false)
            {
                    return (price - (price * discount));
            }
            else
            {
                return Calc(FromDateBase.dbImportId(list.parentid), price);
            }
        }
        
    }
    public class item
    {
        public string name { get; set; }
        public double Price { get; set; }
        public double FinishPrice { get; set; }
    }
}
