using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //for DbUpdate we wan't to see the InnerException in logs
            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(DbUpdateException))
            {
                context.Exception = new Exception(context.Exception.InnerException.Message);
            }
        }
    }
}
