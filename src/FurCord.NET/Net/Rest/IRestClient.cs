using System.Threading.Tasks;

namespace FurCord.NET
{
	public interface IRestClient
	{
		/// <summary>
		/// Executes a REST request, or requeues and waits if necessary. The result is set on <see cref="RestRequest.Response"/> of the passed request..
		/// </summary>
		/// <param name="request">The request to requeue.</param>
		Task DoRequestAsync(RestRequest request);
		Task<T> DoRequestAsync<T>(RestRequest request);
	}
}