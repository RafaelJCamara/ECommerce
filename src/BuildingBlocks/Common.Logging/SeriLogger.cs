using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    /*
        * Serilog hooks up to the Microsoft logging abstraction (ILogger)
        * This means that you don't have to do much changes in our code, only make the configurations
        * Serilog works with log events, instead of log messages (like log4net).
        * An event is made up of:
           -> Timestamp: when the event happened
           -> Level: level of the event (ex. Debug, Information, Error)
           -> Message: description of what the event represents
           -> Properties: there might be other properties present that describe the event
           -> Exception: there might be an exception object
        * Serilog provides us with a way of structured logging, meaning logs that have a specific format.
        * By having this structure, it's easier to perform operations over the logs, such as processing or searching them.
    */
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

                configuration
                    /*
                     * Enrichments consist of adding additional context values. so that we can use them across logging statements
                     * For instance, if we do Enrich.WithMachineName, our logs will contain a property named MachineName with the corresponding value
                     */
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    //this is the debug sink
                    .WriteTo.Debug()
                    //this is the console sink
                    .WriteTo.Console()
                    //this enables us to get our logs to be written to Elastic Search
                    //this is a sink, meaning to where we can send our logs to (a target for our logs)
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            // this is the index format, on which the logs will be grouped
                            IndexFormat =
                                $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                            AutoRegisterTemplate = true,
                            /*
                             * There is no limit to how many documents an index can group, meaning we can exceed the storage limit of the hosting server
                             * By using shards, we are doing an horizontal split up on our index, where each shard will contain a sub-section of our index
                             * The size of the shard is fixed when the index is created, meaning we can't change it after creating the index
                             */
                            NumberOfShards = 2,
                            /*
                             * Copies of our index shards.
                             * If we are doing read requests, we can use the replicas, which can improve the search performance.
                             */
                            NumberOfReplicas = 1
                        })
                    /*
                     * This a special type of enrichment which is a "fixed property" enrichment (done with the Enrich.WithProperty)
                     * For example, by using the two below, every log will have those two properties: Environment and Application
                     */
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    // we need this so we can read configurations
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}