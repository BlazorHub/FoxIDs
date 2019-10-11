﻿using AutoMapper;
using FoxIDs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using Api = FoxIDs.Models.Api;

namespace FoxIDs.MappingProfiles
{
    public class TenantMappingProfiles : Profile
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private RouteBinding RouteBinding => httpContextAccessor.HttpContext.GetRouteBinding();

        public TenantMappingProfiles(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            Mapping();
            UpMapping();
            DownMapping();
        }

        private void Mapping()
        {
            CreateMap<JsonWebKey, Api.JsonWebKey>()
                .ReverseMap()
                .ForMember(d => d.X5c, opt => opt.NullSubstitute(new List<string>()))
                .ForMember(d => d.KeyOps, opt => opt.NullSubstitute(new List<string>()));

            CreateMap<OAuthClientSecret, Api.OAuthClientSecretResponse>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Id))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Name.GetFirstInDotList()));

            CreateMap<SamlBinding, Api.SamlBinding>()
                .ReverseMap();
        }

        private void UpMapping()
        {
            CreateMap<LoginUpParty, Api.LoginUpParty>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => UpParty.IdFormat(RouteBinding, s.Name).GetAwaiter().GetResult()));

            CreateMap<SamlUpParty, Api.SamlUpParty>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => UpParty.IdFormat(RouteBinding, s.Name).GetAwaiter().GetResult()));
        }

        private void DownMapping()
        {
            CreateMap<OAuthDownParty, Api.OAuthDownParty>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.AllowUpPartyNames, opt => opt.MapFrom(s => s.AllowUpParties.Select(aup => aup.Name)))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => DownParty.IdFormat(RouteBinding, s.Name).GetAwaiter().GetResult()))
                .ForMember(d => d.AllowUpParties, opt => opt.MapFrom(s => s.AllowUpPartyNames.Select(n => new UpPartyLink { Name = n })));
            CreateMap<OAuthDownClaim, Api.OAuthDownClaim>()
                .ReverseMap();
            CreateMap<OAuthDownClient, Api.OAuthDownClient>()
                .ReverseMap();
            CreateMap<OAuthDownResource, Api.OAuthDownResource>()
                .ReverseMap();
            CreateMap<OAuthDownResourceScope, Api.OAuthDownResourceScope>()
                .ReverseMap();
            CreateMap<OAuthDownScope, Api.OAuthDownScope>()
                .ReverseMap();

            CreateMap<OidcDownParty, Api.OidcDownParty>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.AllowUpPartyNames, opt => opt.MapFrom(s => s.AllowUpParties.Select(aup => aup.Name)))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => DownParty.IdFormat(RouteBinding, s.Name).GetAwaiter().GetResult()))
                .ForMember(d => d.AllowUpParties, opt => opt.MapFrom(s => s.AllowUpPartyNames.Select(n => new UpPartyLink { Name = n })));
            CreateMap<OidcDownClaim, Api.OidcDownClaim>()
                .ReverseMap();
            CreateMap<OidcDownClient, Api.OidcDownClient>()
                .ReverseMap();
            CreateMap<OidcDownScope, Api.OidcDownScope>()
                .ReverseMap();

            CreateMap<SamlDownParty, Api.SamlDownParty>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.AllowUpPartyNames, opt => opt.MapFrom(s => s.AllowUpParties.Select(aup => aup.Name)))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => DownParty.IdFormat(RouteBinding, s.Name).GetAwaiter().GetResult()))
                .ForMember(d => d.AllowUpParties, opt => opt.MapFrom(s => s.AllowUpPartyNames.Select(n => new UpPartyLink { Name = n })));
        }
    }
}
