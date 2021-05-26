using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelExpertsAppGUI
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void btnPackages_Click(object sender, EventArgs e)
        {
            frmPackages f2 = new frmPackages();
            f2.ShowDialog();
        }
//===============================================================================================
        private void btnProducts_Click_1(object sender, EventArgs e)
        {
            panelLeft.Height = btnProducts.Height;
            panelLeft.Top = btnProducts.Top;
            frmProduct f3 = new frmProduct();
            f3.ShowDialog();
           
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnSuppliers.Height;
            panelLeft.Top = btnSuppliers.Top;
            frmSupplier f4 = new frmSupplier();
            f4.ShowDialog();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelLeft.Height = button1.Height;
            panelLeft.Top = button1.Top;

            frmProductPackages f5 = new frmProductPackages();
            f5.ShowDialog();
            
        }

        private void btnProductSuppliers_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnProductSuppliers.Height;
            panelLeft.Top = btnProductSuppliers.Top;
            frmProductSupplier f6 = new frmProductSupplier();
            f6.ShowDialog();
           
        }

       

        private void frmHome_Load(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelLeft.Height = button2.Height;
            panelLeft.Top = button2.Top;
        }
    }



}
