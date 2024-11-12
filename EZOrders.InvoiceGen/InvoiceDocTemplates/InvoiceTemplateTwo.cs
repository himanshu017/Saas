using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
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
    public class InvoiceTemplateTwo : IDocument
    {
        public InvoiceGenBO Model { get; }
        static string BgColor;
        static string currency;
        public InvoiceTemplateTwo(InvoiceGenBO model)
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
            container.PaddingBottom(2).Row(row =>
            {
                // Download the image from the URL
                HttpClient client = new HttpClient();
                var imageData = client.GetStreamAsync(Model.LogoPath).Result;

                row.RelativeItem().Column(column =>
                {
                    column.Item().Height(65).Image(imageData, ImageScaling.FitArea);

                    column.Item().ShowOnce().Row(row =>
                    {
                        row.RelativeItem().PaddingTop(5).Column(clm =>
                        {
                            clm.Spacing(2);
                            clm.Item().Text(Model.OrderModel.CompanyBO.CompanyName).SemiBold();
                            clm.Item().Text("Street Address");
                            clm.Item().Text("City, State");
                            clm.Item().Text("Zipcode");
                        });

                        row.RelativeItem().AlignRight().PaddingTop(5).Column(clm =>
                        {
                            clm.Item().Hyperlink($"mailto:{Model.OrderModel.CompanyBO.Email}").Text(Model.OrderModel.CompanyBO.Email).FontSize(10).Underline().FontColor(Colors.Blue.Accent3);
                            clm.Item().Hyperlink($"tel:{Model.OrderModel.CompanyBO.Phone}").Text(Model.OrderModel.CompanyBO.Phone).FontSize(10).FontColor(Colors.Blue.Accent3);
                            //clm.Item().Hyperlink("https://web.ezorders.us").Text("web.ezorders.us").FontSize(10).FontColor(Colors.Blue.Accent3);
                        });
                    });
                });
                row.ConstantItem(90);
                row.ConstantItem(215).AlignRight().Column(Column =>
                {
                    Column.Item().AlignRight().AlignRight().Text($"INVOICE # {Model.OrderModel.OrderNumber}")
                        .FontSize(25).SemiBold().FontColor(Colors.Blue.Medium);

                    Column.Item().ShowOnce().PaddingTop(30).Component(new DynamicTableComponent(Model.InvoiceInfo));

                });

            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(15).Column(column =>
            {
                column.Spacing(20);

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Component(new AddressComponent("Deliver To:", Model.OrderModel.DelieryAddress));
                    row.ConstantItem(30);

                    if (Model.OrderModel.BillingAddress != null)
                        row.RelativeItem().AlignRight().Component(new AddressComponent("Bill To:", Model.OrderModel.BillingAddress));
                });

                column.Item().Element(ComposeTable);

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Table(tbl =>
                    {
                        tbl.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                        });

                        tbl.Cell().Row(1).Column(1).Background(Model.OrderModel.CompanyBO.PrimaryColor).Padding(2).Text("Delivery Notes:").SemiBold();
                        tbl.Cell().Row(2).Column(1).Padding(2)
                                  .Text(Model.OrderModel.OrderNote);
                    });
                    row.ConstantItem(25);
                    row.ConstantItem(220).Component(new DynamicTableComponent(Model.AllCharges));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Column(col =>
                    {
                        col.Item().Text("Thank you for your business!").SemiBold().FontSize(20);
                        col.Item().Text("Should you have any inquiries concerning this invoice, please contact us.").FontSize(10);
                    });
                });
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

                int idx = 1;
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
