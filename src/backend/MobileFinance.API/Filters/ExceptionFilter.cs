using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MobileFinance.Communication.Responses;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is MobileFinanceException exception)
            HandleProjectException(exception, context);
        else
            ThrowUnknownException(context);
    }

    private static void HandleProjectException(MobileFinanceException mobileFinanceException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)mobileFinanceException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(mobileFinanceException.GetErrorMessages()));
    }

    private static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ExceptionMessages.UNKNOWN_ERROR));
    }
}
