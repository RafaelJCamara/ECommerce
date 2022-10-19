using Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shopping.Aggregator
{
    /*
        -> All .NET Core applications start as Console application, with this Program.cs file
        -> This file contains our Main method, which is the entry point of our console app
        -> Program class is more directed to infrastructure definition, such as Logging, Kestrel server and IIS Integration
     */
    public class Program
    {
        /*
            -> In .NET Core applications, this Main method is used to run an IWebHost instance.
         */
        public static void Main(string[] args)
        {
            CreateHostBuilder(args) // Create IHostBuilder using the CreateHostBuilder method
                .Build() // Build and return an instance of IHost from the IHostBuilder
                .Run(); // Run the IHost, starts listening for requests and generatig responses
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args) // Creates IHostBuilder using default configurations.
                //by using this, Serilog will catch logs produced by Microsoft's ILogger
                .UseSerilog(SeriLogger.Configure)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }); // Registers Startup class (which defines most of our application's configurations)
        }
    }
}