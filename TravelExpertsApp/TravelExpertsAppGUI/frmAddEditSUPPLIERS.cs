using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TravelExpertsData;

namespace TravelExpertsAppGUI
{
    public partial class frmAddEditSUPPLIERS : Form
    {

        // These public properties are set by the main form
        public Suppliers Supplier { get; set; } // selected product on the main form
        public bool AddSupplier { get; set; } // flag that distinguishes Add from Modify
        public frmAddEditSUPPLIERS()
        {
            InitializeComponent();
        }

        private void frmAddEditSUPPLIERS_Load(object sender, EventArgs e)
        {
            if (AddSupplier) // this is Add
            {
                this.Text = "Add new Supplier";
                txtSupplierId.ReadOnly = false;  
            }
            else // this is Modify
            {
                this.Text = "Edit Supplier";
                txtSupplierId.ReadOnly = true;   // can't alter existing packageId
                this.DisplaySuppliers();
            }
        }

        private void DisplaySuppliers()
        {
            txtSupplierId.Text = Supplier.SupplierId.ToString();
            txtSupplierName.Text = Supplier.SupName;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtSupplierName, "Supplier Name")&&
                Validator.IsPresent(txtSupplierId, "SupplierID")&&
                Validator.IsNonNegativeInt(txtSupplierId, "SupplierId")&&
                Validator.IsNrChar50(txtSupplierName, "Supplier Name") &&
                Validator.IsNrChar50(txtSupplierId, "SupplierId"))
            {
                try
                {
                    if (AddSupplier) // this is Add
                    {
                        // initialize the Product property with new Products object
                        this.Supplier = new Suppliers();
                    }
                    this.LoadSupplierData(); // we have an object (public property Product) with new data
                    this.DialogResult = DialogResult.OK;
                }

                catch (DbUpdateException ex)
                {
                    HandleDatabaseError(ex);
                }
                catch (Exception ex)
                {
                    HandleGeneralError(ex);
                }
            }
        }

        private void HandleGeneralError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString());
        }

        private void HandleDatabaseError(DbUpdateException ex)
        {
            string errorMessage = "";
            var sqlException = (SqlException)ex.InnerException;
            foreach (SqlError error in sqlException.Errors)
            {
                errorMessage += "ERROR CODE:  " + error.Number + " " +
                                error.Message + "\n";
            }
            MessageBox.Show(errorMessage);
        }

        private void LoadSupplierData()
        {
            Supplier.SupName = txtSupplierName.Text;
            Supplier.SupplierId = Convert.ToInt32(txtSupplierId.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSupplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                //validating the only keys that the user can be able
                //to input into the Supplier Name text field



                if (!char.IsLetter(e.KeyChar) &&
                    e.KeyChar != '-' &&
                    e.KeyChar != '\'' &&
                    e.KeyChar != ' ' &&
                    e.KeyChar != (char)Keys.Back
                    )
                {
                    e.Handled = true;
                }
            }
        }
    }
}
