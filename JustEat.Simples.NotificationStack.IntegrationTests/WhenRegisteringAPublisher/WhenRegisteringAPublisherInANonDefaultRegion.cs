using Amazon;
using Amazon.SimpleNotificationService.Model;
using JustEat.Simples.NotificationStack.Messaging.Messages;
using JustEat.Simples.NotificationStack.Stack;
using JustEat.Testing;
using NUnit.Framework;

namespace NotificationStack.IntegrationTests.WhenRegisteringAPublisher
{
    public class WhenRegisteringAPublisherInANonDefaultRegion : FluentNotificationStackTestBase
    {
        private string _topicName;
        private RegionEndpoint _regionEndpoint;
        private Topic _topic;

        protected override void Given()
        {
            _regionEndpoint = RegionEndpoint.USEast1;
            _topicName = "NonDefaultRegionTestTopic";

            Configuration = new MessagingConfig
            {
                Component = "integrationtestcomponent",
                Environment = "integrationtest",
                Tenant = "all",
                Region = _regionEndpoint.SystemName
            };

            DeleteTopicIfItAlreadyExists(_regionEndpoint, _topicName);

        }

        protected override void When()
        {
            SystemUnderTest.WithSnsMessagePublisher<Message>(_topicName);
        }

        [Then]
        public void ASnsTopicIsCreatedInTheNonDefaultRegion()
        {
            Assert.IsTrue(TryGetTopic(_regionEndpoint, _topicName, out _topic));
        }

        [TearDown]
        public void TearDown()
        {
            if (_topic != null)
            {
                DeleteTopic(_regionEndpoint, _topic);
            }
        }
    }
}