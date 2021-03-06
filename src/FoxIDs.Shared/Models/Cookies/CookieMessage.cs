﻿using Newtonsoft.Json;
using System;

namespace FoxIDs.Models.Cookies
{
    public abstract class CookieMessage
    {
        [JsonProperty(PropertyName = "ct")]
        public long CreateTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
