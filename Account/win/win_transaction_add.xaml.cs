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
using DateModel;
using Account.Class;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_transaction_add.xaml
    /// </summary>
    public partial class win_transaction_add : Window
    {
        public int ID { set; get; }
        public string Pname { set; get; }
        public int Suply { set; get; }
        AccountEntities db = new AccountEntities();
        public win_transaction_add()
        {
            InitializeComponent();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_date.Content = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(cal.Text));
            lbl_fullname.Content = PublicVar.GlobalForName + " " + PublicVar.GlobalSurName;
            lbl_title.Content = Pname;
            for(int i=1;i<=50;i++)
            {
                cmb_count.Items.Add(i);
            }
            cmb_count.SelectedIndex = 5;
            cmb_num.Items.Add("افزایش موجودی");
            cmb_num.Items.Add("کاهش موجودی");
            cmb_num.SelectedIndex = 0;
        }

        private Boolean CheckTr()
        {
            int i = Suply += SetCount();
            if(i<0)
            {
                MessageBox.Show("موجودی مربوط به این کالا کافی نمی باشد.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                return false;
            }
            return true;
        }
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckTr())
                    return;
                int Loacl = SetCount();
                if (string.IsNullOrWhiteSpace(txt_dis.Text))
                    txt_dis.Text = "ندارد";
                db.SP_INSERT_TRANSACTION(Loacl, txt_dis.Text.Trim(), lbl_date.Content.ToString(), PublicVar.GlobalUserId, this.ID);
                db.SP_UPDATE_PRODUCTLASTSUPLY(this.ID, Loacl);
                db.SaveChanges();
                MessageBox.Show("اطلاعات شما با موفقیت ثبت گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            }
            finally
            {
                this.Close();
            }
            
        }
        private int SetCount()
        {
            if (cmb_num.SelectedIndex == 1)
                return -Convert.ToInt32(cmb_count.SelectedIndex + 1);
            return Convert.ToInt32(cmb_count.SelectedIndex + 1);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
