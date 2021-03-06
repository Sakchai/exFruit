﻿using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;

using System.Threading.Tasks;
using MissionControl.Shared;
using MissionControl.Shared.Models.Common;
using Microsoft.Extensions.Configuration;

namespace MissionControl.Services.Common
{
    public partial class PdfService : IPdfService
    {
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IRepository<PurchaseItem> _purchaseItemRepository;
        private readonly IConfiguration _config;
        private string ExportPath { get; set; }
        private string ExportURL { get; set; }
        public PdfService(IRepository<Purchase> purchaseRepository,
            IRepository<PurchaseItem> purchaseItemRepository,
            IConfiguration config)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
            _config = config;
            ExportPath = _config.GetValue<string>("ExportPath", string.Empty);
            ExportURL = _config.GetValue<string>("ExportURL", string.Empty);
        }

        public void PrintBarcodeToPdf(Stream stream,int purchaseId)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var purchase = _purchaseRepository.GetById(purchaseId);
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));
          //  string guid = string.Format("{0,10:D6}", purchase.Id).Trim();
          //  var fileName = $"barcode_{guid}_{GenerateRandomDigitCode(2)}.pdf";
            //var filePath = Combine(MapPath(ExportPath), fileName);
            //var filePath = string.Format(@"C:\source\blazor\blazor-TropiThai\src\BlazingPizza.Client\wwwroot\files\exportimport\{0}", fileName);
            //var filePath = string.Format(ExportPath, fileName);
            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
                var purchases = new List<Purchase> { purchase };
                PrintPurchasesToPdf(stream, purchases);
            //}

            //return fileName;
        }

        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('/');

            //if virtual path has slash on the end, it should be after transform the virtual path to physical path too
            var pathEnd = path.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

            return Combine(string.Empty, path) + pathEnd;
        }

        public virtual string Combine(params string[] paths)
        {
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
                //add leading slash to correctly form path in the UNIX system
                path = "/" + path;

            return path;
        }

        protected static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Get font
        /// </summary>
        /// <param name="fontFileName">Font file name</param>
        /// <returns>Font</returns>
        protected virtual Font GetFont(string fontFileName)
        {
            if (fontFileName == null)
                throw new ArgumentNullException(nameof(fontFileName));

            var fontPath = Combine(MapPath("~/App_Data/Pdf/"), fontFileName);
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont, 10, Font.NORMAL);
            return font;
        }

        public virtual void PrintPurchasesToPdf(Stream stream, IList<Purchase> purchases)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (purchases == null)
                throw new ArgumentNullException(nameof(purchases));

            var pageSize = PageSize.A4;


            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);


            doc.Open();

            //fonts
           //var font = GetFont();
            var purCount = purchases.Count;
            var purNum = 0;

            foreach (var purchase in purchases)
            {

                // bar code
                PrintBarcodes(pdfWriter, doc, purchase);

                purNum++;
                if (purNum < purCount)
                {
                    doc.NewPage();
                }
            }

            doc.Close();
        }

        /// <summary>
        /// Get font
        /// </summary>
        /// <returns>Font</returns>
        protected virtual Font GetFont()
        {
            //nopCommerce supports Unicode characters
            //nopCommerce uses Free Serif font by default (~/App_Data/Pdf/FreeSerif.ttf file)
            //It was downloaded from http://savannah.gnu.org/projects/freefont
            return GetFont("EAN13.ttf");
        }

        protected void PrintBarcodes(PdfWriter pdfWriter, Document doc, Purchase purchase)
        {
            PdfPTable table = new PdfPTable(1);
            var  query = _purchaseItemRepository.Table;
            var purchaseItems = query.Where(x => x.PurchaseId == purchase.Id).ToList();
            foreach (var purchaseItem in purchaseItems)
            {
                iTextSharp.text.pdf.PdfContentByte cb = pdfWriter.DirectContent;
                iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                bc.TextAlignment = Element.ALIGN_LEFT;
                bc.Code = purchaseItem.EAN; // CommonUtils.GenerateBarCodeEAN13(purchaseItem.Id); 
                bc.StartStopText = false;
                bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                bc.Extended = true;

                iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb,
                  iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);

                cb.SetTextMatrix(1.5f, 3.0f);
                img.ScaleToFit(60, 5);
                img.SetAbsolutePosition(1.5f, 1);
                cb.AddImage(img);
                table.AddCell(img);

            }
            doc.Add(table);


        }
        protected void PrintBarcodes_(PdfWriter pdfWriter, Document doc, Purchase purchase, Font font)
        {

            var purchaseItems = purchase.PurchaseItems;

            var count = 4;

            var productsTable = new PdfPTable(count)
            {
                RunDirection = GetDirection(),
                WidthPercentage = 100f
            };

            var widths = new Dictionary<int, int[]>
            {
                { 4, new[] { 50, 20, 10, 20 } },
                { 5, new[] { 45, 15, 15, 10, 15 } },
                { 6, new[] { 40, 13, 13, 12, 10, 12 } }
            };

            productsTable.SetWidths(widths[count]);


            foreach (var purchaseItem in purchaseItems)
            {
                var pAttribTable = new PdfPTable(1) { RunDirection = GetDirection() };
                pAttribTable.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfContentByte cb = pdfWriter.DirectContent;
                //string code = purchaseItem.EAN;  //CommonUtils.GenerateBarCodeEAN13(purchaseItem.Id);
                var bc = new BarcodeEan
                {
                    Code = purchaseItem.EAN,
                    TextAlignment = Element.ALIGN_CENTER,
                    StartStopText = true,
                    CodeType = Barcode.EAN13,
                    Extended = false
                };

                Image img = bc.CreateImageWithBarcode(cb, BaseColor.Black, BaseColor.Black);

                var cell = new PdfPCell(img);
                productsTable.AddCell(cell);


            }

            doc.Add(productsTable);
        }



        protected virtual int GetDirection()
        {
            return PdfWriter.RUN_DIRECTION_LTR;
        }
        protected virtual PdfPCell GetPdfCell(string value, Font font)
        {
            return new PdfPCell(new Phrase(value, font));
        }

        public string PrintBarcodeToFilePdf(int purchaseId)
        {
            var purchase = _purchaseRepository.GetById(purchaseId);
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));
            string guid = string.Format("{0,10:D6}", purchase.Id).Trim();
            //var fileName = $"barcode_{guid}_{GenerateRandomDigitCode(2)}.pdf";
            var fileName = $"barcode_{purchase.Id}.pdf";
            //var filePath = Combine(MapPath(ExportPath), fileName);
            //var filePath = string.Format(@"C:\source\blazor\blazor-TropiThai\src\BlazingPizza.Client\wwwroot\files\exportimport\{0}", fileName);
            var filePath = string.Format(ExportPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var purchases = new List<Purchase> { purchase };
                PrintPurchasesToPdf(fileStream, purchases);
            }

            return fileName;
        }
    }
}
