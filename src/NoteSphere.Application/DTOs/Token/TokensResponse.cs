﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Token
{
    public record TokensResponse(string AccessToken, string RefreshToken);
}
