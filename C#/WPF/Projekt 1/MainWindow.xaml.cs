using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Projekt_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;
        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadProducts();
        }

        private void LoadProducts()
        {
            List<Product> products = dbHelper.GetProducts();
            productsList.ItemsSource = products;

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Product product = new Product();
                product.Name = nameTextBox.Text;
                if (!string.IsNullOrEmpty(priceTextBox.Text))
                {
                    product.Price = decimal.Parse(priceTextBox.Text);

                }
                else product.Price = 0;
                product.Image = File.ReadAllBytes(openFileDialog.FileName);

                dbHelper.AddProduct(product);
                LoadProducts();
            }

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {

            Product selectedProduct = (Product)productsList.SelectedItem;
            if (selectedProduct != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DatabaseHelper dbHelper = new DatabaseHelper();
                    dbHelper.DeleteProduct(selectedProduct);
                    productsList.ItemsSource = dbHelper.GetProducts();
                    dbHelper.CloseConnection();
                    LoadProducts();
                }
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

            Product selectedProduct = (Product)productsList.SelectedItem;
            Product Newproduct = new Product();

            if (!string.IsNullOrEmpty(priceTextBox.Text))
            {
                Newproduct.Price = decimal.Parse(priceTextBox.Text);

            }
            else Newproduct.Price = 0;

            if (Newproduct.Price == 0)
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                {
                    Newproduct.Name = nameTextBox.Text;

                }
                else Newproduct.Name = selectedProduct.Name;
                Newproduct.Price = selectedProduct.Price;
            }
            else
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                {
                    Newproduct.Name = nameTextBox.Text;

                }
                else Newproduct.Name = selectedProduct.Name;
            }

       
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.EditProduct(selectedProduct, Newproduct);
            dbHelper.CloseConnection();
            LoadProducts();

        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta!");
                return false;
            }
            else if (!decimal.TryParse(priceTextBox.Text, out decimal price) && !string.IsNullOrEmpty(priceTextBox.Text))
            {
                MessageBox.Show("Wprowadzona cena jest nieprawidłowa!");
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
    }

    public class DatabaseHelper
    {
        private SQLiteConnection connection;

        public DatabaseHelper()
        {
            connection = new SQLiteConnection("Data Source=menu.db;Version=3;");
            connection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Products (Name TEXT UNIQUE, Price DECIMAL, Image BLOB);";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();
        }

        public void AddProduct(Product product)
        {
            string selectQuery = "SELECT COUNT(*) FROM Products WHERE Name = @Name;";
            SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@Name", product.Name);
            int count = Convert.ToInt32(selectCommand.ExecuteScalar());
            if (count > 0)
            {
                MessageBox.Show("Product with this name already exists!");
                return;
            }

            string insertQuery = "INSERT INTO Products (Name, Price, Image) VALUES (@Name, @Price, @Image);";
            SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@Name", product.Name);
            insertCommand.Parameters.AddWithValue("@Price", product.Price);
            insertCommand.Parameters.AddWithValue("@Image", product.Image);
            insertCommand.ExecuteNonQuery();
        }

        public void DeleteProduct(Product product)
        {
            string deleteQuery = "DELETE FROM Products WHERE Name = @Name;";
            SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@Name", product.Name);
            deleteCommand.ExecuteNonQuery();
        }

        public void EditProduct(Product product, Product NewProduct)
        {
            string editQuery = "UPDATE Products SET Name = @NewName, Price = @NewPrice WHERE Name = @Name;";
            SQLiteCommand editCommand = new SQLiteCommand(editQuery, connection);

            editCommand.Parameters.AddWithValue("@NewName", NewProduct.Name);
            editCommand.Parameters.AddWithValue("@NewPrice", NewProduct.Price);
            editCommand.Parameters.AddWithValue("@Name", product.Name);
            editCommand.ExecuteNonQuery();
        }

        public List<Product> GetProducts()
        {
            string selectQuery = "SELECT Name, Price, Image FROM Products;";
            SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = selectCommand.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product product = new Product();
                product.Name = reader.GetString(0);
                product.Price = reader.GetDecimal(1);
                product.Image = (byte[])reader.GetValue(2);
                products.Add(product);
            }

            return products;
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }

    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytes = value as byte[];
                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
