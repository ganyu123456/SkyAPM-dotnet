/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcGreeter;
using Microsoft.Extensions.DependencyInjection;
using SkyApm.Diagnostics.Grpc.Server;

namespace SkyApm.Sample.GrpcServer
{
    public static class Extensions
    {
        public static IServiceProvider StartGrpcServer(this IServiceProvider provider)
        {
            var interceptor = provider.GetService<ServerDiagnosticInterceptor>();
            var definition = Greeter.BindService(new GreeterImpl());
            if (interceptor != null)
            {
                definition = definition.Intercept(interceptor);
            }
            int port = 12345;
            Server server = new Server
            {
                Services = { definition },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) },
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + port);
            return provider;
        }
    }
}
