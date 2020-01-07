using MissionControl.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MissionControl.Services.Common
{
    public partial interface IPdfService
    {
        void PrintBarcodeToPdf(Stream stream, int purchaseId);
        string PrintBarcodeToFilePdf(int purchaseId);
    }
}
