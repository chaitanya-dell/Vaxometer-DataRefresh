using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Vaxometer_DataRefresh.Models.DataModels;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh.Manager
{
    public class VexoDataService : IVexoDataService
    {
        private readonly IMongoCollection<Sessions> _session;
        private readonly IMongoCollection<Centers> _centers;
        public VexoDataService(IVexoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _session = database.GetCollection<Sessions>("sessions");
            _centers = database.GetCollection<Centers>("centers");
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
                        }

                        // process change event
                        //var update = change.FullDocument;
                        //todo: raise an event for document change
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
