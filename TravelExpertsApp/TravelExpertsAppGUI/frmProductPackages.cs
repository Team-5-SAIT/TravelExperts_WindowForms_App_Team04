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
    public partial class frmProductPackages : Form
    {
        public frmProductPackages()
        {
            InitializeComponent();
        }

        private TravelExpertsContext context = new TravelExpertsContext();
        private PackagesProductsSuppliers selectedPackageProduct;// the current product

        private void frmProductPackages_Load(object sender, EventArgs e)
        {
            DisplayPackageProducts();
        }

        private void DisplayPackageProducts()
        {
            //Table is a "Linking" table. related to other tables based on a "Many-to-Many" relationship.
            dgvProductPackage.Columns.Clear(); // clears old content
            var productpackages = context.PackagesProductsSuppliers // retrieve data
                .OrderBy(p => p.PackageId) // 
                .Select(p => new { p.PackageId, p.ProductSupplierId }) // only two columns
                .ToList();

            dgvProductPackage.DataSource = productpackages;

            // format the column header
            dgvProductPackage.EnableHeadersVisualStyles = false;
            dgvProductPackage.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvProductPackage.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal; // golden background on headers
            dgvProductPackage.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // text on headers

            // format the odd numbered rows
            dgvProductPackage.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
