using Microsoft.AspNetCore.Mvc;
using Amazon.SQS;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS.Model;
using System.Text;
using Newtonsoft.Json;
using System;

namespace ProjetoPratico
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {

        private readonly IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.USWest1);


        private readonly string queueName = "HelloWorld";


        private string GetQueueURL()
        {
            try
            {
                return sqs.GetQueueUrlAsync(queueName).Result.QueueUrl;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            string queueURL = GetQueueURL();

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueURL
            };

            Task<ReceiveMessageResponse> receiveMessageResponse = sqs.ReceiveMessageAsync(receiveMessageRequest);

            var msgQueue = new StringBuilder();

            foreach(Message message in receiveMessageResponse.Result.Messages)
            {
                msgQueue.Append("= Message = \n");
                msgQueue.Append("\nMessageId: " + message.MessageId.ToString());
                msgQueue.Append("\nReceiptHandle: "+ message.ReceiptHandle);
                msgQueue.Append("\nMD5OfBody; " + message.MD5OfBody);
                msgQueue.Append("\nBody: " + message.Body);
            }

            return Ok(msgQueue.ToString());
        }

        [HttpPost]
        public IActionResult Post()
        {
            SendMessageRequest sendMessageRequest = new SendMessageRequest();
            sendMessageRequest.QueueUrl = GetQueueURL();
            sendMessageRequest.MessageBody = JsonConvert.SerializeObject("Hello World");

            try
            {
                sqs.SendMessageAsync(sendMessageRequest);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
