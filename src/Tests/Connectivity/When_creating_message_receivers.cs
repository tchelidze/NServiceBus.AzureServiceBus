namespace NServiceBus.AzureServiceBus.Tests
{
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NServiceBus.Azure.WindowsAzureServiceBus.Tests;
    using NServiceBus.Settings;
    using NUnit.Framework;

    [TestFixture]
    [Category("AzureServiceBus")]
    public class When_creating_message_receivers
    {
        [Test]
        public void Delegates_creation_to_messaging_factory()
        {
            var settings = new DefaultConfigurationValues().Apply(new SettingsHolder());

            var factory = new InterceptedMessagingFactory();

            var creator = new MessageReceiverCreator(new InterceptedMessagingFactoryFactory(factory), settings);

            var receiver = creator.CreateAsync("myqueue", AzureServiceBusConnectionString.Value).Result;

            Assert.IsTrue(factory.IsInvoked);
            Assert.IsInstanceOf<IMessageReceiver>(receiver);
        }

        [Test]
        public void Applies_user_defined_connectivity_settings()
        {
            var settings = new DefaultConfigurationValues().Apply(new SettingsHolder());

            var extensions = new TransportExtensions<AzureServiceBusTransport>(settings);
            extensions.Connectivity().MessageReceivers()
                      .PrefetchCount(1000)
                      .RetryPolicy(RetryPolicy.NoRetry)
                      .ReceiveMode(ReceiveMode.ReceiveAndDelete);

            var factory = new InterceptedMessagingFactory();

            var creator = new MessageReceiverCreator(new InterceptedMessagingFactoryFactory(factory), settings);

            var receiver = (IMessageReceiver)creator.CreateAsync("myqueue", AzureServiceBusConnectionString.Value).Result;

            Assert.AreEqual(ReceiveMode.ReceiveAndDelete, receiver.Mode);
            Assert.IsInstanceOf<NoRetry>(receiver.RetryPolicy);
            Assert.AreEqual(1000, receiver.PrefetchCount);
        }

        class InterceptedMessagingFactoryFactory : IManageMessagingFactoryLifeCycle
        {
            readonly IMessagingFactory factory;

            public InterceptedMessagingFactoryFactory(IMessagingFactory factory)
            {
                this.factory = factory;
            }

            public IMessagingFactory Get(string @namespace)
            {
                return factory;
            }
        }

        class InterceptedMessagingFactory : IMessagingFactory
        {
            public bool IsInvoked;

            public bool IsClosed
            {
                get { throw new System.NotImplementedException(); }
            }

            public RetryPolicy RetryPolicy
            {
                get { throw new System.NotImplementedException(); }
                set { throw new System.NotImplementedException(); }
            }

            public Task<IMessageReceiver> CreateMessageReceiverAsync(string entitypath, ReceiveMode receiveMode)
            {
                IsInvoked = true;

                return Task.FromResult<IMessageReceiver>(new FakeMessageReceiver() { Mode = receiveMode });
            }
        }

        class FakeMessageReceiver : IMessageReceiver
        {
            public bool IsClosed
            {
                get { return false; }
            }

            public RetryPolicy RetryPolicy { get; set; }

            public int PrefetchCount { get; set; }

            public ReceiveMode Mode { get; internal set; }
        }
    }
}