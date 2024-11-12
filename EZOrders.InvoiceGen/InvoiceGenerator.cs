using AdminPanel.Shared;
using AdminPanel.Shared.BO.WebApp;
using EZOrders.InvoiceGen.InvoiceDocTemplates;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZOrders.InvoiceGen
{
    public static class InvoiceGenerator
    {
        public static byte[] GeneratePDFDefault(InvoiceGenBO model)
        {
            var invoice = new InvoiceTemplateDefault(model);

            return invoice.GeneratePdf();
        }

        public static byte[] GeneratePDFTwo(InvoiceGenBO model)
        {
            var invoice = new InvoiceTemplateTwo(model);

            return invoice.GeneratePdf();
        }

        public static byte[] GeneratePDFThree(InvoiceGenBO model)
        {
            var invoice = new InvoiceTemplateThree(model);

            return invoice.GeneratePdf();
        }
    }
}
