using Microsoft.AspNetCore.Mvc;
using Amazon.SQS;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS.Model;
using System.Text;
using Newtonsoft.Json;
using System;
using Amazon.Runtime;

namespace ProjetoPratico
{

    public class AmazonSqsConnection
    {

        private readonly IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.USWest1);
        private readonly string queueName = "HelloWorld";

        public string GetQueueURL()
        {
            try
            {
                return sqs.GetQueueUrlAsync(queueName).Result.QueueUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

    }
}
