﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCrud.Shared
{
    public record EmbeddedReportViewModel
    (
        string Id,
        string Name,
        string EmbedUrl,
        string token
    );

}
