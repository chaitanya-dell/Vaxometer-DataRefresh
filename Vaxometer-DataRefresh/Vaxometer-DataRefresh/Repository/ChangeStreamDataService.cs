using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaxometer.Servicebus.Messages;
using Vaxometer.Servicebus.Publishers;
using Vaxometer_DataRefresh.Models.DataModels;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh.Repository
{
    public class ChangeStreamDataService : IChangeStreamDataService
    {
        private readonly IMongoCollection<Centers> _centers;
        private readonly ILogger _logger;
        private readonly IMessagePublisher _servicebus;
        
        public ChangeStreamDataService(IVexoDatabaseSettings settings, ILogger<ChangeStreamDataService> logger
            , IMessagePublisher servicebus)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _centers = database.GetCollection<Centers>("centers");
            _logger = logger;
            _servicebus = servicebus;
        }

        public async Task WatchSession()
        {
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<Centers>>().Match(x => x.OperationType == ChangeStreamOperationType.Update);
            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            try
            {
                using (var cursor = await _centers.WatchAsync(pipeline, options))
                {
                    await cursor.ForEachAsync(change =>
                    {
                        var isAvailableCapacity = change.FullDocument.sessions.FirstOrDefault(x => x.available_capacity > 0);

                        if (isAvailableCapacity != null)
                        {
                            // Raise event..Azure Event Grid
                            //Will raise c# event and event handler as of now. 
                            var message = new ChangeStream()
                            {
                                CenterId = change.FullDocument.center_id,
                                DistrictName = change.FullDocument.district_name,
                                PinCode = change.FullDocument.pincode
                            };
                             _servicebus.PublisherAsync<ChangeStream>(message);
                        }
                        // process change event
                        //var update = change.FullDocument;
                        //todo: raise an event for document change
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
    }
}
