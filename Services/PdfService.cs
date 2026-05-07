using iTextSharp.text;
using iTextSharp.text.pdf;
using BarangaySystem.Models;

namespace BarangaySystem.Services
{
    public class PdfService
    {
        public byte[] GeneratePdf(DocumentRequest request)
        {
            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 60, 60, 80, 80);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            //Set up fonts
            var bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var normal = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            var small = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.Gray);
            var title = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, new BaseColor(30, 64, 175));

            //Header
            var h = new Paragraph("BARANGAY DOCUMENT REQUEST SYSTEM\n", title);
            h.Alignment = Element.ALIGN_CENTER;
            doc.Add(h);

            var docName = new Paragraph((request.DocumentType?.Name ?? "Document").ToUpper() + "\n\n", bold);
            docName.Alignment = Element.ALIGN_CENTER;
            doc.Add(docName);

            //Details table
            var table = new PdfPTable(2);
            table.WidthPercentage = 90;
            table.SetWidths(new float[] { 40f, 60f });

            //Helper to add a row to the table
            void Row(string label, string value)
            {
                table.AddCell(new PdfPCell(new Phrase(label, bold))
                    { BackgroundColor = new BaseColor(239,246,255), Padding=8 });
                table.AddCell(new PdfPCell(new Phrase(value, normal))
                    { Padding = 8 });
            }

            Row("Reference No:", request.ReferenceNumber);
            Row("Resident Name:", request.User?.FullName ?? "");
            Row("Address:", request.User?.Address ?? "");
            Row("Purpose:", request.Purpose);
            Row("Date Field:", request.RequestDate.ToString("MMM dd, yyyy"));
            Row("Status:", request.Status);

            doc.Add(table);
            doc.Add(new Paragraph("\n\n"));

            // Notice
            var note = new Paragraph(
                "NOTICE: This is a soft copy for reference only. " +
                "The official hard copy is released at the Barangay Office.", small);
            note.Alignment = Element.ALIGN_CENTER;
            doc.Add(note);

            doc.Close();
            return ms.ToArray();
        }
    }
}