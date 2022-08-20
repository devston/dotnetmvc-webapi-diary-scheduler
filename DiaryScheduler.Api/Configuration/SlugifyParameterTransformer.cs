﻿using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace DiaryScheduler.Api.Configuration;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
    {
        // Slugify value.
        return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
