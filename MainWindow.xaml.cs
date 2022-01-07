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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SalonLotModel;
using System.Data.Entity;
using System.Data;

namespace Salon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        SalonLotEntitiesModel ctx = new SalonLotEntitiesModel();
        CollectionViewSource frizeriVSource,clientiVSource;

        private void SaveClienti()
        {
            Clienti clienti = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    clienti = new Clienti()
                    {
                        Nume = numeTextBox.Text.Trim(),
                        Prenume = prenumeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Clientis.Add(clienti);
                    clientiVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    clienti = (Clienti)clientiDataGrid.SelectedItem;
                    clienti.Nume = numeTextBox.Text.Trim();
                    clienti.Prenume = prenumeTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    clienti = (Clienti)(clientiDataGrid.SelectedItem);
                    ctx.Clientis.Remove(clienti);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                clientiVSource.View.Refresh();
            }

        }
        private void SaveFrizeri()
        {
            Frizeri frizeri = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Inventory entity
                    frizeri = new Frizeri()
                    {
                        Numef = numefTextBox.Text.Trim(),
                        Ziua = ziuaTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Frizeris.Add(frizeri);
                    frizeriVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    frizeri = (Frizeri)frizeriDataGrid.SelectedItem;
                    frizeri.Numef = numefTextBox.Text.Trim();
                    frizeri.Ziua = ziuaTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    frizeri = (Frizeri)frizeriDataGrid.SelectedItem;
                    ctx.Frizeris.Remove(frizeri);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                frizeriVSource.View.Refresh();
            }

        }

        CollectionViewSource clientiProgramaresVSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            clientiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiViewSource")));
            clientiVSource.Source = ctx.Clientis.Local;
            ctx.Clientis.Load();
            clientiProgramaresVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiProgramaresViewSource")));
           // clientiProgramaresVSource.Source = ctx.Programares.Local;
            ctx.Programares.Load();
            ctx.Frizeris.Load();
            cmbClienti.ItemsSource = ctx.Clientis.Local;
            //cmbClienti.DisplayMemberPath = "Nume";
            cmbClienti.SelectedValuePath = "ClId";
            cmbFrizeri.ItemsSource = ctx.Frizeris.Local;
            //cmbFrizeri.DisplayMemberPath = "Numef";
            cmbFrizeri.SelectedValuePath = "FrId";

            frizeriVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("frizeriViewSource")));
            frizeriVSource.Source = ctx.Frizeris.Local;
            ctx.Frizeris.Load();

            System.Windows.Data.CollectionViewSource clientiViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiViewSource")));
            System.Windows.Data.CollectionViewSource frizeriViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("frizeriViewSource")));
            BindDataGrid();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void tbCtrlSalonLot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            clientiVSource.View.MoveCurrentToNext();
        }
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            clientiVSource.View.MoveCurrentToPrevious();
        }
        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            frizeriVSource.View.MoveCurrentToNext();
        }
        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            frizeriVSource.View.MoveCurrentToPrevious();
        }


        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }
        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }

        private void clientiDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlSalonLot.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Clienti":
                    SaveClienti();
                    break;
                case "Frizeri":
                    SaveFrizeri();
                    break;
                case "Programare":
                    break;
            }
            ReInitialize();
        }
        private void SaveProgramare()
        {
            Programare programare = null;
            if (action == ActionState.New)
            {
                try
                {
                    Clienti clienti = (Clienti)cmbClienti.SelectedItem;
                    Frizeri frizeri = (Frizeri)cmbFrizeri.SelectedItem;
                    //instantiem Order entity
                    programare = new Programare()
                    {

                        ClId = clienti.ClId,
                        FrId = frizeri.FrId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Programares.Add(programare);
                    //salvam modificarile
                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
              if (action == ActionState.Edit)
            {
                dynamic selectedProgramare = programaresDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedProgramare.PrId;
                    var editedProgramare = ctx.Programares.FirstOrDefault(s => s.PrId == curr_id);
                    if (editedProgramare != null)
                    {
                        editedProgramare.ClId = Int32.Parse(cmbClienti.SelectedValue.ToString());
                        editedProgramare.FrId = Convert.ToInt32(cmbFrizeri.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                clientiProgramaresVSource.View.MoveCurrentTo(selectedProgramare);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedProgramare = programaresDataGrid.SelectedItem;
                    int curr_id = selectedProgramare.PrId;
                    var deletedProgramare = ctx.Programares.FirstOrDefault(s => s.PrId == curr_id);
                    if (deletedProgramare != null)
                    {
                        ctx.Programares.Remove(deletedProgramare);
                        ctx.SaveChanges();
                        MessageBox.Show("Programare stearsa ", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void BindDataGrid()
        {
            var queryOrder = from pro in ctx.Programares
                             join cli in ctx.Clientis on pro.ClId equals
                             cli.ClId
                             join fri in ctx.Frizeris on pro.FrId
                 equals fri.FrId
                             select new
                             {
                                 pro.PrId,
                                 pro.FrId,
                                 pro.ClId,
                                 cli.Nume,
                                 cli.Prenume,
                                 fri.Numef,
                                 fri.Ziua
                             };
            clientiProgramaresVSource.Source = queryOrder.ToList();
        }

        private void frizeriDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbClienti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SetValidationBinding()
        {
            Binding NumeValidationBinding = new Binding();
            NumeValidationBinding.Source = clientiVSource;
            NumeValidationBinding.Path = new PropertyPath("Nume");
            NumeValidationBinding.NotifyOnValidationError = true;
            NumeValidationBinding.Mode = BindingMode.TwoWay;
            NumeValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            NumeValidationBinding.ValidationRules.Add(new StringNotEmpty());
            numeTextBox.SetBinding(TextBox.TextProperty,
           NumeValidationBinding);
            Binding PrenumeValidationBinding = new Binding();
            PrenumeValidationBinding.Source = clientiVSource;
            PrenumeValidationBinding.Path = new PropertyPath("Prenume");
            PrenumeValidationBinding.NotifyOnValidationError = true;
            PrenumeValidationBinding.Mode = BindingMode.TwoWay;
            PrenumeValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            PrenumeValidationBinding.ValidationRules.Add(new
           StringMinLengthValidator());
            prenumeTextBox.SetBinding(TextBox.TextProperty,
           PrenumeValidationBinding); //setare binding nou
        }

    }
}
