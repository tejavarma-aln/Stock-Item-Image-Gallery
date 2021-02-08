using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ImageGallery
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<ItemModel> _items;
        List<ItemModel> _invalidItems;
        List<ItemModel> _configuredItems;
        private readonly string ini_path  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.ini");

        private void connect_bt_Click(object sender, RoutedEventArgs e)
        {
            string host = hostIp.Text.Trim();
            string port = hostPort.Text.Trim();
            if(host.Length == 0)
            {
                showMessage("Invalid host ip", "Info");
                return;
            }
            if (port.Length == 0)
            {
                showMessage("Invalid port", "Info");
                return;
            }

             getItems(ObjectHolder.makeHostName(host,port));
             diaplyDashboard();
            
            
        }


        private void getItems(string host)
        {
            string res = new NetworkHandle(ObjectHolder.getItemXML(), host).DoPost();
            if (res != null)
            {
                saveHostDetails();

                XDocument xml = XDocument.Parse(res);
                if (_items == null)
                {
                    _items = new List<ItemModel>();
                }
                else
                {
                    _items.Clear();
                }
                try
                {
                    foreach (var node in xml.Root.Element("BODY").Element("DATA").Element("COLLECTION").Elements("STOCKITEM"))
                    {

                        ItemModel item = new ItemModel();
                        item.Name = node.Attribute("NAME").Value;
                        item.Unit = node.Element("BASEUNITS").Value;
                        item.Description = node.Element("DESCRIPTION").Value;
                        item.PartNo = node.Element("MAILINGNAME").Value;
                        _items.Add(item);
                    }
                }
                catch (Exception e1)
                {
                    showMessage(e1.Message,"Error");
                    return;
                }

            }
        }

        private void saveHostDetails()
        {
            if(saveCheck.IsChecked == true)
            {
                string data = string.Format("<CONFIG><IP>{0}</IP><PORT>{1}</PORT></CONFIG>",hostIp.Text.Trim(),hostPort.Text.Trim());
                File.WriteAllText(ini_path, data);
            }
        }

        private void setDashBoard()
        {
            _configuredItems = _items.Where(i => File.Exists(i.Description)).ToList();
            _invalidItems = _items.Where(i => !File.Exists(i.Description)).ToList();
            itemst1.Text = _items.Count.ToString();
            itemst2.Text = _invalidItems.Count.ToString();
            itemst3.Text = _configuredItems.Count.ToString();
        }

        private void diaplyDashboard()
        {
            setDashBoard();
            loginPanel.Visibility = Visibility.Collapsed;
            dashboard.Visibility = Visibility.Visible;
            
        }

        private void showMessage(string msg,string title)
        {
            MessageBox.Show(msg, title);
        }

        private void show_item_Click(object sender, RoutedEventArgs e)
        {
            ObjectHolder._items = _configuredItems;
            ItemWindow itemWindow = new ItemWindow("Valid Items");
            if(itemWindow.ShowDialog() == true)
            { 
              reload();
            }
            
        }

        private void show_invalid_Click(object sender, RoutedEventArgs e)
        {
            ObjectHolder._items = _invalidItems;
            ItemWindow itemWindow = new ItemWindow("Items With Invalid Path");
            if (itemWindow.ShowDialog() == true)
            {
              reload();
            }
        }


        private void reload()
        {
            if (MessageBox.Show("Do you to reload items?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                getItems(ObjectHolder.URL);
                setDashBoard();
            }
        }

        private void reload_bt_Click(object sender, RoutedEventArgs e)
        {

            reload();
        }

        private void Present_Click(object sender, RoutedEventArgs e)
        {
            if (_configuredItems.Count > 0)
            {
                ObjectHolder._items = _configuredItems;
                Gallery gallery = new Gallery(0);
                gallery.ShowDialog();
            }
            else
            {
                showMessage("No items found with proper path to start presentation", "Info");
            }
        }

        private void SlideShow_Click(object sender, RoutedEventArgs e)
        {
            if (_configuredItems.Count > 0)
            {
                ObjectHolder._items = _configuredItems;
                Gallery gallery = new Gallery(1);
                gallery.ShowDialog();
            }
            else
            {
                showMessage("No items found with proper path to start slide show", "Info");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (File.Exists(ini_path))
                {
                    XDocument xml = XDocument.Load(ini_path);
                    hostIp.Text = xml.Root.Element("IP").Value;
                    hostPort.Text = xml.Root.Element("PORT").Value;
                }
            }
            catch (Exception e2)
            {
                showMessage(e2.Message, "Error");
                return;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (_configuredItems.Count > 0)
            {
                ObjectHolder._items = _configuredItems;
                Search search = new Search();
                search.ShowDialog();
            }
            else
            {
                showMessage("No items found with proper path to search", "Info");
            }
        }
    }
}
