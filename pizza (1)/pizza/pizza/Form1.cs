using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace pizza
{
    public partial class Form1 : Form
    {
        private List<Order> orders = new List<Order>();
        private List<Pizza> pizzaList = new List<Pizza>();

        public Form1()
        {
            InitializeComponent();
            LoadPizzaNames();
            PopulateToppingComboBox();
            groupBox3.Visible = rbKiszallitas.Checked;
            LoadordersfromTxt();
            
                
              
            
        }

        private void LoadordersfromTxt()
        {
            string filePath = "rendelesek.txt";

            try
            {
                string[] orderdata = File.ReadAllLines(filePath);
                foreach (var line in orderdata)
                {
                    string[] parts = line.Split(' ');
                    string pizzaName = string.Join(" ", parts.Take(parts.Length - 4));

                    int price24 = int.Parse(parts[parts.Length - 4]);
                    int price32 = int.Parse(parts[parts.Length - 2]);

                    Pizza pizza = new Pizza(pizzaName, price24, price32);
                    pizzaList.Add(pizza);
                    pizzaCbox.Items.Add(pizzaName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a pizza betöltésekor: " + ex.Message);
            }
        }

        private void LoadPizzaNames()
        {
            string filePath = "pizza.txt";

            try
            {
                string[] pizzaData = File.ReadAllLines(filePath);
                foreach (var line in pizzaData)
                {
                    string[] parts = line.Split(' ');
                    string pizzaName = string.Join(" ", parts.Take(parts.Length - 4));

                    int price24 = int.Parse(parts[parts.Length - 4]);
                    int price32 = int.Parse(parts[parts.Length - 2]);

                    Pizza pizza = new Pizza(pizzaName, price24, price32);
                    pizzaList.Add(pizza);
                    pizzaCbox.Items.Add(pizzaName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a pizza betöltésekor: " + ex.Message);
            }
        }

        private void PopulateToppingComboBox()
        {
            List<Topping> toppingList = new List<Topping>
            {
                new Topping("Csurmi", 200),
                new Topping("Sajt", 300),
                new Topping("Gomba", 500),
                new Topping("Lekvár", 500)
            };

            feltetCbox.Items.AddRange(toppingList.ToArray());
            feltetCbox.DisplayMember = "Name";
        }

        public class Topping
        {
            public string Name { get; set; }
            public int Price { get; set; }

            public Topping(string name, int price)
            {
                Name = name;
                Price = price;
            }

            public override string ToString()
            {
                return $"{Name} - {Price} Ft";
            }
        }

        public class Pizza
        {
            public string Name { get; set; }
            public int Price24 { get; set; }
            public int Price32 { get; set; }

            public Pizza(string name, int price24, int price32)
            {
                Name = name;
                Price24 = price24;
                Price32 = price32;
            }

            public override string ToString()
            {
                return $"{Name} - {Price24} Ft (24cm), {Price32} Ft (32cm)";
            }
        }

        public class Order
        {
            public string Name { get; set; }
            public int Size { get; set; }
            public int Price { get; set; }
            public int Amount { get; set; }
            public Topping Topping { get; set; }

            public Order(string name, int size, int price, int amount, Topping topping)
            {
                Name = name;
                Size = size;
                Price = price;
                Amount = amount;
                Topping = topping;
            }

            public override string ToString()
            {
                string toppingText = Topping != null ? $"{Topping.Name}" : "Nincs feltét";
                return $"{Name};{Size};{toppingText};{Amount};{Price}";
            }
        }

        private void hozzaadBtn_Click(object sender, EventArgs e)
        {
            string pizzaName = pizzaCbox.Text;
            Topping selectedTopping = (Topping)feltetCbox.SelectedItem;
            int size = rb24.Checked ? 24 : 32;

            Pizza selectedPizza = pizzaList.FirstOrDefault(p => p.Name == pizzaName);
            if (selectedPizza == null)
            {
                MessageBox.Show("A kiválasztott pizza nem található a listában!");
                return;
            }

            int basePrice = (size == 24) ? selectedPizza.Price24 : selectedPizza.Price32;
            int finalPrice = (selectedTopping == null) ? basePrice : basePrice + selectedTopping.Price;
            int amount = Convert.ToInt32(darabszam.Value);

            Order newOrder = new Order(pizzaName, size, finalPrice, amount, selectedTopping);
            orders.Add(newOrder);
            rendelesekLbox.Items.Add(newOrder);
            MessageBox.Show("Rendelés hozzáadva!");
        }

        private void torlesBtn_Click(object sender, EventArgs e)
        {
            if (rendelesekLbox.SelectedItem is Order selectedOrder)
            {
                orders.Remove(selectedOrder);
                rendelesekLbox.Items.Remove(selectedOrder);
                MessageBox.Show("A kiválasztott rendelés törölve!");
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva rendelés törlésre!");
            }
        }

        private void rbKiszallitas_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Visible = rbKiszallitas.Checked;
        }

        private void rendelesBtn_Click(object sender, EventArgs e)
        {
            if (orders.Count == 0)
            {
                MessageBox.Show("Nincs rendelés leadva!");
                return;
            }

            string customerName = nevTbox.Text.Trim();
            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Adja meg a nevét!");
                return;
            }

            bool kiszallitas = rbKiszallitas.Checked;
            string address = textBcimTboxox1.Text.Trim();
            string date = dateTimePicker1.Value.ToShortDateString();
            string time = idoTbox.Text.Trim();

            int szallitasiDij = kiszallitas && !address.ToLower().Contains("baja") ? 500 : 0;

            if (kiszallitas && string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Adja meg a kiszállítási címet!");
                return;
            }

            int totalPrice = orders.Sum(o => o.Price * o.Amount) + szallitasiDij;
            string orderSummary = $"{customerName};";

            string orderText = $"Név: {customerName} ";

            foreach (var order in orders)
            {
                orderSummary += order.ToString() + ";" ;
                orderText += $"Pizza neve: {order.Name}, pizza  mérete: {order.Size}, extra feltét: {order.Topping}, mennyiség: {order.Amount}, egységár: {order.Price}";
            }

            if (kiszallitas)
            {
                orderSummary += $"{address};{date};{time};{szallitasiDij};";

           
            }

            orderSummary += $"{totalPrice}";

            DialogResult result = MessageBox.Show(orderSummary + "\n\nElfogadja a rendelést?", "Rendelés véglegesítése", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter("rendelesek.txt", true))
                    {
                   
                        writer.WriteLine(orderSummary);
               
                    }

                    MessageBox.Show("A rendelés sikeresen rögzítve!");

                  

                   
                    rendelesekLbox.Items.Clear();
                    orders.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba történt a rendelés mentésekor: " + ex.Message);
                }
            }
        }

        private void adminBtn_Click(object sender, EventArgs e)
        {

          
        }

        private void listBoxAdminOrders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
