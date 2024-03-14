using Application.Abstractions;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public partial class UrlUtility : IUrlUtility
    {
        //private const string Base64UrlRegex = @"^[A-Za-z0-9_-]+$";
        public string EncodeUrlToBase64(string token)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenBytes);
        }

        public string DecodeBase64Url(string base64token)
        {
            byte[] tokenDecodedBytes = WebEncoders.Base64UrlDecode(base64token);
            return Encoding.UTF8.GetString(tokenDecodedBytes);
        }

        public bool TryDecodeBase64Url(string base64, out string? decoded)
        { 
            var isMatch = Base64UrlRegex().IsMatch(base64);

            if (!isMatch)
            {
                decoded = null;
                return false;
            }

            try
            {
                decoded = DecodeBase64Url(base64);
                return true;
            }
            catch
            {
                decoded = null;
                return false;
            }
        }

        [GeneratedRegex(@"^[A-Za-z0-9_-]+$", RegexOptions.IgnoreCase)]
        private static partial Regex Base64UrlRegex();
    }
}
