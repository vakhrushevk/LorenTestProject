using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorenTestProject
{
    class Salons
    {
        public string name { get; set; }
        public double discount { get; set; }
        public bool discount_parent { get; set; }
        public string description { get; set; }
        public int parentid { get; set; }

        public Salons() { }

        public Salons(string Name, double Discount, bool Discount_Parent, string Description,int ParentId)
        {
            this.name = Name;
            this.discount = Discount;
            this.discount_parent = Discount_Parent;
            this.description = Description;
            this.parentid = ParentId;

        }



        public static List<Salons> ListSalons()
        {
            List<Salons> salons = new List<Salons>();
            salons.Add(new Salons("Миасс", 0.04, false, "Описание салона 'Миасс'",0));
            salons.Add(new Salons("Курган", 0.11, false, "Описание салона 'Курган'", 0));
            salons.Add(new Salons("Амелия", 0.05, true, "Описание салона 'Амелия'",1));
            salons.Add(new Salons("Тест2", 0, true, "Описание салона 'Тест2'",1));
            salons.Add(new Salons("Тест1", 0.02, true, "Описание салонa 'Тест1'",3));
            return salons;
        }

    }
    class importSalons:Salons
    {
        public int id { get; set; }
        public importSalons(int Id,string Name,double Discount,bool DiscountParent,int ParentId)
        {
            this.id = Id;
            this.name = Name;
            this.discount = Discount;
            this.discount_parent = DiscountParent;
            this.parentid = ParentId;
        }
    }
}
