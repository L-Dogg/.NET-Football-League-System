using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FootballManager.Web.Infrastructure.core
{
public class ApiControllerBase : ApiController
	{

		public ApiControllerBase()
		{
		}

		protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> function)
		{
			HttpResponseMessage response = null;

			try
			{
				response = function.Invoke();
			}
			catch (DbUpdateException ex)
			{
				response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
			}
			catch (Exception ex)
			{
				response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			return response;
		}

		protected async Task<HttpResponseMessage> CreateHttpResponse(HttpRequestMessage request, Func<Task<HttpResponseMessage>> function)
		{
			HttpResponseMessage response = null;

			try
			{
				response = await function.Invoke();
			}
			catch (DbUpdateException ex)
			{
				response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
			}
			catch (Exception ex)
			{
				response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			return response;
		}
	}
}