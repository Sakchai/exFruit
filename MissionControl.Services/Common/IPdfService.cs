using MissionControl.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Services.Common
{
    public partial interface IPdfService
    {
        string PrintBarcodeToPdf(int purchaseId);
    }
}
