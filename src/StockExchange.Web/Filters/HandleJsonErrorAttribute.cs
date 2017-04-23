using log4net;
using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Web.Helpers.Json;
using System;
using System.Net;
using System.Web.Mvc;

namespace StockExchange.Web.Filters
{
    /// <summary>
    /// Filter for reacting to exceptions while processing an AJAX request
    /// </summary>
    public class HandleJsonErrorAttribute : FilterAttribute, IExceptionFilter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HandleJsonErrorAttribute));

        /// <inheritdoc />
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            if (filterContext.Exception is BusinessException)
            {
                filterContext.ExceptionHandled = true;
                var exception = filterContext.Exception as BusinessException;
                logger.Error("Business error", exception);

                filterContext.HttpContext.Response.StatusCode = (int) GetHttpStatusCode(exception.Status);
                filterContext.Result = new JsonNetResult(exception.Errors);
            }
        }

        private static HttpStatusCode GetHttpStatusCode(ErrorStatus exceptionStatus)
        {
            switch (exceptionStatus)
            {
                case ErrorStatus.AccessDenied:
                    return HttpStatusCode.Forbidden;
                case ErrorStatus.DataNotFound:
                    return HttpStatusCode.NotFound;
                // ReSharper disable once RedundantCaseLabel
                case ErrorStatus.BusinessRuleViolation:
                default:
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}