using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IUrlUtility
    {
        string EncodeUrlToBase64(string token);
        string DecodeBase64Url(string base64token);
        bool TryDecodeBase64Url(string input, out string? output);
    }
}
