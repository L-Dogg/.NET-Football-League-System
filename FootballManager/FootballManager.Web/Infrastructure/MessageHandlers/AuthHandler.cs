using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using FootballManager.Web.Infrastructure.Extensions;

namespace FootballManager.Web.Infrastructure.MessageHandlers
{
	public class AuthHandler : DelegatingHandler
	{
		IEnumerable<string> authHeaderValues = null;
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			try
			{
				request.Headers.TryGetValues("Authorization", out authHeaderValues);
				if (authHeaderValues == null)
					return base.SendAsync(request, cancellationToken); // cross fingers

				var tokens = authHeaderValues.FirstOrDefault();
				tokens = tokens.Replace("Basic", "").Trim();
				if (!string.IsNullOrEmpty(tokens))
				{
					byte[] data = Convert.FromBase64String(tokens);
					string decodedString = Encoding.UTF8.GetString(data);
					string[] tokensValues = decodedString.Split(':');
					var membershipService = request.GetMembershipService();

					var membershipCtx = membershipService.ValidateUser(tokensValues[0], tokensValues[1]);
					if (membershipCtx.User != null)
					{
						IPrincipal principal = membershipCtx.Principal;
						Thread.CurrentPrincipal = principal;
						HttpContext.Current.User = principal;
					}
					else // Unauthorized access - wrong crededentials
					{
						var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
						var tsc = new TaskCompletionSource<HttpResponseMessage>();
						tsc.SetResult(response);
						return tsc.Task;
					}
				}
				else
				{
					var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
					var tsc = new TaskCompletionSource<HttpResponseMessage>();
					tsc.SetResult(response);
					return tsc.Task;
				}
				return base.SendAsync(request, cancellationToken);
			}
			catch (Exception ex)
			{
				var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
				var tsc = new TaskCompletionSource<HttpResponseMessage>();
				tsc.SetResult(response);
				return tsc.Task;
			}
		}
	}
}