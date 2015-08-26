using System;
using System.Timers;
using Topshelf;
using Topshelf.WebApi;
using Topshelf.Ninject;
using Topshelf.WebApi.Ninject;

namespace ASPConsoleApp1
{
    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
        }
        public void Start() { _timer.Start(); }
        public void Stop() {
            _timer.Stop();
            Console.WriteLine("Now I stop!");
        }
    }

    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<TownCrier>(s =>                        //2
                {
                   s.ConstructUsing(name=> new TownCrier());     //3
                   s.WhenStarted(tc => tc.Start());              //4
                   s.WhenStopped(tc => tc.Stop());               //5
                   s.WebApiEndpoint(api => 
                //Topshelf.WebApi - Uses localhost as the domain, defaults to port 8080.
                //You may also use .OnHost() and specify an alternate port.
                api.OnLocalhost()
                    //Topshelf.WebApi - Pass a delegate to configure your routes
                  //  .ConfigureRoutes(Configure)
                    //Topshelf.WebApi.Ninject (Optional) - You may delegate controller 
                    //instantiation to Ninject.
                    //Alternatively you can set the WebAPI Dependency Resolver with
                    .UseDependencyResolver()
                   // .UseNinjectDependencyResolver()
                    //Instantaties and starts the WebAPI Thread.
                    .Build());
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("Stuff");                       //9
            });                                                  //10
        }
    }
}
