﻿// Copyright 2007-2013 Chris Patterson
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
namespace MassTransit.Courier.Tests.Testing
{
    using System;
    using System.Threading.Tasks;


    public class ReviseWithNoChangeItineraryActivity :
        Activity<TestArguments, TestLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<TestArguments> context)
        {
            Console.WriteLine("ReviseWithNoChangeItineraryActivity: Execute: {0}", context.Arguments.Value);

            return context.ReviseItinerary(x => x.AddActivitiesFromSourceItinerary());
        }

        public async Task<CompensationResult> Compensate(CompensateContext<TestLog> context)
        {
            return context.Compensated();
        }
    }
}