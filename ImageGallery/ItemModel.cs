using System.ComponentModel;

namespace ImageGallery
{
    class ItemModel :INotifyPropertyChanged
    {
        public ItemModel()
        { 
        }

        private string name;
        private string unit;
        private string description;
        private string partNo;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public string NotifyDescription
        {
            set { description = value; RaisePropertyChanged();}
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string PartNo
        {
            get { return partNo; }
            set { partNo = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string caller = "")
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

    }
}
