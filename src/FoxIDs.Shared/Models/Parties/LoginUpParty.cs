﻿using FoxIDs.Infrastructure.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoxIDs.Models
{
    public class LoginUpParty : UpParty
    {
        public LoginUpParty()
        {
            Type = PartyType.Login;
        }

        [Range(Constants.Models.LoginUpParty.SessionLifetimeMin, Constants.Models.LoginUpParty.SessionLifetimeMax)] 
        [JsonProperty(PropertyName = "session_lifetime")]
        public int SessionLifetime { get; set; }

        [Range(Constants.Models.LoginUpParty.SessionAbsoluteLifetimeMin, Constants.Models.LoginUpParty.SessionAbsoluteLifetimeMax)]
        [JsonProperty(PropertyName = "session_absolute_lifetime")]
        public int SessionAbsoluteLifetime { get; set; }

        [Range(Constants.Models.LoginUpParty.PersistentAbsoluteSessionLifetimeMin, Constants.Models.LoginUpParty.PersistentAbsoluteSessionLifetimeMax)]
        [JsonProperty(PropertyName = "persistent_session_absolute_lifetime")]
        public int PersistentSessionAbsoluteLifetime { get; set; }

        [Required]
        [JsonProperty(PropertyName = "persistent_session_lifetime_unlimited")]
        public bool? PersistentSessionLifetimeUnlimited { get; set; }

        [Required]
        [JsonProperty(PropertyName = "enable_cancel_login")]
        public bool? EnableCancelLogin { get; set; }

        [Required]
        [JsonProperty(PropertyName = "enable_create_user")]
        public bool? EnableCreateUser { get; set; }

        [Required]
        [JsonProperty(PropertyName = "logout_consent")]
        public LoginUpPartyLogoutConsent LogoutConsent { get; set; }

        [Length(Constants.Models.LoginUpParty.AllowIframeOnDomainsMin, Constants.Models.LoginUpParty.AllowIframeOnDomainsMax, Constants.Models.LoginUpParty.AllowIframeOnDomainsLength)]
        [JsonProperty(PropertyName = "allow_iframe_on_domains")]
        public List<string> AllowIframeOnDomains { get; set; }

        [MaxLength(Constants.Models.LoginUpParty.CssStyleLength)]
        [JsonProperty(PropertyName = "css_style")]
        public string CssStyle { get; set; }
    }
}
