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
    public partial class frmSupplier : Form
    {
        public frmSupplier()
        {
            InitializeComponent();
        }

        private TravelExpertsContext context = new TravelExpertsContext();
        private Suppliers selectedSupplier;// the current supplier

        private void frmSupplier_Load(object sender, EventArgs e)
        {
            DisplaySuppliers();
        }

        private void DisplaySuppliers()
        {
            dgvSuppliers.Columns.Clear(); // clears old content
            var suppliers = context.Suppliers // retrieve suppliers data
                .OrderBy(s => s.SupplierId) // ordered numerically by the primary key
                .Select(s => new { s.SupplierId, s.SupName }) // only two columns
                .ToList();

            dgvSuppliers.DataSource = suppliers;

            // add column for modify button
            var modifyColumn = new DataGridViewButtonColumn()
            { // object initializer
                UseColumnTextForButtonValue = true,
                HeaderText = "", // header on the top
                Text = "Edit"
            };


            dgvSuppliers.Columns.Add(modifyColumn);// add new column to the grid view

            // add column for delete/Remove  button
            var deleteColumn = new DataGridViewButtonColumn()
            {
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                Text = "Remove"
            };
            dgvSuppliers.Columns.Add(deleteColumn);

            // format the column header
            dgvSuppliers.EnableHeadersVisualStyles = false;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal; // golden background on headers
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // text on headers

            // format the odd numbered rows
            dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

            //Format Edit and Delete/Remove cell buttons.
            dgvSuppliers.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvSuppliers.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void DeleteSupplier()
        {
            DialogResult result =
               MessageBox.Show($"Delete {selectedSupplier.SupplierId}?",
               "Confirm Delete", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)// user confirmed
            {
                try
                {
                    context.Suppliers.Remove(selectedSupplier);
                    context.SaveChanges(true);
                    DisplaySuppliers();
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

            var state = context.Entry(selectedSupplier).State;
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
            this.DisplaySuppliers();
        }

        private void dgvSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // store index values for Modify and Delete button columns
            const int ModifyIndex = 2; // Modify buttons are column 5
            const int DeleteIndex = 3; // Delete buttons are column 6

            if (e.ColumnIndex == ModifyIndex || e.ColumnIndex == DeleteIndex) // is it a button?
            {
                // get the Supplier ID: 
                // grid view has properties (collection) Rows and Columns
                // product code is cell number 0 in the current row
                int supplierId = Convert.ToInt32(dgvSuppliers.Rows[e.RowIndex].Cells[0].Value);
                selectedSupplier = context.Suppliers.Find(supplierId);// find by PK value
            }

            if (e.ColumnIndex == ModifyIndex) // user clicked on Modify
            {
                ModifySuppliers();
            }
            else if (e.ColumnIndex == DeleteIndex) // user clicked on Delete
            {
                DeleteSupplier();
            }
        }

        private void ModifySuppliers()
        {
            var addEditSupplierForm = new frmAddEditSUPPLIERS()
            { // object initializer
                AddSupplier = false,
                Supplier = selectedSupplier
            };
            DialogResult result = addEditSupplierForm.ShowDialog();// display modal
            if (result == DialogResult.OK)// user clicked Accept on the second form
            {
                try
                {
                    selectedSupplier = addEditSupplierForm.Supplier; // new data
                    context.SaveChanges();
                    DisplaySuppliers();
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
            var addEditSupplierForm = new frmAddEditSUPPLIERS()
            {

                AddSupplier = true
            };
            DialogResult result = addEditSupplierForm.ShowDialog();
            if (result == DialogResult.OK)// user clicked on Accept on the second form
            {
                try
                {
                    selectedSupplier = addEditSupplierForm.Supplier;// record product from the second
                                                                 // form as the current product
                    context.Suppliers.Add(selectedSupplier);
                    context.SaveChanges();
                    DisplaySuppliers();
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
    }
}
