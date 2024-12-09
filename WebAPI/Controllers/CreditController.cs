using AutoMapper;
using Domain.Interfaces;
using Domain.Transactions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditController : ControllerBase
    {
        private readonly ILogger<CreditController> _logger;
        private readonly IMapper _mapper;
        private readonly IBalanceService _balanceService;

        public CreditController(
            ILogger<CreditController> logger,
            IMapper mapper,
            IBalanceService balanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _balanceService = balanceService;
        }

        [HttpPost]
        public async Task<IActionResult> PostCreditTransaction(CreditTransactionModel request)
        {
            _logger.LogInformation($"PostCreditTransaction:{request}");
            CreditTransaction transaction = _mapper.Map<CreditTransaction>(request);
            ITransactionResult result = await _balanceService.CreditTransaction(transaction);
            TransactionModel model = _mapper.Map<TransactionModel>(result);
            return Ok(model);
        }
    }
}
