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
using System.Transactions;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DateModel;
using Account.Class;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Account.win
{
    /// <summary>
    /// Interaction logic for win_Factor.xaml
    /// </summary>
    public partial class win_Factor : Window
    {
        AccountEntities db = new AccountEntities();
        public int FactorId { set; get; }
        public int ProductId { set; get; }
        public string CustomerName { set; get; }
        public long SellPrice { set; get; }
        public long PurchPrice { set; get; }
        public string Date { set; get; }
        public Boolean Type { set; get; }
        public win_Factor()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txt_no_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Type == true)
            {
                ShowInfo(SearchInputC);
                ShowProduct(SearchInputP);
                SetCombo();
                SetLoad();
            }
            else if(Type==false)
            {
                SetLoad2();
            }
        }
        private void SetLoad2()
        {
            ShowInfo(SearchInputC);
            ShowProduct(SearchInputP);
            SetCombo();
            ShowInfoDG();
            img_search_pro.IsEnabled = false;
            lbl_factor_code.Content = FactorId;
            lbl_date.Content = Date;
            lbl_factor_price.Content = SellPrice;
            btn_end_factor.Content = "بروزرسانی فاکتور";
            lbl_factor_purch.Content = PurchPrice;
            lbl_customer_name.Content = CustomerName;
            btn_add.Visibility = Visibility.Hidden;
            txt_pro_name.IsEnabled = false;
            dg_customer.IsEnabled = false;
            lbl_title.Content = "فرم ویرایش مشتریان";
            txt_no.IsEnabled = false;
            btn_submit.IsEnabled = false;
            txt_name.IsEnabled = false;
            btn_select.IsEnabled = true;
            img_search.IsEnabled = false;
        }
        private void ShowProduct(Func<string> Input)
        {
            
            var Query = db.Database.SqlQuery<VW_PRODUCTS>("SELECT * FROM VW_PRODUCTS WHERE ProductActive = 1 " + Input());
            var Run = Query.ToList();
            datagrid_pro.ItemsSource = Run;
        }
        private string SearchInputP()
        {
            string Result = "";
            if (!string.IsNullOrEmpty(txt_pro_name.Text))
                Result += "AND ProductName LIKE '%" + txt_pro_name.Text.Trim() + "%' ";
            return Result;
        }
        private void SetLoad()
        {
            lbl_date.Content = string.Format("{0:yyyy/MM/dd}",Convert.ToDateTime(cal.Text));
            lbl_fullname.Content = PublicVar.GlobalForName + " " + PublicVar.GlobalSurName;
            btn_del_pro.IsEnabled = false;
            btn_end_factor.IsEnabled = false;
        }
        private void ShowInfo(Func<string> Input)
        {
            var Query = db.Database.SqlQuery<VW_CUSTOMERS>("SELECT * FROM VW_CUSTOMERS WHERE IsActive = 1 "+Input());
            var Run = Query.ToList();
            dg_customer.ItemsSource = Run;
        }
        private string SearchInputC()
        {
            string Result = string.Empty;
            if (txt_name.Text.Trim() != string.Empty)
                Result += "AND Name LIKE '%"+txt_name.Text.Trim()+"%' ";
            if (!string.IsNullOrEmpty(txt_no.Text))
                Result += "AND Mobile LIKE '%"+txt_no.Text.Trim()+"%' ";
            return Result;

        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowInfo(SearchInputC);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {

            var Obj = dg_customer.SelectedItem;
           
            if (Obj != null)
            {
                if ((dg_customer.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text == string.Empty)
                {
                    MessageBox.Show("کاربر گرامی لطفا یک سلول معتبر را انتخاب نمایید \n با تشکر", "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Stop, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                    return;
                }
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        FACTOR F = new FACTOR();
                        F.CustomerId = Convert.ToInt32((dg_customer.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                        F.LastBuyFee = 0;
                        F.LastPurchFee = 0;
                        F.StartDate = lbl_date.Content.ToString();
                        F.UserId = PublicVar.GlobalUserId;
                        db.FACTORs.Add(F);
                        db.SaveChanges();
                        lbl_customer_name.Content = (dg_customer.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text;
                        var Query = db.Database.SqlQuery<FACTOR>("SELECT TOP 1 * FROM FACTOR ORDER BY Id DESC");
                        var Run = Query.ToList();
                        FactorId= Run[0].Id;
                        lbl_factor_code.Content = this.FactorId;
                        lbl_factor_price.Content = 0;
                        lbl_factor_purch.Content = 0;
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                        
                    }
                    finally
                    {
                        btn_add.IsEnabled = false;
                        btn_select.IsEnabled = true;
                        
                    }
                }
            }
            else
                MessageBox.Show("لطفا مشتری موردنظر را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            ShowProduct(SearchInputP);
        }
        private void SetCombo()
        {
            for(int i=1;i<=20;i++)
            cmb_count.Items.Add(i);
            cmb_count.SelectedIndex = 0;
        }

        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            object Obj = datagrid_pro.SelectedItem;
            if(Obj!=null)
            {
                lbl_exit.Content = (datagrid_pro.SelectedCells[3].Column.GetCellContent(Obj) as TextBlock).Text;
                lbl_price.Content= (datagrid_pro.SelectedCells[4].Column.GetCellContent(Obj) as TextBlock).Text;
                lbl_purch.Content = (datagrid_pro.SelectedCells[5].Column.GetCellContent(Obj) as TextBlock).Text;
                lbl_selecte_pro.Content= (datagrid_pro.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text;
                btn_submit.IsEnabled = true;
            }
            else
                MessageBox.Show("لطفا کالای موردنظر را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }

        private void ShowInfoDG()
        {
            var Query = db.Database.SqlQuery<VW_FACTORITEMS>("SELECT * FROM VW_FACTORITEMS WHERE FACTORID = "+this.FactorId);
            var Run = Query.ToList();
            dg_orderdpro.ItemsSource = Run;
        }
        
        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {

            object Obj = datagrid_pro.SelectedItem;
            if(Obj!=null)
            {
                using (TransactionScope ts = new TransactionScope())
                {

                
                    var Q = (from u in db.FACTORITEMS where u.FactorId == this.FactorId select u);
                var R = Q.ToList();
                for (int i = 0; i < R.Count; ++i)
                {
                    if (R[i].ProductId == int.Parse((datagrid_pro.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text))
                    {
                        MessageBox.Show("برای افزودن لطفا کالای موردنظر را ابتدا حذف نمایید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                            BlankLbl();
                        return;
                    }
                }
                int EN = Convert.ToInt32((datagrid_pro.SelectedCells[3].Column.GetCellContent(Obj) as TextBlock).Text);
                int CN = cmb_count.SelectedIndex + 1;
                    if (EN < CN)
                    {
                        MessageBox.Show("موجودی کالا کافی نیست", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        return;
                    }

                    else
                    {

                        try
                        {
                            FACTORITEM FI = new FACTORITEM();
                            FI.COUNT = CN;
                            FI.FactorId = this.FactorId;
                            FI.LastBuyFee = int.Parse((datagrid_pro.SelectedCells[5].Column.GetCellContent(Obj) as TextBlock).Text);
                            FI.LastPurchFee = int.Parse((datagrid_pro.SelectedCells[4].Column.GetCellContent(Obj) as TextBlock).Text);
                            //lbl_productprice.Content = FI.LastPurchFee;
                            FI.ProductId = int.Parse((datagrid_pro.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                            //lbl_proudctid.Content = FI.ProductId;
                            db.FACTORITEMS.Add(FI);
                            db.SP_UPDATE_PRODUCTLASTSUPLY(Convert.ToInt32((datagrid_pro.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text), -Convert.ToInt32(CN));
                            db.SaveChanges();
                            ShowInfoDG();
                            ShowProduct(SearchInputP);
                            ts.Complete();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        }
                        finally
                        {
                            btn_submit.IsEnabled = false;
                            btn_end_factor.IsEnabled = true;
                            btn_del_pro.IsEnabled = true;
                            lbl_factor_price.Content = Convert.ToInt64(lbl_factor_price.Content) + CN * Convert.ToInt32(lbl_price.Content);
                            lbl_factor_purch.Content = Convert.ToInt64(lbl_factor_purch.Content) + CN * Convert.ToInt32(lbl_purch.Content);
                            BlankLbl();
                        }
                    }
                }
            }
        }
        private void BlankLbl()
        {
            lbl_selecte_pro.Content = "...";
            lbl_price.Content = "...";
            lbl_purch.Content = "...";
            lbl_exit.Content = "...";
            cmb_count.SelectedIndex = 0;
        }

        private void btn_del_pro_Click(object sender, RoutedEventArgs e)
        {
            
            Object Obj = dg_orderdpro.SelectedItem;
            
           
            if (Obj!=null)
            {
                if ((dg_orderdpro.SelectedCells[1].Column.GetCellContent(Obj) as TextBlock).Text == string.Empty)
                {
                    MessageBox.Show("کاربر گرامی لطفا یک سلول معتبر را انتخاب نمایید \n با تشکر", "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Stop, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                    return;
                }
                if (MessageBox.Show("آیا از حذف کالای موردنظر مطمین هستید؟", "سوال", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
                {

                    int Count= int.Parse((dg_orderdpro.SelectedCells[4].Column.GetCellContent(Obj) as TextBlock).Text);
                    int Price = int.Parse((dg_orderdpro.SelectedCells[2].Column.GetCellContent(Obj) as TextBlock).Text);
                    int Purch = int.Parse((dg_orderdpro.SelectedCells[3].Column.GetCellContent(Obj) as TextBlock).Text);
                    ProductId = int.Parse((dg_orderdpro.SelectedCells[0].Column.GetCellContent(Obj) as TextBlock).Text);
                    try
                    {
                        var Query = (from II in db.FACTORITEMS where II.FactorId == this.FactorId where II.ProductId == ProductId select II).SingleOrDefault();
                        db.FACTORITEMS.Remove(Query);
                        db.SaveChanges();
                        MessageBox.Show("کالای موردنظر با موفقیت حذف گردید", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                    finally
                    {
                        db.SP_UPDATE_PRODUCTLASTSUPLY(ProductId, Count);
                        db.SaveChanges();
                        lbl_factor_price.Content = Convert.ToInt32(lbl_factor_price.Content) - Count * Price;
                        lbl_factor_purch.Content = Convert.ToInt32(lbl_factor_purch.Content) - Count * Purch;
                        ShowInfoDG();
                        ShowProduct(SearchInputP);
                    }
                    
                }
            }
            else
                MessageBox.Show("لطفا کالای موردنظر را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK,MessageBoxOptions.RightAlign);
        }

        private void ClearForm()
        {
            dg_orderdpro.ItemsSource = null;
            dg_orderdpro.Items.Clear();
            btn_add.IsEnabled = true;
            btn_select.IsEnabled = false;
            btn_submit.IsEnabled = false;
            btn_del_pro.IsEnabled = false;
            btn_end_factor.IsEnabled = false;
            lbl_customer_name.Content = string.Empty;
            lbl_exit.Content = string.Empty;
            lbl_factor_code.Content = string.Empty;
            lbl_factor_price.Content = string.Empty;
            lbl_factor_purch.Content = string.Empty;
            lbl_price.Content = string.Empty;
            lbl_proudctid.Content = string.Empty;
            lbl_purch.Content = string.Empty;
            lbl_selecte_pro.Content = string.Empty;
            txt_name.Text = string.Empty;
            txt_no.Text = string.Empty;
            txt_pro_name.Text = string.Empty;
            cmb_count.SelectedIndex = 0;
            FactorId = 0;
            ShowInfo(SearchInputC);
            ShowInfoDG();
            ShowProduct(SearchInputP);
        }
        private void btn_end_factor_Click(object sender, RoutedEventArgs e)
        {
           
            if (0 == Convert.ToInt32(lbl_factor_price.Content))
            {
                MessageBox.Show("کاربر گرامی شما هیچ کالایی را انتخاب نکرده اید.", "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                return;
            }
            if (SellPrice == Convert.ToInt32(lbl_factor_price.Content))
            {
                MessageBox.Show("شما هیچ تغییری ایجاد نکرده اید، لطفا روی دکمه ی خروج کلیک کنید.", "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                return;
            }
            if (MessageBox.Show("آیا از انجام این عملیات اطمینان دارید؟","سوال",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No,MessageBoxOptions.RightAlign)==MessageBoxResult.Yes)
            {
                try
                {
                    FACTOR FI = (from f in db.FACTORs where f.Id == this.FactorId select f).SingleOrDefault();
                    FI.LastBuyFee = Convert.ToInt64(lbl_factor_price.Content);
                    FI.LastPurchFee = Convert.ToInt64(lbl_factor_purch.Content);
                    FI.PurchType = true;
                    db.SaveChanges();
                    MessageBox.Show("فاکتور مربوطه با موفقیت ثبت شد.", "اطلاعات", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                finally
                {
                    ClearForm();
                }
            }
        }
    }
}
