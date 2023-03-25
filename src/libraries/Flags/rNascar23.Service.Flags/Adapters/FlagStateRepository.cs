﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using rNascar23.Data;
using rNascar23.Data.Flags.Ports;
using rNascar23.Flags.Models;
using rNascar23.Service.Flags.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.Service.Flags.Adapters
{
    internal class FlagStateRepository : JsonDataRepository, IFlagStateRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<FlagStateRepository> _logger;

        public FlagStateRepository(IMapper mapper, ILogger<FlagStateRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Url { get => @"https://cf.nascar.com/live/feeds/live-flag-data.json"; }

        public async Task<IList<FlagState>> GetFlagStatesAsync()
        {
            try
            {
                // var json = System.IO.File.ReadAllText(@"C:\Users\Rob\source\repos\rNascar23\src\libraries\FlagStates\rNascar23.FlagStates\Examples\example-flag-data.json");

                //var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FlagStateListModel>(json);

                //var flagStates = _mapper.Map<List<FlagState>>(model.FlagStates);

                var json = await GetAsync(Url).ConfigureAwait(false);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FlagStateModel[]>(json);

                var flagStates = _mapper.Map<List<FlagState>>(model);

                return flagStates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
