using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FurCord.NET
{
    /// <summary>
    /// Represents a rate-limit bucket.
    /// </summary>
	public sealed class RestBucket
    {
        //This could just be [MethodImpl(MethodImplOptions.Synchronized)] on the getter, right?
        private readonly SemaphoreSlim _wait = new(1);
		
        /// <summary>
        /// Max allotted requests.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// The alloted requests that remain.
        /// </summary>
        public int Remaining { get; private set; }

        /// <summary>
        /// When this bucket's limit resets.
        /// </summary>
        public DateTime ResetsAt { get; internal set; }

        /// <summary>
        /// The Id of the bucket this ratelimit belongs to.
        /// </summary>
        public string BucketId { get; internal set; }

        /// <summary>
        /// Whether this is a global ratelimit.
        /// </summary>
        public bool Global { get; }
        
        public RestBucket(int limit, int remaining, DateTime resetsAt, string bucketId, bool global)
        {
            Limit = limit;
            Remaining = remaining;
            ResetsAt = resetsAt;
            BucketId = bucketId;
            Global = global;
        }
        
        /// <summary>
        /// Whether this bucket can be used. True if Remaining > 0.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CanUseAsync()
        {
            try
            {
                await _wait.WaitAsync().ConfigureAwait(false);

                if (Remaining <= 0)
                    return ResetsAt < DateTime.Now;

                Remaining--;
                return true;
            }
            finally { _wait.Release(); }
        }

        /// <summary>
        /// Attempts to parse a rate limit bucket from the response headers.
        /// </summary>
        /// <param name="headers">The response headers.</param>
        /// <param name="result">The resulting rate limit bucket.</param>
        /// <returns>true if a bucket was successfully parsed; otherwise, false.</returns>
        public static bool TryParse(HttpResponseHeaders headers, [NotNullWhen(true)] out RestBucket? result)
        {
            result = null;

            if (!headers.TryGetValues("X-RateLimit-Limit", out var rawLimit))
                return false;

            if (!int.TryParse(rawLimit.SingleOrDefault(), out var limit))
                return false;

            if (!headers.TryGetValues("X-RateLimit-Remaining", out var rawRemaining))
                return false;

            if (!int.TryParse(rawRemaining.SingleOrDefault(), out var remaining))
                return false;

            if (!headers.TryGetValues("X-RateLimit-Reset", out var rawReset))
                return false;

            if (!int.TryParse(rawReset.SingleOrDefault(), out var resetsAtEpoch))
                return false;

            if (!headers.TryGetValues("X-RateLimit-Bucket", out var rawBucket))
                return false;

            var id = rawBucket.SingleOrDefault();
            
            if (id is null)
                return false;

            var isGlobal = headers.Contains("X-RateLimit-Global");
            var resetsAt = DateTime.UnixEpoch + TimeSpan.FromSeconds(resetsAtEpoch);

            result = new RestBucket(limit, remaining, resetsAt, id, isGlobal);
            return true;
        }
    }
}