﻿using FoxIDs.Models;
using ITfoxtec.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FoxIDs
{
    public static class TrackKeyExtensions
    {
        public static JsonWebKey GetPublicKey(this TrackKey trackKey)
        {
            return trackKey.Key.GetPublicKey();
        }
    }
}
