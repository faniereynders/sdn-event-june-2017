using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Text;
using System.Configuration;

namespace Awesome.Webhook
{
    public static class VSTSWebhook
    {
        [FunctionName("VSTSWebHook")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            var data = JsonConvert.DeserializeObject<dynamic>(await req.Content.ReadAsStringAsync());

            var buildResult = (string)data.resource.status;

            log.Info($"Build status: {buildResult}");

            var connectionString = ConfigurationManager.AppSettings["AwesomeIoTHub"];

            var serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            await serviceClient.SendAsync("awesome-device", new Message(Encoding.UTF8.GetBytes(buildResult)));

            log.Info($"C# Event Hub trigger function processed a message.");

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
