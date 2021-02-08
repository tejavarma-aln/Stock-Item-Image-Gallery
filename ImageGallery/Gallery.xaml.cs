using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageGallery
{
    /// <summary>
    /// Interaction logic for Gallery.xaml
    /// </summary>
    public partial class Gallery : Window
    {
        int displayMode = 0;

        public Gallery(int mode)
        {
            InitializeComponent();
            this.displayMode = mode;
            setup();
        }

        private List<ItemModel> items;
        private int counter = 0;
        private Timer timer;
       

        private void setup()
        {
            items = new List<ItemModel>(ObjectHolder._items);
            switch (displayMode)
            {
                case 0:
                    startPresentation();
                    break;
                case 1:
                    prev_bt.Visibility = Visibility.Hidden;
                    next_bt.Visibility = Visibility.Hidden;
                    startSlideShow();
                    break;
                default:
                    break;
            }
            
        }

        private void startSlideShow()
        {
            currentImage.Source = new BitmapImage(new Uri(items[counter].Description));
            imagDescription.Text = items[counter].Name.ToUpper();
            timer = new Timer(3000);
            timer.Elapsed += new ElapsedEventHandler(handleSlide);
            timer.Start();
        }

        private void handleSlide(object sender, ElapsedEventArgs e)
        {
            if ((items.Count == (counter + 1)) && counter != 0)
            {
                timer.Stop();
                timer.Dispose();
                if(MessageBox.Show("Slide show ended\n Do you want to start again?", "Info", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        counter = 0;
                        startSlideShow();
                    }));
                }
            }
            else
            {
               moveSlide();
            }
           
        }


        private void moveSlide()
        {
            counter += 1;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                currentImage.Source = new BitmapImage(new Uri(items[counter].Description));
                imagDescription.Text = items[counter].Name.ToUpper();
            }));
        }


        private void startPresentation()
        {
            currentImage.Source = new BitmapImage(new Uri(items[counter].Description));
            imagDescription.Text = items[counter].Name.ToUpper();
        }

        private void next_bt_Click(object sender, RoutedEventArgs e)
        {
            if((items.Count == (counter + 1)) && counter != 0)
            {
                showMessage("Info", "You reached end of slide show");
                return;
            }
            counter += 1;
            currentImage.Source = new BitmapImage(new Uri(items[counter].Description));
            imagDescription.Text = items[counter].Name.ToUpper();
        }

        private void prev_bt_Click(object sender, RoutedEventArgs e)
        {
            if (counter == 0)
            {
                return;
            }
            counter -= 1;
            currentImage.Source = new BitmapImage(new Uri(items[counter].Description));
            imagDescription.Text = items[counter].Name.ToUpper();
        }

          private void showMessage(string title, string msg)
         {
            MessageBox.Show(msg, title);  
         }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(timer != null)
            {
                timer.Close();
                timer.Dispose();
            }
        }
    }
}
