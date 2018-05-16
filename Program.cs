using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloKafkaOnEventHub
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new Dictionary<string, object>
            {
                { "bootstrap.servers", "<EVENTHUB_NAMESPACE>.servicebus.windows.net:9093" },
                { "security.protocol", "SASL_SSL" },
                { "sasl.mechanism", "PLAIN"},
                { "group.id", "$Default"},
                //{ "debug", "generic,broker,topic,metadata,feature,queue,protocol,msg,security,all" },
                { "sasl.username", "$ConnectionString" },
                { "sasl.password", "Endpoint=sb://<EVENTHUB_NAMESPACE>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<PRIMARY_KEY>" },
                { "ssl.ca.location", "cacert.pem" },
            };

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg)
                  => Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");

                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe("kafka-events");

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }
        }
    }    
}
