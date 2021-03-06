﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FoxIDs.Models.Logic
{
    public class LogoutRequest
    {
        [Required]
        public Party DownParty { get; set; }

        public string SessionId { get; set; }
         
        public bool RequireLogoutConsent { get; set; }

        public bool PostLogoutRedirect { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
