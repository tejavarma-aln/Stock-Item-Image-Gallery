using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageGallery
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()

        {
            InitializeComponent();
        }

        private void itemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(itemList.SelectedIndex != -1)
            {
                searchKey.Text = itemList.SelectedItem.ToString();
                ItemModel im =(ItemModel)itemInfo.Items[0];
                selectedImage.Source = new BitmapImage(new Uri(im.Description));
                itemList.ItemsSource = null;
                itemList.Visibility = Visibility.Collapsed;
            }

        }

        private void searchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            string key = searchKey.Text.Trim();
            if (!String.IsNullOrEmpty(key))
            {
                List<String> _temp = ObjectHolder._items.Where(i => i.Name.ToLower().Contains(key.ToLower())).Select(i => i.Name).ToList();
                if (_temp.Count > 0)
                {
                    itemList.ItemsSource = _temp;
                    itemInfo.ItemsSource = ObjectHolder._items.Where(i => i.Name.ToLower().Equals(key.ToLower())).ToList();
                    itemList.Visibility = Visibility.Visible;
                }
            }
                
            else
            {
                itemList.Visibility = Visibility.Collapsed;
            }
        }
    }
}