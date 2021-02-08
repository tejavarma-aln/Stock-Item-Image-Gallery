using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ImageGallery
{

    public partial class ItemWindow : Window
    {
        private ObservableCollection<ItemModel> itemModels;
        private OpenFileDialog fileDialog;

        public ItemWindow(string title)
        {
            InitializeComponent();
            this.Title = title;
            setup();
            
        }


        private void setup()
        {
            itemModels = new ObservableCollection<ItemModel>(ObjectHolder._items);
            itemGrid.ItemsSource = itemModels;
          
        }

        private void update_bt_Click(object sender, RoutedEventArgs e)
        {
            if(itemGrid.SelectedItems.Count == 0)
            {
                showMessage("Info", "Please select items to update!");
                return;
            }
            StringBuilder xml = new StringBuilder();
            xml.Append("<ENVELOPE><HEADER><TALLYREQUEST>Import data</TALLYREQUEST><TYPE>Data</TYPE><ID>All Masters</ID></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF='TallyUDF'>");
            foreach(ItemModel im  in itemGrid.SelectedItems)
            {
                getItemAlterXML(xml, im.Name, im.Description);
            }
            xml.Append("</TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>");

            string res = new NetworkHandle(xml.ToString(),ObjectHolder.URL).DoPost();
            showMessage("Info", res);
            this.DialogResult = true;
        }


        private void createFileDialog()
        {
            if (fileDialog == null)
            {
                fileDialog = new OpenFileDialog();
                fileDialog.Title = "Select image for item";
                fileDialog.Multiselect = false;
                fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            }
        }

        private void browse_bt_Click(object sender, RoutedEventArgs e)
        {
            
            if(itemGrid.SelectedItems.Count == 0 || itemGrid.SelectedIndex < 0)
            {
                showMessage("Info", "Please select an item to browse!");
                return;
            }
                createFileDialog();

               if(fileDialog.ShowDialog() == true)
                 {
                     itemModels[itemGrid.SelectedIndex].NotifyDescription = fileDialog.FileName;
                 }
             
           }


        private void showMessage(string title,string msg)
        {
            MessageBox.Show(msg, title);
        }

        private void getItemAlterXML(StringBuilder xml,string name,string description)
        {
            xml.Append(string.Format("<STOCKITEM Action='ALTER' Name='{0}'>",name));
            xml.Append(string.Format("<DESCRIPTION>{0}</DESCRIPTION>",description));
            xml.Append("</STOCKITEM>");
        }

    }
}
