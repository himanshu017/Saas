using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EZOrders.InvoiceGen.InvoiceDocTemplates
{
    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private string[] Address { get; }

        public AddressComponent(string title, string[] address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                if (!string.IsNullOrEmpty(Title))
                {
                    column.Item().Text(Title).SemiBold();
                    column.Item().PaddingBottom(5).LineHorizontal(1);
                }

                foreach (var item in Address)
                {
                    column.Item().Text(item);
                }
            });
        }
    }


    public static class SimpleExtension
    {
        private static IContainer Cell(this IContainer container, bool dark, uint border = 1)
        {
            return container
                .Border(border)
                .Background(dark ? Colors.Grey.Lighten3 : Colors.White)
                .PaddingLeft(5);
        }

        // displays only text label
        public static void LabelCell(this IContainer container, string text) => container.Cell(true).AlignMiddle().Text(text).SemiBold();

        // allows to inject any type of content, e.g. image
        public static IContainer ValueCell(this IContainer container, uint border = 1) => (IContainer)container.Cell(false, border);
    }
}
