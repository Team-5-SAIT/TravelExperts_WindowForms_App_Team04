using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// /Validator class was done by "DZIANIS TSISHCHENKA and OGHENEGARE UTI"
/// Entity framework/database was installed/scaffolded by "OGHENEGARE UTI </summary>


namespace TravelExpertsAppGUI
{
    /// <summary>
    /// a repository of validation methods
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// validates if textbox has something in it
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsPresent(TextBox tb, string name)
        {
            bool isValid = true; // "innocent until proven guilty"
            if (tb.Text == "") // bad
            {
                isValid = false;
                MessageBox.Show(name + " is required", "Input error");
                tb.Focus();
            }        
            return isValid;
        }


        /// <summary>
        /// validates if textbox input is a DateTime type/format
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsDateTime(TextBox tb, string name)
        {
            bool isValid = true;
            DateTime value;
            if (!DateTime.TryParse(tb.Text, out value)) //not a date/time format
            {
                isValid = false;
                MessageBox.Show(name + " has to be a date/time format: YYYY-MM-DD HH:MM:SS AM/PM - ", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            return isValid;
        }

        /// <summary>
        /// validates if textbox contains non-negative int
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsNonNegativeInt(TextBox tb, string name)
        {
            bool isValid = true; // "innocent until proven guilty"
            int value;
            if (!Int32.TryParse(tb.Text, out value)) //not an int
            {
                isValid = false;
                MessageBox.Show(name + " has to be whole number", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            else if(value < 0)// cannot be negative
            {
                isValid = false;
                MessageBox.Show(name + " has to positive or zero", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            return isValid;
        }


        /// <summary>
        /// checks if combo box has item selected.
        /// </summary>
        /// <param name="cbo">The combo box to check</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if selected and false if not</returns>
        public static bool IsSelected(ComboBox cbo, string name)
        {
            bool isValid = true;
            if (cbo.SelectedIndex == -1) // not selected
            {
                isValid = false;
                MessageBox.Show(name + " has to be selected", "Input error");
                cbo.Focus();
            }
            return isValid;
        }


        /// <summary>
        /// validates if textbox contains non-negative double
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsNonNegativeDouble(TextBox tb, string name)
        {
            bool isValid = true; // "innocent until proven guilty"
            double value;
            if (!Double.TryParse(tb.Text, out value)) //not a double
            {
                isValid = false;
                MessageBox.Show(name + " has to be a number", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            else if (value < 0)// cannot be negative
            {
                isValid = false;
                MessageBox.Show(name + " has to positive or zero", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            return isValid;
        }


        /// <summary>
        /// validates if textbox contains non-negative decimal
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsNonNegativeDecimal(TextBox tb, string name)
        {
            bool isValid = true; // "innocent until proven guilty"
            decimal value;
            if (!Decimal.TryParse(tb.Text, out value)) //not an int
            {
                isValid = false;
                MessageBox.Show(name + " has to be a number", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            else if (value < 0)// cannot be negative
            {
                isValid = false;
                MessageBox.Show(name + " has to positive or zero", "Input error");
                tb.SelectAll();
                tb.Focus();
            }
            return isValid;
        }


        /// <summary>
        /// validates if the date in textbox1 is later than the date in textbox2
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsnonLaterDateTime(TextBox tb, TextBox tb1, string name, string name1)
        {
            bool isValid = true; // "innocent until proven guilty"
            if (DateTime.Parse(tb.Text) > DateTime.Parse(tb1.Text))
            {
                isValid = false;
                MessageBox.Show(name1 + " has to be later than " + name, "Input error");
                tb.Focus();
            }
            return isValid;
        }


        /// <summary>
        /// validates if the characters in the textbox exceeds 50
        /// </summary>
        /// <param name="tb">text box to validate</param>
        /// <param name="name">name for error message</param>
        /// <returns>true if valid, and false if not</returns>
        public static bool IsNrChar50(TextBox tb, string name)
        {
            bool isValid = true; // "innocent until proven guilty"
            if (tb.Text.Length > 50) //exceeds 50 char
            {
                isValid = false;
                MessageBox.Show(name + " cannot exceed 50 characters", "Input error");
                tb.Focus();
            }
            return isValid;
        }


    } // class
}// namespace
