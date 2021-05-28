using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TravelExpertsData;


namespace TravelExpertsAppGUI
{
    public partial class frmPackages : Form
    {
        public frmPackages()
        {
            InitializeComponent();
        }

        private TravelExpertsContext context = new TravelExpertsContext();
        private Packages selectedPackage;// the current package

        private void frmPackages_Load(object sender, EventArgs e)
        {
            DisplayPackages();
        }

        private void DisplayPackages()
        {
            dgvPackages.Columns.Clear(); // clears old content
            var packages = context.Packages // retrieve products data
                .OrderBy(p => p.PackageId) // ordered numerically by the primary key
                .Select(p => new { p.PackageId, p.PkgName, p.PkgStartDate, p.PkgEndDate, p.PkgDesc, p.PkgBasePrice, p.PkgAgencyCommission }) // only seven columns
                .ToList();

            dgvPackages.DataSource = packages;

            // add column for modify button
            var modifyColumn = new DataGridViewButtonColumn()
            { // object initializer
                UseColumnTextForButtonValue = true,
                HeaderText = "", // header on the top
                Text = "Edit"
            };


            dgvPackages.Columns.Add(modifyColumn);// add new column to the grid view

            // add column for delete/Remove  button
            var deleteColumn = new DataGridViewButtonColumn()
            {
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                Text = "Remove",
                
               
                
            };
            
            dgvPackages.Columns.Add(deleteColumn);

            // format the column header
            dgvPackages.EnableHeadersVisualStyles = false;
            dgvPackages.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvPackages.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal; // golden background on headers
            dgvPackages.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // text on headers
           
            

            // format the odd numbered rows
            dgvPackages.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;


            dgvPackages.Columns[0].HeaderText = "PKg ID";
            dgvPackages.Columns[1].HeaderText = "PKg Name";
            dgvPackages.Columns[2].HeaderText = "PKg Start Date";
            dgvPackages.Columns[3].HeaderText = "PKg End Date";
            dgvPackages.Columns[4].HeaderText = "PKg Description";
            dgvPackages.Columns[5].HeaderText = "PKg BasePrice $";
            dgvPackages.Columns[6].HeaderText = "PKg AgencyCommission $";
            dgvPackages.Columns[5].DefaultCellStyle.Format = "c2";
            dgvPackages.Columns[6].DefaultCellStyle.Format = "c2";

            // format the second column
            dgvPackages.Columns[1].Width = 321;

            //// format the third and fourth column
            //dgvPackages.Columns[2].Width = 290;
            //dgvPackages.Columns[3].Width = 290;

            //// format the fifth column
            dgvPackages.Columns[4].Width = 310;

            //// format the second column
            //dgvPackages.Columns[1].Width = 280;

            //// format the sixth and seventh column
            //dgvPackages.Columns[5].Width = 70;
            //dgvPackages.Columns[6].Width = 70;

            // format the third column

            //dgvPackages.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvPackages.Columns[2].Width = 75;


            dgvPackages.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPackages.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // user clicked on a cell - is it one of the buttons

        private void DeletePackage()
        {
            DialogResult result =
               MessageBox.Show($"Delete {selectedPackage.PackageId}?",
               "Confirm Delete", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)// user confirmed
            {
                try
                {
                    context.Packages.Remove(selectedPackage);
                    context.SaveChanges(true);
                    DisplayPackages();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    HandleConcurrencyError(ex);
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

        private void HandleConcurrencyError(DbUpdateConcurrencyException ex)
        {
            ex.Entries.Single().Reload();

            var state = context.Entry(selectedPackage).State;
            if (state == EntityState.Detached)
            {
                MessageBox.Show("Another user has deleted that product.",
                    "Concurrency Error");
            }
            else
            {
                string message = "Another user has updated that product.\n" +
                    "The current database values will be displayed.";
                MessageBox.Show(message, "Concurrency Error");
            }
            this.DisplayPackages();
        }

        private void dgvPackages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // store index values for Modify and Delete button columns
            const int ModifyIndex = 7; // Modify buttons are column 7
            const int DeleteIndex = 8; // Delete buttons are column 8

            if (e.ColumnIndex == ModifyIndex || e.ColumnIndex == DeleteIndex) // is it a button?
            {
                // get the package ID: 
                // grid view has properties (collection) Rows and Columns
                // package ID is cell number 0 in the current row
               
                int packageId = Convert.ToInt32(dgvPackages.Rows[e.RowIndex].Cells[0].Value);
                selectedPackage = context.Packages.Find(packageId);// find by PK value
            }

            if (e.ColumnIndex == ModifyIndex) // user clicked on Modify
            {
                ModifyPackages();
            }
            else if (e.ColumnIndex == DeleteIndex) // user clicked on Delete
            {
                DeletePackage();
            }
        }

        private void ModifyPackages()
        {
            var addEditPackageForm = new frmAddEditpackages()
            { // object initializer
                AddPackage = false,
                Package = selectedPackage
                
        };
            DialogResult result = addEditPackageForm.ShowDialog();// display modal
            if (result == DialogResult.OK)// user clicked Accept on the second form
            {
                try
                {
                    selectedPackage = addEditPackageForm.Package; // new data 
                    context.SaveChanges();
                    DisplayPackages();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    HandleConcurrencyError(ex);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            var addEditPackageForm = new frmAddEditpackages()
            {
                AddPackage = true
            };
            DialogResult result = addEditPackageForm.ShowDialog();
            if (result == DialogResult.OK)// user clicked on Accept on the second form
            {
                try
                {
                    selectedPackage = addEditPackageForm.Package;// record product from the second
                                                                   // form as the current product
                    context.Packages.Add(selectedPackage);
                    context.SaveChanges();
                    DisplayPackages();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }//Form level
}//NameSpace
