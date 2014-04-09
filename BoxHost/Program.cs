using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using BoxManager;

namespace BoxHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://" + Environment.MachineName + ":8000/";
            ServiceHost host = new ServiceHost(typeof(BoxService), new Uri(baseAddress));
            host.AddServiceEndpoint(typeof(IBoxService), new WebHttpBinding(), "").Behaviors.Add(new WebHttpBehavior());
            host.Open();
            Console.WriteLine("Service is running");
            Console.Write("Press ENTER to close the host");
            Console.ReadLine();
            host.Close();
        }
    }
}
