using HomeAccounting.Domain.Services.Abstraction;
using MonoBankApi.Implements.Requests;
using MonoBankApi.Models.Responses;

namespace HomeAccounting.Domain.Services.Realization
{
    public class MonoApiService : IMonoApiService
    {
        private readonly MonoApiClient _client;
        private readonly IUserService _userService;

        public MonoApiService(MonoApiClient client, IUserService userService)
        {
            _client = client;
            _userService = userService;
        }

        public async Task<ICollection<StatementResponse>> ReturnStatementAsync(Guid currentUserId, DateTime from, DateTime to, CancellationToken ct, string acc = "0")
        {
            var token = (await _userService.GetPureUserAsync(currentUserId, ct)).MonobankToken;

            if (token is null)
            {
                throw new InvalidOperationException();
            }
            
            return await _client.HttpGetAsync<ICollection<StatementResponse>>(new StatementRequest(from, to, acc), token);
        }
    }
}