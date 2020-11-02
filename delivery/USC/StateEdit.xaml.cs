using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for StateEdit.xaml
    /// </summary>
    public partial class StateEdit : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable();
        DataTable DtState = new DataTable();
        Button[] btn = new Button[3];
        bool isnew = false;
        public StateEdit()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
            cmbState.DataContext = DtState;
            flag.Fill_ComboBox(DtState, cmbState, 1);
        }
        private void btnShutDown_Click(object sender, RoutedEventArgs e)
        {
            
        }

       
    }
}
