using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : UserControl
    {
        SqlDataAdapter da = new SqlDataAdapter();
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable();
        DataTable DtDelegate = new DataTable();
        DataTable DtCity = new DataTable();
        DataTable DtDeliveryPrice = new DataTable();
        DataTable DtCustomer = new DataTable();
        DataTable DtSerch = new DataTable();
        DataTable DtState = new DataTable();
        Button[] btn = new Button[4];
        bool isnew = false;
        UIElement[] txt = new UIElement[14];
        private string sql;

        void usc_Initialize()
        {
            Order usc = new Order();
            flag.Initialize_uscgrid(usc);
        }
        public Order()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("Select  [ID_Order],[Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date] ,[Note],[users],[Exist],[HasEdit],[WhoIsEdit],[City],[Delegate] , ROW_NUMBER() OVER(ORDER BY[ID_Order]) AS RowNum FROM [dbo].[Orders] where Exist = 'true'");
                dgvOrder.DataContext = Dt;

                DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen FROM [dbo].[Cities] where Exist = 'true' ");
                dgvCity.DataContext = DtCity;

                DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
                cmbState.DataContext = DtState;
                flag.Fill_ComboBox(DtState, cmbState, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.btn_New(btn, grdbtn, txt);
                isnew = true;
                dgvOrder.SelectedIndex = -1;
                btnPrint.IsEnabled = true;
                txtCity.IsEnabled = true;
                txtDelegate.IsEnabled = true;
                btn_Serch.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btn, grdbtn, Dt, dgvOrder.SelectedIndex, txt);
                if (dgvOrder.SelectedIndex == -1) return;
                isnew = false;
                btnPrint.IsEnabled = true;
                txtCity.IsEnabled = true;
                txtDelegate.IsEnabled = true;
                txtCity.Clear();
                txtDelegate.Clear();
                txtCity.Text = Dt.Rows[dgvOrder.SelectedIndex].ItemArray[19].ToString();
                txtDelegate.Text = Dt.Rows[dgvOrder.SelectedIndex].ItemArray[20].ToString();
                dgvCity.Visibility = Visibility.Collapsed;
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Db = new Linq.DbDataContext(flag.Con);
                Linq.Order order = new Linq.Order();


                if (!isnew)
                {
                    if (dgvOrder.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        order = Db.Orders.SingleOrDefault(item => item.Exist == true && item.ID_Order == Convert.ToInt32(Dt.Rows[dgvOrder.SelectedIndex].ItemArray[0]));
                        order.HasEdit = true;
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                order.Barcode = txtBarcode.Text;
                order.Customer = txtCustomer.Text;
                order.CustomerPhone = txtCustomerPhone.Text;
                order.Recipient = txtRecipient.Text;
                order.RecipientPhone1 = txtRecipientPhone1.Text;
                order.RecipientPhone2 = txtRecipientPhone2.Text;
                order.Address = txtAddress.Text;
                order.City = txtCity.Text;
                order.PackagePrice = Convert.ToDecimal(txtPackagePrice.Text);
                order.PackageNumber = Convert.ToInt32(txtPackageNumber.Text);
                order.DeliveryPrice = Convert.ToDecimal(txtDeliveryPrice.Text);
                order.TotalPrice = Convert.ToDecimal(txtTotalPrice.Text);
                order.Delegate = txtDelegate.Text;
                order.State = cmbState.Text;
                order.Date = Convert.ToDateTime(txtDate.Text);
                order.Note = txtNote.Text;
                order.Exist = true;

                if (isnew)
                {
                    Db.Orders.InsertOnSubmit(order);
                }

                Db.SubmitChanges();

                if (isnew)
                {
                    MessageBox.Show("تم الحفظ");
                }
                usc_Initialize();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag.Dellete("Orders", "ID_Order", Dt, dgvOrder);
                usc_Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSerch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (cmbSerch.SelectedIndex == 0)
                {
                    sql = "Barcode";
                }
                if (cmbSerch.SelectedIndex == 1)
                {
                    sql = "Customer";
                }
                if (cmbSerch.SelectedIndex == 2)
                {
                    sql = "CustomerPhone";
                }
                Dt.Clear();
                da = new SqlDataAdapter("Select  [ID_Order],[Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date]  ,[Note],[users],[Exist],[HasEdit],[WhoIsEdit],[City],[Delegate] , ROW_NUMBER() OVER(ORDER BY[ID_Order]) AS RowNum FROM [dbo].[Orders] where Exist = 'true' and " + sql + " like '%'+'" + txtSerch.Text + "'+'%'", flag.Con);
                da.Fill(Dt);
                dgvOrder.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBarcode_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
            dgvCity.Visibility = Visibility.Collapsed;
            dgvDelegate.Visibility = Visibility.Collapsed;
        }

        private void txtCustomerPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbCustomer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {

                //da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Customer]) AS RowNum FROM [dbo].[Customers] where Exist = 'true' and Name like '%'+'" + txtSerch.Text + "'+'%' ", flag.Con);        
                // da.Fill(DtSerch);
                // cmbCustomer.DataContext =DtSerch;

            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);


            }
        }

       
       

        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
       {
            try
            {        
                if(dgvCity.SelectedIndex == -1)
                {
                    DtCity.Clear();
                    DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen FROM [dbo].[Cities] where Exist = 'true' and CityName like '%'+ '" + txtCity.Text + "' +'%'");
                    dgvCity.DataContext = DtCity;
                    if (DtCity.Rows.Count > 0)
                    {
                        dgvCity.Visibility = Visibility.Visible;

                    }
                    if (txtCity.Text == string.Empty)
                    {

                        dgvCity.Visibility = Visibility.Visible;
                        DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen FROM [dbo].[Cities] where Exist = 'true' ");
                        dgvCity.DataContext = DtCity;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtCity_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                txtCity.SelectAll();
                if (dgvCity.Visibility == Visibility.Visible)
                {
                    dgvCity.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgvCity.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCity.SelectedIndex == -1) return;
                txtCity.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[0].ToString();
                txtDeliveryPrice.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[1].ToString();
                dgvCity.Visibility = Visibility.Collapsed;
                dgvCity.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }

        private void dgvDelegate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtDelegate.Text = DtDelegate.Rows[dgvDelegate.SelectedIndex].ItemArray[0].ToString();
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

       
        private void txtPackagePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal packageprice = 0, deliveryprice = 0;
                if (!decimal.TryParse(txtPackagePrice.Text, out packageprice)) packageprice = 0;
                if (!decimal.TryParse(txtDeliveryPrice.Text, out deliveryprice)) deliveryprice = 0;
                txtTotalPrice.Text = (packageprice + deliveryprice).ToString();
            }
            catch (Exception ex)
            {
            }

        }


        private void txtDelegate_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                txtDelegate.SelectAll();
                if (dgvDelegate.Visibility == Visibility.Visible)
                {
                    dgvDelegate.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgvDelegate.Visibility = Visibility.Visible;
                    DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'");
                    dgvDelegate.DataContext = DtDelegate;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtDelegate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DtDelegate.Clear();
                DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM [dbo].[Delegates] where Exist = 'true' and Name like '%'+ '" + txtDelegate.Text + "' +'%'");
                dgvDelegate.DataContext = DtDelegate;
                if (DtDelegate.Rows.Count > 0)
                {
                    dgvDelegate.Visibility = Visibility.Visible;

                }
                if (txtDelegate.Text == string.Empty)
                {
                    DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'");
                    dgvDelegate.DataContext = DtDelegate;

                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
