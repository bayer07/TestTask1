using AutoMapper;
using Domain.Transactions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Domain.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebitController : ControllerBase
    {
        private readonly ILogger<CreditController> _logger;
        private readonly IMapper _mapper;
        private readonly IBalanceService _balanceService;

        public DebitController(
            ILogger<CreditController> logger,
            IMapper mapper,
            IBalanceService balanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _balanceService = balanceService;
        }

        [HttpPost]
        public async Task<IActionResult> PostDebitTransaction(DebitTransactionModel request)
        {
            _logger.LogInformation($"PostDebitTransaction:{request}");
            DebitTransaction transaction = _mapper.Map<DebitTransaction>(request);
            ITransactionResult result = await _balanceService.DebitTransaction(transaction);
            TransactionModel model = _mapper.Map<TransactionModel>(result);
            return Ok(model);
        }
    }
}
