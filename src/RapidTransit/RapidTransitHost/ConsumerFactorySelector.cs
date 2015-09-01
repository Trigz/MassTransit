﻿// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace RapidTransit
{
    using Autofac;
    using MassTransit;
    using MassTransit.AutofacIntegration;


    public class ConsumerFactorySelector<TConsumer> :
        IConsumerFactorySelector<TConsumer>
        where TConsumer : class, IConsumer
    {
        readonly ILifetimeScope _scope;
        string _name;

        public ConsumerFactorySelector(ILifetimeScope scope)
        {
            _scope = scope;
            _name = "message";
        }

        public IConsumerFactory<TConsumer> ConsumerFactory()
        {
            return new AutofacConsumerFactory<TConsumer>(_scope, _name);
        }
    }
}