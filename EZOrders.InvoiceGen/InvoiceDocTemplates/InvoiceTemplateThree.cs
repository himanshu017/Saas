using AdminPanel.Shared;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZOrders.InvoiceGen.InvoiceDocTemplates
{
    internal class InvoiceTemplateThree : IDocument
    {
        public InvoiceGenBO Model { get; }
        static string BgColor;
        static string currency;
        public InvoiceTemplateThree(InvoiceGenBO model)
        {
            Model = model;
            BgColor = Model.PrimaryColor;
            currency = model.OrderModel.CompanyBO.CurrencyInfo;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var txtStyle = TextStyle.Default.Fallback(x => x.FontFamily("Arial"));
            container
                .Page(page =>
                {
                    page.Margin(20);

                    page.Header().DefaultTextStyle(txtStyle).Element(ComposeHeader);
                    page.Content().DefaultTextStyle(txtStyle).Element(ComposeContent);

                    page.Footer().DefaultTextStyle(txtStyle).AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Download the image from the URL
                HttpClient client = new HttpClient();
                var imageData = client.GetStreamAsync(Model.LogoPath).Result;

                row.RelativeItem().Column(Column =>
                {
                    Column.Item().Text(Model.CompanyName).SemiBold();
                    Column.Item().Text(Model.CompanyEmail);
                    Column.Item().Text(Model.CompanyPhone);
                    //Column.Item().Text("Country, Postcode");
                });

                row.RelativeItem().Column(Column =>
                {
                    Column.Item().AlignCenter().AlignMiddle().Text("INVOICE").FontSize(25).Bold();
                    Column.Item().AlignCenter().AlignMiddle().Text($"{Model.OrderModel.OrderNumber}").SemiBold().FontSize(20);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Height(70).AlignRight().Image(imageData, ImageScaling.FitArea);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(15).Column(column =>
            {
                column.Spacing(15);

                column.Item().Border(1).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(100);
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.RelativeColumn();
                    });

                    table.ExtendLastCellsToTableBottom();

                    table.Cell().RowSpan(3).ColumnSpan(4).ValueCell().AlignCenter().Text("INVOICE DETAILS").Bold();
                    table.Cell().RowSpan(4).ColumnSpan(2).ValueCell().AlignMiddle().Text(Model.OrderModel.CustomerName);

                    foreach (var detailItem in Model.InvoiceInfo)
                    {
                        table.Cell().LabelCell(detailItem.Key);
                        table.Cell().ValueCell().AlignCenter().Text(detailItem.Value);
                    }

                    if (Model.OrderModel.BillingAddress != null)
                    {
                        table.Cell().ColumnSpan(2).LabelCell("Billing Address");

                        table.Cell().ColumnSpan(2).LabelCell("Shipping Address");

                        table.Cell().ColumnSpan(2).ValueCell().Column(col =>
                        {
                            col.Item().Component(new AddressComponent("", Model.OrderModel.DelieryAddress));
                        });

                        table.Cell().ColumnSpan(2).ValueCell().Column(col =>
                        {
                            col.Item().Component(new AddressComponent("", Model.OrderModel.BillingAddress));
                        });
                    }
                    else
                    {
                        table.Cell().ColumnSpan(4).LabelCell("Shipping Address");
                        table.Cell().ColumnSpan(4).ValueCell().Column(col =>
                        {
                            col.Item().Component(new AddressComponent("", Model.OrderModel.DelieryAddress));
                        });
                    }

                    //table.Cell().LabelCell("Order Notes:");
                    //table.Cell().ColumnSpan(3).ValueCell().Text(Placeholders.Sentence());
                });

                column.Item().Element(ComposeTable);
                column.Item().Element(ComposePricingSection);

                column.Item().PaddingTop(25).Column(col =>
                {
                    col.Item().AlignCenter().Text("Thank you for you order!").Bold().FontSize(25);
                    col.Item().AlignCenter().Text("If there is any issue with the order, please reach out to the sales team.").FontSize(11);
                });

                // add dynamic component to add order notes. Need to make changes to the admin section where we add the Invoice notes to add it as a string array of items, with a title.
            });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().BorderLeft(1).Element(CellStyle).AlignCenter().Text("#");
                    header.Cell().Element(CellStyle).PaddingLeft(4).Text("Product").Style(headerStyle);
                    header.Cell().Element(CellStyle).PaddingLeft(4).AlignLeft().Text("Unit").Style(headerStyle);
                    header.Cell().Element(CellStyle).AlignRight().PaddingRight(4).Text("Price").Style(headerStyle);
                    header.Cell().Element(CellStyle).AlignCenter().Text("Qty").Style(headerStyle);
                    header.Cell().BorderRight(1).Element(CellStyle).AlignRight().PaddingRight(4).Text("Total").Style(headerStyle);

                    header.Cell().ColumnSpan(6).PaddingVertical(1).BorderBottom(1).BorderColor(Colors.Black);

                    static IContainer CellStyle(IContainer container) => container.BorderTop(1).Background(BgColor).PaddingVertical(5);
                });

                var idx = 1;
                foreach (var item in Model.OrderModel.OrderItems)
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{idx}");
                    table.Cell().Element(CellStyle).Column(col =>
                    {
                        col.Item().Text(item.ProductName);
                        if (!string.IsNullOrEmpty(item.CommentDesc))
                            col.Item().Text(item.CommentDesc).FontSize(6).FontColor(Colors.Red.Lighten1);
                    });
                    table.Cell().Element(CellStyle).AlignLeft().Text(item.UnitName);
                    table.Cell().Element(CellStyle).AlignRight().Text((item.Price + (item.AttributePriceAdjustment ?? 0)).ToPrice());
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.Quantity}");
                    table.Cell().Element(CellStyle).AlignRight().Text(((item.Price + (item.AttributePriceAdjustment ?? 0)) * (decimal)item.Quantity).ToPrice());

                    static IContainer CellStyle(IContainer container) => container.BorderVertical(1).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
                    
                    idx++;
                }
            });
        }

        void ComposePricingSection(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().ShowEntire().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                        });

                        table.Cell().LabelCell("Order Notes:");
                        table.Cell().ValueCell().Text($"{Model.OrderModel.OrderNote}");

                        // add check to see if there are terms and conditions available
                        table.Cell().LabelCell("Terms & Conditions:");
                        table.Cell().ValueCell().Text($"{Placeholders.LoremIpsum()}");
                    });


                });

                row.ConstantItem(20);

                row.ConstantItem(215).Component(new DynamicTableComponent(Model.AllCharges));

            });
        }
    }
}
