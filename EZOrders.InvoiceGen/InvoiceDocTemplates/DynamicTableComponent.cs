using AdminPanel.Shared;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZOrders.InvoiceGen.InvoiceDocTemplates
{
    public class DynamicTableComponent : IComponent
    {
        public Dictionary<string, string> Rows { get; }
        public DynamicTableComponent(Dictionary<string, string> rows)
        {
            Rows = rows;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(column =>
                {
                    column.RelativeColumn();
                    column.RelativeColumn();
                });

                uint idx = 1;
                foreach (var item in Rows)
                {
                    table.Cell().Row(idx).Column(1).Text(item.Key).SemiBold();
                    table.Cell().Row(idx).Column(2).Element(CellBlock).Text(item.Value);
                    idx++;
                }
            });
        }

        static IContainer CellBlock(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten2)
                .PaddingBottom(1)
                .BorderBottom(1).BorderColor(Colors.White)
                .AlignRight();
        }
    }
}
