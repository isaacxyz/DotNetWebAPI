using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Going.Plaid.Sandbox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DotNetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaidController : Controller
    {
        private readonly ILogger<PlaidController> _logger;
        private readonly IOptions<PlaidCredentials> _credentials;
        private readonly PlaidClient _client;

        public PlaidController(ILogger<PlaidController> logger, IOptions<PlaidCredentials> credentials, PlaidClient client)
        {
            _logger = logger;
            _credentials = credentials;
            _client = client;
        }

        [HttpPost("token")]
        public async Task<IActionResult> PlaidToken()
        {
            _logger.LogInformation("Testing Plaid client...");

            var publicTokenResult = await _client.SandboxPublicTokenCreateAsync(
                new SandboxPublicTokenCreateRequest
                {
                    InstitutionId = "ins_3",
                    InitialProducts = new[]
                    {
                        Products.Auth,
                        Products.Balance,
                        Products.Investments,
                        Products.Transactions,
                    },
                });

            var result = await _client.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest()
                {
                    PublicToken = publicTokenResult.PublicToken,
                });
            _credentials.Value.AccessToken = result.AccessToken;
            _logger.LogInformation($"access_token: '{result.AccessToken}'");

            return Ok(result);
        }

        [HttpGet("balances")]
        public async Task<IActionResult> GetBalance()
        {
            var result = await _client.AccountsBalanceGetAsync(
                new()
                {
                    AccessToken = _credentials.Value.AccessToken,
                });
            

            return Ok(result);
        }

        [HttpGet("investments")]
        public async Task<IActionResult> GetInvestments()
        {
            var result = await _client.InvestmentsTransactionsGetAsync(
                new()
                {
                    AccessToken = _credentials.Value.AccessToken,
                });

            return Ok(result);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var result = await _client.TransactionsGetAsync(
                new()
                {
                    AccessToken = _credentials.Value.AccessToken,
                });

            return Ok(result);
        }

        [HttpGet("total-debt")]
        public async Task<IActionResult> GetTotalDebt()
        {
            var balanceResult = await _client.AccountsBalanceGetAsync(
                new()
                {
                    AccessToken = _credentials.Value.AccessToken,
                });

            //decimal? totalDebt = 0;
            //foreach (var account in balanceResult.Accounts)
            //{
            //    if (account.Type is AccountType.Credit or AccountType.Loan)
            //    {
            //        totalDebt += account.Balances.Current;
            //    }
            //}

            var totalDebt = balanceResult.Accounts.Where(a => a.Type is AccountType.Credit or AccountType.Loan).Sum(a => a.Balances.Current);

            return Ok(totalDebt);
        }
    }


    public class PlaidCredentials : PlaidOptions
    {
        public string? AccessToken { get; set; }
    }

    public enum TestEnums
    {
        EnumOne,
        EnumTwo,
        EnumThree,
        EnumFour,
    }
}