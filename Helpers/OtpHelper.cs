using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using google2fa.API.Services;

namespace google2fa.API.Helpers
{
    public static class OtpHelper
    {
        private const string OTP_HEADER = "X-OTP";

        public static bool HasValidTotp(this HttpRequest request, string key)
        {
            if (request.Headers.ContainsKey(OTP_HEADER))
            {
                string otp = request.Headers[OTP_HEADER].First();

                // We need to check the passcode against the past, current, and future passcodes

                if (!string.IsNullOrWhiteSpace(otp))
                {
                    if (TimeSensitivePassCode.GetListOfOTPs(key).Any(t => t.Equals(otp)))
                    {
                        return true;
                    }
                }

            }
            return false;
        }

    }
}