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
    public partial class frmProductSupplier : Form
    {
        public frmProductSupplier()
        {
            InitializeComponent();
        }
        private TravelExpertsContext context = new TravelExpertsContext();
        private ProductsSuppliers selectedProductSupplier;// the current product supplier

        private void frmProductSupplier_Load(object sender, EventArgs e)
        {
            DisplayProductsSupplier();
        }

        private void DisplayProductsSupplier()
        {
            //Table is a "Linking" table. related to other tables based on a "Many-to-Many" relationship.
            dgvProductSupplier.Columns.Clear(); // clears old content
            var productsupplier = context.ProductsSuppliers // retrieve data
                .OrderBy(p => p.ProductSupplierId) // 
                .Select(p => new { p.ProductSupplierId, p.ProductId, p.SupplierId }) // only three columns
                .ToList();

            dgvProductSupplier.DataSource = productsupplier;

            // format the column header
            dgvProductSupplier.EnableHeadersVisualStyles = false;
            dgvProductSupplier.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvProductSupplier.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal; // Blue background on headers
            dgvProductSupplier.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // text on headers

            // format the odd numbered rows
            dgvProductSupplier.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
