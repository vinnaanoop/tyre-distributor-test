using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TyreDistributor
{
    public class Order
    {
        public string Company { get; }
        /// <summary>
        /// Set this contant in the app config to avoid code changing for any change in VAT
        /// </summary>
        private const double TaxRate = .0725d;
        private readonly IList<Line> _lines = new List<Line>();
        private double totalAmount = 0d;
        private double tax = 0d;
        StringBuilder result = null;
        public Order(string company)
        {
            Company = company;
        }

        public void AddLine(Line line)
        {
            _lines.Add(line);
        }
        /// <summary>
        /// Receipt  Formating method
        /// </summary>
        /// <returns></returns>
        public string Receipt()
        {
            totalAmount = 0d;
            tax = 0d;
            result = new StringBuilder(string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine));
            totalAmount = AmountCalculator(_lines, result);
            result.AppendLine(string.Format("Sub-Total: {0}", totalAmount.ToString("C")));
            tax = totalAmount * TaxRate;
            result.AppendLine(string.Format("Tax: {0}", tax.ToString("C")));
            result.Append(string.Format("Total: {0}", (totalAmount + tax).ToString("C")));
            return result.ToString();
        }
        /// <summary>
        /// Mentod for calculating the Total amount
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private double AmountCalculator(IList<Line> _lines, StringBuilder result)
        {
            foreach (var line in _lines)
            {
                double thisAmount = 0d;
                switch (line.Tyre.Model)
                {
                    case Tyre.Suv:
                        thisAmount += ThisAmount(Tyre.Suv, line, .9d, thisAmount);
                        break;
                    case Tyre.Mini:
                        thisAmount += ThisAmount(Tyre.Mini, line, .8d, thisAmount);
                        break;
                    case Tyre.Estate:
                        thisAmount += ThisAmount(Tyre.Estate, line, .8d, thisAmount);
                        break;
                }
                result.AppendLine(string.Format("{0} x {1} {2} = {3}", line.Quantity, line.Tyre.Brand, line.Tyre.Model, thisAmount.ToString("C")));
                totalAmount += thisAmount;
            }

            return totalAmount;
        }
        /// <summary>
        /// Line amount calculation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_line"></param>
        /// <param name="multVal"></param>
        /// <param name="thisAmount"></param>
        /// <returns></returns>
        private double ThisAmount(string type, Line _line, double multVal, double thisAmount)
        {
            if (_line.Quantity >= 20 && type == Tyre.Suv)
                thisAmount = _line.Quantity * _line.Tyre.Price * multVal;
            else if (_line.Quantity >= 10 && type == Tyre.Mini)
                thisAmount = _line.Quantity * _line.Tyre.Price * multVal;
            else if (_line.Quantity >= 5 && type == Tyre.Estate)
                thisAmount = _line.Quantity * _line.Tyre.Price * multVal;
            else
                thisAmount = _line.Quantity * _line.Tyre.Price;
            return thisAmount;
        }
        /// <summary>
        /// HTML Receipt formating Method
        /// </summary>
        /// <returns></returns>
        public string HtmlReceipt()
        {
            result = new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", Company));
            if (_lines.Any())
            {
                result.Append("<ul><li>");
                totalAmount = AmountCalculator(_lines, result);
                result.Append("</li></ul>");
            }
            result.Append(string.Format("<h3>Sub-Total: {0}</h3>", totalAmount.ToString("C")));
            tax = totalAmount * TaxRate;
            result.Append(string.Format("<h3>Tax: {0}</h3>", tax.ToString("C")));
            result.Append(string.Format("<h2>Total: {0}</h2>", (totalAmount + tax).ToString("C")));
            result.Append("</body></html>");
            return result.ToString();
        }
    }
}