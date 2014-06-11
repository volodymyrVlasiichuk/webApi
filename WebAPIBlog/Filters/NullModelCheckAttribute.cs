using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPIBlog.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class NullModelCheckAttribute : ActionFilterAttribute
	{
		readonly Func<Dictionary<string, object>, bool> _validate;

		public NullModelCheckAttribute() : this(arguments =>
			arguments.ContainsValue(null))
		{
		}

		public NullModelCheckAttribute(Func<Dictionary<string, object>, bool> checkCondition)
		{
			_validate = checkCondition;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (_validate(actionContext.ActionArguments))
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(
					HttpStatusCode.BadRequest, "The request cannot be empty.");
			}
		}
	}
}