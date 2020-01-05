using System;
using System.Collections.Generic;
using System.Text;

namespace MissionControl.Shared.Models.Common
{
    public static class CommonUtils
    {
        public static string GenerateBarCodeEAN13(int id)
        {
            string code = string.Format("{0,10:D13}", id);
            var sum = 0;

            for (var i = code.Length; i >= 1; i--)
            {
                var d = Convert.ToInt32(code.Substring(i - 1, 1));
                var f = i % 2 == 0 ? 3 : 1;
                sum += d * f;
            }
            var checksum = (10 - (sum % 10)) % 10;

            return code + checksum;
        }
    }
}
