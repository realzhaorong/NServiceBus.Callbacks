﻿namespace NServiceBus.AcceptanceTests.Callbacks
{
    using System.Threading.Tasks;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_int_response : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_send_back_old_style_control_message()
        {
            var context = await Scenario.Define<Context>()
                .WithEndpoint<EndpointWithLocalCallback>(b => b.When(async (bus, c) =>
                {
                    c.Response = await bus.Request<int>(new MyRequest(), new SendOptions());
                    c.CallbackFired = true;
                }))
                .WithEndpoint<Replier>()
                .Done(c => c.CallbackFired)
                .Run();

            Assert.AreEqual(200, context.Response);
        }

        public class Context : ScenarioContext
        {
            public bool CallbackFired { get; set; }
            public int Response { get; set; }
        }

        public class Replier : EndpointConfigurationBuilder
        {
            public Replier()
            {
                EndpointSetup<DefaultServer>();
            }

            public class MyRequestHandler : IHandleMessages<MyRequest>
            {
                public Task Handle(MyRequest message, IMessageHandlerContext context)
                {
                    return context.Reply(200);
                }
            }
        }

        public class EndpointWithLocalCallback : EndpointConfigurationBuilder
        {
            public EndpointWithLocalCallback()
            {
                EndpointSetup<DefaultServer>()
                    .AddMapping<MyRequest>(typeof(Replier));
            }
        }

        public class MyRequest : IMessage
        {
        }

    }
}