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
    public partial class frmAddEditPRODUCT : Form
    {
        // These public properties are set by the main form
        public Products Product { get; set; } // selected product on the main form
        public bool AddProduct { get; set; } // flag that distinguishes Add from Modify
        public frmAddEditPRODUCT()
        {
            InitializeComponent();
        }

        private void frmAddEditPRODUCT_Load(object sender, EventArgs e)
        {
            if (AddProduct) // this is Add
            {
                this.Text = "Add new Product";
                txtProductId.ReadOnly = true;  // can't alter existing productId
            }
            else // this is Modify
            {
                this.Text = "Modify Product";
                txtProductId.ReadOnly = true;   // can't alter existing packageId
                this.DisplayProducts();
            }
        }

        private void DisplayProducts()
        {
            txtProductId.Text = Product.ProductId.ToString();
            txtProductName.Text = Product.ProdName;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtProductName, "Product Name"))
            {
                try
                {
                    if (AddProduct) // this is Add
                    {
                        // initialize the Product property with new Products object
                        this.Product = new Products();
                    }
                    this.LoadProductData(); // we have an object (public property Product) with new data
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

        private void LoadProductData()
        {
            Product.ProdName = txtProductName.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
