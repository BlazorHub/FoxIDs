﻿using FoxIDs.Infrastructure.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoxIDs.Models.Api
{
    public class OidcDownParty : IValidatableObject, IDownParty, INameValue
    {
        [Required]
        [MaxLength(Constants.Models.PartyNameLength)]
        [RegularExpression(Constants.Models.PartyNameRegExPattern)]
        public string Name { get; set; }

        [Length(Constants.Models.DownParty.AllowUpPartyNamesMin, Constants.Models.DownParty.AllowUpPartyNamesMax, Constants.Models.PartyNameLength, Constants.Models.PartyNameRegExPattern)]
        public List<string> AllowUpPartyNames { get; set; }

        /// <summary>
        /// OIDC down client.
        /// </summary>
        [ValidateObject]
        public OidcDownClient Client { get; set; }

        /// <summary>
        /// OAuth 2.0 down resource.
        /// </summary>
        [ValidateObject]
        public OAuthDownResource Resource { get; set; }

        /// <summary>
        /// Allow cors origins.
        /// </summary>
        [Length(Constants.Models.OAuthDownParty.AllowCorsOriginsMin, Constants.Models.OAuthDownParty.AllowCorsOriginsMax, Constants.Models.OAuthDownParty.AllowCorsOriginLength)]
        public List<string> AllowCorsOrigins { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if(Client == null && Resource == null)
            {
                results.Add(new ValidationResult($"Either the field {nameof(Client)} or the field {nameof(Resource)} is required.", new[] { nameof(Client), nameof(Resource) }));
            }
            return results;
        }
    }
}
