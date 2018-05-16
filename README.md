# hello-kafka-on-eventhubs-cs
This repo demonstrates how to connect to a [Kafka-enabled Azure Event Hubs](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-for-kafka-ecosystem-overview) using [Confluent's .NET Client](https://github.com/confluentinc/confluent-kafka-dotnet).

## Caveat
[This doc](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-quickstart-kafka-enabled-event-hubs#send-and-receive-messages-with-kafka-in-event-hubs) describes how to provide auth config for connecting a Java-based Kafka Producer/Consumer. However the setting `sasl.jaas.config` won't work with Confluent's .NET Client and will throw an exception:

```
Unhandled Exception: System.ArgumentException: Java JAAS configuration is not supported, see https://github.com/edenhill/librdkafka/wiki/Using-SASL-with-librdkafka for more information.
```

## Solution
Instead of using `sasl.jaas.config`, use `sasl.username` and `sasl.password`. Furthermore on  Windows, default trusted root CA certs are stored in Windows Registry. They're not automatically discovered by Confluent client. You'll need to obtain these from somewhere else, hence I've used the `cacert.pem` file distributed with [curl](https://curl.haxx.se/ca/cacert.pem), set its build setting to "Always copy to output directory" and reference it in the `ssl.ca.location`.

## Reference
[ConfluentCloud's C# example](https://github.com/confluentinc/confluent-kafka-dotnet/blob/17004b179df7fcde38e12062391b60c9a37d4a41/examples/ConfluentCloud/Program.cs#L51)