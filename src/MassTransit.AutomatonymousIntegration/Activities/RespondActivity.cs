// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Automatonymous.Activities
{
    using System;
    using System.Threading.Tasks;
    using Contexts;
    using MassTransit;
    using MassTransit.Context;
    using MassTransit.Pipeline;


    public class RespondActivity<TInstance, TData, TMessage> :
        Activity<TInstance, TData>
        where TInstance : SagaStateMachineInstance
        where TData : class
        where TMessage : class
    {
        readonly Func<ConsumeEventContext<TInstance, TData>, TMessage> _messageFactory;
        readonly IPipe<SendContext<TMessage>> _publishPipe;

        public RespondActivity(Func<ConsumeEventContext<TInstance, TData>, TMessage> messageFactory,
            Action<SendContext<TMessage>> contextCallback)
        {
            _messageFactory = messageFactory;

            _publishPipe = Pipe.New<SendContext<TMessage>>(x =>
            {
                x.Execute(contextCallback);
            });
        }

        public RespondActivity(Func<ConsumeEventContext<TInstance, TData>, TMessage> messageFactory)
        {
            _messageFactory = messageFactory;

            _publishPipe = Pipe.Empty<SendContext<TMessage>>();
        }

        void Visitable.Accept(StateMachineVisitor inspector)
        {
            inspector.Visit(this);
        }

        async Task Activity<TInstance, TData>.Execute(BehaviorContext<TInstance, TData> context, Behavior<TInstance, TData> next)
        {
            ConsumeContext<TData> consumeContext;
            if (!context.TryGetPayload(out consumeContext))
                throw new ContextException("The consume context could not be retrieved.");

            var consumeEventContext = new AutomatonymousConsumeEventContext<TInstance, TData>(context, consumeContext);

            TMessage message = _messageFactory(consumeEventContext);

            await consumeContext.RespondAsync(message);
        }

        Task Activity<TInstance, TData>.Faulted<TException>(BehaviorExceptionContext<TInstance, TData, TException> context,
            Behavior<TInstance, TData> next)
        {
            return next.Faulted(context);
        }
    }
}