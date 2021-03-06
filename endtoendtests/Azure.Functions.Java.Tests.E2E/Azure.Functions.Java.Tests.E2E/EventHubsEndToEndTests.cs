// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Azure.Functions.Java.Tests.E2E
{
    public class EventHubsEndToEndTests 
    {
        [Fact]
        public async Task EventHubTriggerAndOutputJSON_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            try
            {
                await SetupQueue(Constants.OutputJsonEventHubQueueName);

                // Need to setup EventHubs: test-inputjson-java and test-outputjson-java
                await EventHubsHelpers.SendJSONMessagesAsync(expectedEventId);

                //Verify
                var queueMessage = await StorageHelpers.ReadFromQueue(Constants.OutputJsonEventHubQueueName);
                JObject json = JObject.Parse(queueMessage);
                Assert.Contains(expectedEventId, json["value"].ToString());
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(Constants.OutputJsonEventHubQueueName);
            }
        }

        [Fact]
        public async Task EventHubTriggerAndOutputString_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            try
            {
                await SetupQueue(Constants.OutputEventHubQueueName);

                // Need to setup EventHubs: test-input-java and test-output-java
                await EventHubsHelpers.SendMessagesAsync(expectedEventId, Constants.InputEventHubName);

                //Verify
                var queueMessage = await StorageHelpers.ReadFromQueue(Constants.OutputEventHubQueueName);
                Assert.Contains(expectedEventId, queueMessage);
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(Constants.OutputEventHubQueueName);
            }
        }

        [Fact]
        public async Task EventHubTriggerCardinalityOne_Succeeds()
        {
            string expectedEventId = Guid.NewGuid().ToString();
            try
            {
                await SetupQueue(Constants.OutputOneEventHubQueueName);

                // Need to setup EventHubs: test-inputOne-java and test-outputone-java
                await EventHubsHelpers.SendMessagesAsync(expectedEventId, Constants.InputCardinalityOneEventHubName);

                //Verify
                IEnumerable<string> queueMessages = await StorageHelpers.ReadMessagesFromQueue(Constants.OutputOneEventHubQueueName);
                Assert.True(queueMessages.All(msg => msg.Contains(expectedEventId)));
            }
            finally
            {
                //Clear queue
                await StorageHelpers.ClearQueue(Constants.OutputOneEventHubQueueName);
            }
        }

        private static async Task SetupQueue(string queueName)
        {
            //Clear queue
            await StorageHelpers.ClearQueue(queueName);

            //Set up and trigger            
            await StorageHelpers.CreateQueue(queueName);
        }
    }
}
