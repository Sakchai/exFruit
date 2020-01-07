function PdfFileSaveAs(purchaseId) {
    var link = document.createElement('a');
    link.href = "/purchaseBarcode/" + purchaseId;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}