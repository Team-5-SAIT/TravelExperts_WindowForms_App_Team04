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
    public partial class frmAddEditpackages : Form
    {

        // These public properties are set by the main form
        public Packages Package { get; set; } // selected product on the main form
        public bool AddPackage { get; set; } // flag that distinguishes Add from Modify
        public frmAddEditpackages()
        {
            InitializeComponent();
        }

        private void frmAddEditpackages_Load(object sender, EventArgs e)
        {
            if (AddPackage) // this is Add
            {
                this.Text = "Add new Package";
                txtPackageId.ReadOnly = true;  // can't alter existing packageId
            }
            else // this is Modify
            {
                this.Text = "Modify Product";
                txtPackageId.ReadOnly = true;   // can't alter existing packageId
                this.DisplayPackages();
            }
        }

        private void DisplayPackages()
        {
            {
                txtPackageId.Text = Package.PackageId.ToString();
                txtPackageName.Text = Package.PkgName;
                txtStartDate.Text = Package.PkgStartDate.ToString();
                txtEndDate.Text = Package.PkgEndDate.ToString();
                txtDescription.Text = Package.PkgDesc;
                txtBasePrice.Text = Package.PkgBasePrice.ToString();
                txtAgentCommission.Text = Package.PkgAgencyCommission.ToString();

            }
        }

        // user clicks on the "Accept" button
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtPackageName, "Package Name") &&
               Validator.IsPresent(txtStartDate, "Package Start Date") &&
               Validator.IsPresent(txtEndDate, "Package End Date") &&
               Validator.IsPresent(txtDescription, "Package Description") &&
               Validator.IsPresent(txtBasePrice, "Package base price") &&
               Validator.IsPresent(txtAgentCommission, "Agents' Commission") &&
               Validator.IsNrChar50(txtDescription, "Package Description") && //no more than 50 characters.
               Validator.IsNrChar50(txtPackageName, "Package Name") && 
               Validator.IsNonNegativeDecimal(txtBasePrice, "Package Base price") &&
               Validator.IsNonNegativeDecimal(txtAgentCommission, "Agents' Commission") &&
               Validator.IsnonLaterDateTime(txtStartDate, txtEndDate, "Package Start Date", "Package End Date") &&  //Validates if the end date is later than the start date.
               Validator.IsDateTime(txtStartDate, "Package Start Date") &&
               Validator.IsDateTime(txtEndDate, "Package End Date"))
            {
                try
                {
                    //Business rule validation. Agent commission should or must be less than the base price.
                    //bool isValid = true;
                    decimal basePrice;
                    decimal agentCommission;
                    basePrice = Convert.ToDecimal(txtBasePrice.Text);
                    agentCommission = Convert.ToDecimal(txtAgentCommission.Text);
                    if(agentCommission >= basePrice)
                    {
                        //isValid = false;
                        MessageBox.Show("Agent Commission cannot be greater or equal to the base price!");
                        txtAgentCommission.SelectAll();
                        txtAgentCommission.Focus();
                        
                    }

                    else  // this is Add (AddPackage)
                    {
                        // initialize the Product property with new Products object
                        this.Package = new Packages();
                        this.LoadPackageData(); // we have an object (public property Product) with new data
                        this.DialogResult = DialogResult.OK;
                    }
                    
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

        private void LoadPackageData()
        {
            Package.PkgName = txtPackageName.Text;
            Package.PkgStartDate = Convert.ToDateTime(txtStartDate.Text);
            Package.PkgEndDate = Convert.ToDateTime(txtEndDate.Text);
            Package.PkgDesc = txtDescription.Text;
            Package.PkgBasePrice = Convert.ToDecimal(txtBasePrice.Text);
            Package.PkgAgencyCommission = Convert.ToDecimal(txtAgentCommission.Text);


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPackageName_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                //validating the only keys that the user can be able
                //to input into the package name text field



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

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                //validating the only keys that the user can be able
                //to input into the package description text field



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
