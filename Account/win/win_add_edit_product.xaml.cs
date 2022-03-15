using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using DateModel;
using Account.Class;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_add_edit_product.xaml
    /// </summary>
    public partial class win_add_edit_product : Window
    {
        AccountEntities db = new AccountEntities();
        public byte Type { set; get; }
        public int ID { set; get; }
        public string ProName { set; get; }
        public string ProDisc { set; get; }
        public string filename { set; get; }
        public win_add_edit_product()
        {
            InitializeComponent();
        }

        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (!Checkable())
                return;
            else
            {
                try
                {
                  
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    byte[] imgarray = new byte[fs.Length];
                    fs.Read(imgarray, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    switch (Type)
                    {
                        case 1:
                            {
                                db.SP_INSERT_PRODUCTS(txt_name.Text.Trim(), txt_disc.Text.Trim(), imgarray, string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text)), PublicVar.GlobalUserId);
                                db.SaveChanges();
                                MessageBox.Show("اطلاعات شما با موفقیت در پایگاه داده ذخیره گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                            }
                            break;
                        case 2:
                            {
                                PRODUCT P = (from u in db.PRODUCTS where u.ProductId == ID select u).SingleOrDefault();
                                P.ProductDiscription = txt_disc.Text.Trim();
                                P.ProductName = txt_name.Text.Trim();
                                P.ProductImage = imgarray;
                                P.ProductUpdateDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
                                db.SaveChanges();
                                MessageBox.Show("اطلاعات شما با موفقیت بروزرسانی گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                            }
                            break;
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                finally
                { this.Close(); }
            }
           
        }
        private Boolean Checkable()
        {
            if(string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("لطفا نام کالا را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_name.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_disc.Text))
            {
                MessageBox.Show("لطفا توضییحات کالا را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                txt_disc.Focus();
                return false;
            }
            return true;
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            FileDialog open = new OpenFileDialog();
            open.Filter = "Photo Files(*.jpg; *.bmp; *.png; *.ico; *.tiff)|*.jpg;*.png;*.bmp;*.tiff;*.ico;";
            open.ShowDialog();
            filename = open.FileName;
            if(filename!=string.Empty)
            {
                ImageSourceConverter isc = new ImageSourceConverter();
                img_open.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(filename));
            }
            else
                filename = "C:\\Users\\Ali\\source\\repos\\Account\\Account\\img\\add-icon.png";
            open = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_date.Content = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
            lbl_fullname.Content = PublicVar.GlobalForName + " " + PublicVar.GlobalSurName;
            if(Type==2)
            {
                lbl_title.Content = "فرم ویرایش کالا";
                txt_name.Text = ProName;
                txt_disc.Text = ProDisc;
                ShowImage();
            }
        }
        private void ShowImage()
        {
            var query = from p in db.PRODUCTS where p.ProductId == ID select p;
            var run = query.ToList();
            if(run[0].ProductImage!=null)
            {
                Byte[] imgarray = run[0].ProductImage;
                MemoryStream stream = new MemoryStream();
                stream.Write(imgarray, 0, imgarray.Length);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                img_open.Source = bitmap;
            }
        }
    }
}
