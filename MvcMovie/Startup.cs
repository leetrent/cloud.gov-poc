using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MvcMovie.Models;

namespace MvcMovie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            ////////////////////////////////////////////////////////////////////////
            // WRITE ENVIRONMENT VARIABLES TO THE LOG
            ////////////////////////////////////////////////////////////////////////
            // Console.WriteLine("[Startup] List of environment variables:");
            // var enumerator = Environment.GetEnvironmentVariables().GetEnumerator();
            // while (enumerator.MoveNext())
            // {
            //     Console.WriteLine($"{enumerator.Key,5}:{enumerator.Value,100}");
            // }
            ////////////////////////////////////////////////////////////////////////

            String connectionString = null;

            // CHECK FOR LOCAL 'LOCAL_CONNECTION_STRING' ENVIRONMENT VARIABLE
            // (in case we're running on localhost)
            connectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");
 
            // IF connectionString IS NULL, WE MUST BE RUNNING IN THE CLOUD,
            // (either CLOUD.GOV or Cloud Foundry)
            if (connectionString == null) {
                connectionString = BuildConnectionString();
            }
  
            // WRITE CONNECTION STRING TO THE LOG
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("[Startup] Connection String: " + connectionString);
            Console.WriteLine("********************************************************************************");
            
            // NOW THAT WE HAVE OUR CONNECTION STRING
            // WE CAN ESTABLISH OUR DB CONTEXT
            services.AddDbContext<MvcMovieContext>
            (
		        opts => opts.UseMySQL(connectionString)
		    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private String BuildConnectionString()
        {
            String connectionString = null;
                               
			//This section retrieves credentials from the VCAP_SERVICES
			try
			{
				string vcapServices = System.Environment.GetEnvironmentVariable("VCAP_SERVICES");

				if (vcapServices != null)
				{
					dynamic json = JsonConvert.DeserializeObject(vcapServices);
					foreach (dynamic obj in json.Children())
					{

						dynamic credentials = (((JProperty)obj).Value[0] as dynamic).credentials;
						if (credentials != null)
						{
							string host = credentials.host;
							string username = credentials.username;
							string password = credentials.password;
                            string port = credentials.port;
                            string db_name = credentials.db_name;

                            connectionString = "Username=" + username + ";"
                                + "Password=" + password + ";"
                                + "Host=" + host + ";"
                                + "Port=" + port + ";"
                                + "Database=" + db_name + ";Pooling=true;";

                            return connectionString;
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception in Startup.BuildConnectionString:");
				Console.WriteLine(e);
            }
            return "No Connection String";
		}
    }
}
