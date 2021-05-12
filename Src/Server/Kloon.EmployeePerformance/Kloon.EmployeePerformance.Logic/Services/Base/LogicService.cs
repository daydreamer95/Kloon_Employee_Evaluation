﻿using Kloon.EmployeePerformance.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services.Base
{
    public interface ILogicService<T> : ICommonService
    {
        ValidationResult Start();
    }
    public class LogicService<T> : CommonService, ILogicService<T>
    {
        public LogicService(
             IServiceProvider serviceProvider,
             IUnitOfWork<EmployeePerformanceContext> dbContext,
             ILoggerFactory loggerFactory
        ) : base(serviceProvider, dbContext, loggerFactory.CreateLogger<T>())
        {
        }

        public ValidationResult Start()
        {
            var logicResult = new LogicResult(null, this);
            return new ValidationResult(logicResult);
        }
    }
}
