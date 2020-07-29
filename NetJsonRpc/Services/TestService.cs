﻿using System;

using Microsoft.Extensions.Logging;

namespace NetJsonRpc.Services
{
    public class TestService
    {
        private readonly ILogger<TestService> _logger;

        public TestService()
        {
            LoggerFactory loggerFactory = new LoggerFactory();

            this._logger = loggerFactory.CreateLogger<TestService>();
        }

        public TestService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<TestService>();
        }

        public string Hello(Person person)
        {
            _logger.LogInformation("TestService.Hello(" + person + ")...");

            if (person.Firstname != null && person.Firstname.Equals("error"))
            {
                throw new Exception("Test error");
            }

            return "Hello " + person + ".";
        }

        public string Hello(Person person, string locale)
        {
            _logger.LogInformation("TestService.Hello(" + person + "," + locale + ")...");

            if (locale != null && locale.ToLower().Equals("it"))
            {
                return "Ciao " + person + ".";
            }

            return "Hello " + person + ".";
        }
    }
}
