using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for Movement.xaml
    /// </summary>
    public partial class Movement : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable();
        DataTable DtState = new DataTable();
        DataTable DtDelegate = new DataTable();
        DataTable DtCity = new DataTable();
        DataTable DtSearch = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        Button[] btn = new Button[3];
        bool isnew = false;
        int test = 0;
        public Movement()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
            cmbState.DataContext = DtState;
            flag.Fill_ComboBox(DtState, cmbState, 1);
            Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Province]) AS RowNum FROM [dbo].[Provinces] where Exist = 'true'");
            flag.Fill_ComboBox(Dt, cmbProvince, 1);

        }
        private void btnAllEdit_Click(object sender, RoutedEventArgs e)
        {
            USC.StateEdit edit = new USC.StateEdit();
            grdEdit.Children.Clear();
            grdEdit.Children.Add(edit);
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
                    DtCity = flag.Fill_DataGrid_join("Select  CityName FROM[dbo].[Cities] where Exist = 'true'");
                    dgvCity.DataContext = DtCity;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DtCity.Clear();
                DtCity = flag.Fill_DataGrid_join("Select CityName FROM [dbo].[Cities] where Exist = 'true' and CityName like '%'+ '" + txtCity.Text + "' +'%'");
                dgvCity.DataContext = DtCity;
                if (DtCity.Rows.Count > 0)
                {
                    dgvCity.Visibility = Visibility.Visible;

                }
                if (txtCity.Text == string.Empty)
                {
                    DtCity = flag.Fill_DataGrid_join("Select CityName FROM [dbo].[Cities] where Exist = 'true'");
                    dgvCity.DataContext = DtCity;

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dgvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtCity.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[0].ToString();
                dgvCity.Visibility = Visibility.Collapsed;
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

        private void cbxState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbState.IsEnabled = (bool)cbxState.IsChecked;
                cmbState.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxDelegate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDelegate.IsEnabled = (bool)cbxDelegate.IsChecked;
                txtDelegate.Clear();
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxProvince_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbProvince.IsEnabled = (bool)cbxProvince.IsChecked;
                cmbProvince.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtCity.IsEnabled = (bool)cbxCity.IsChecked;
                txtCity.Text = "";
                dgvCity.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_from_date.IsEnabled = (bool)cbxDate.IsChecked;
                txt_to_date.IsEnabled = (bool)cbxDate.IsChecked;
                txt_from_date.Text = "";
                txt_to_date.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DtSearch.Clear();
                da = new SqlDataAdapter("Select  [ID_Order],[Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date]  ,[Note],[users],[Exist],[HasEdit],[WhoIsEdit],[City],[Delegate] , ROW_NUMBER() OVER(ORDER BY[ID_Order]) AS RowNum FROM [dbo].[Orders] where Exist = 'true' and State like '%'+'" + cmbState.Text + "'+'%' and Delegate like '%'+'" + txtDelegate.Text + "'+'%' and City like '%'+'" + txtCity.Text + "'+'%' ", flag.Con);
                da.Fill(DtSearch);
                dgvMovement.DataContext = DtSearch;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
