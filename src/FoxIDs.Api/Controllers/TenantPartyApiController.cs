﻿using FoxIDs.Infrastructure;
using FoxIDs.Models;
using Api = FoxIDs.Models.Api;
using FoxIDs.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using System;
using FoxIDs.Logic;

namespace FoxIDs.Controllers
{
    /// <summary>
    /// Abstract party api.
    /// </summary>
    public abstract class TenantPartyApiController<AParty, MParty> : TenantApiController where AParty : Api.INameValue where MParty : Party
    {
        private readonly TelemetryScopedLogger logger;
        private readonly IMapper mapper;
        private readonly ITenantRepository tenantService;
        private readonly ValidatePartyLogic validatePartyLogic;

        public TenantPartyApiController(TelemetryScopedLogger logger, IMapper mapper, ITenantRepository tenantService, ValidatePartyLogic validatePartyLogic) : base(logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.tenantService = tenantService;
            this.validatePartyLogic = validatePartyLogic;
        }

        protected async Task<ActionResult<AParty>> Get(string name)
        {
            try
            {
                if (!ModelState.TryValidateRequiredParameter(name, nameof(name))) return BadRequest(ModelState);

                var MParty = await tenantService.GetAsync<MParty>(await GetId(name));
                return Ok(mapper.Map<AParty>(MParty));
            }
            catch (CosmosDataException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    logger.Warning(ex, $"Get '{typeof(AParty).Name}' by name '{name}'.");
                    return NotFound(typeof(AParty).Name, name);
                }
                throw;
            }
        }

        protected async Task<ActionResult<AParty>> Post(AParty party, Func<AParty, Task<bool>> validateApiModelAsync, Func<AParty, MParty, Task<bool>> validateModelAsync)
        {
            try
            {
                if (!await ModelState.TryValidateObjectAsync(party) || !await validateApiModelAsync(party)) return BadRequest(ModelState);

                var mParty = mapper.Map<MParty>(party);
                if (!await (party is Api.IDownParty downParty ? validatePartyLogic.ValidateAllowUpPartiesAsync(ModelState, nameof(downParty.AllowUpPartyNames), mParty as DownParty) : Task.FromResult(true))) return BadRequest(ModelState);
                if (!await validateModelAsync(party, mParty)) return BadRequest(ModelState);

                await tenantService.CreateAsync(mParty);

                return Created(mapper.Map<AParty>(mParty));
            }
            catch (CosmosDataException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict)
                {
                    logger.Warning(ex, $"Create '{typeof(AParty).Name}' by name '{party.Name}'.");
                    return Conflict(typeof(AParty).Name, party.Name);
                }
                throw;
            }
        }

        protected async Task<ActionResult<AParty>> Put(AParty party, Func<AParty, Task<bool>> validateApiModelAsync, Func<AParty, MParty, Task<bool>> validateModelAsync)
        {
            try
            {
                if (!await ModelState.TryValidateObjectAsync(party) || !await validateApiModelAsync(party)) return BadRequest(ModelState);

                var mParty = mapper.Map<MParty>(party);
                if (!await (party is Api.IDownParty downParty ? validatePartyLogic.ValidateAllowUpPartiesAsync(ModelState, nameof(downParty.AllowUpPartyNames), mParty as DownParty) : Task.FromResult(true))) return BadRequest(ModelState);
                if (!await validateModelAsync(party, mParty)) return BadRequest(ModelState);

                await tenantService.UpdateAsync(mParty);

                return Ok(mapper.Map<AParty>(mParty));
            }
            catch (CosmosDataException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    logger.Warning(ex, $"Update '{typeof(AParty).Name}' by name '{party.Name}'.");
                    return NotFound(typeof(AParty).Name, party.Name);
                }
                throw;
            }
        }

        protected async Task<IActionResult> Delete(string name)
        {
            try
            {
                if (!ModelState.TryValidateRequiredParameter(name, nameof(name))) return BadRequest(ModelState);

                await tenantService.DeleteAsync<MParty>(await GetId(name));
                return NoContent();
            }
            catch (CosmosDataException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    logger.Warning(ex, $"Delete '{typeof(AParty).Name}' by id '{name}'.");
                    return NotFound(typeof(AParty).Name, name);
                }
                throw;
            }
        }

        private Task<string> GetId(string name)
        {
            if(EqualsBaseType(0, typeof(MParty), (typeof(UpParty))))
            {
                return UpParty.IdFormat(RouteBinding, name);
            }
            else if (EqualsBaseType(0, typeof(MParty), (typeof(DownParty))))
            {
                return DownParty.IdFormat(RouteBinding, name);
            }
            else
            {
                throw new NotSupportedException($"{typeof(MParty)} type not supported.");
            }
        }

        private bool EqualsBaseType(int recursivCount, Type type, Type baseType)
        {
            var bt = type.BaseType;
            if (bt.Equals(baseType)) return true;

            if (recursivCount > 2) return false;

            recursivCount++;
            return EqualsBaseType(recursivCount, bt, baseType);
        }
    }
}
