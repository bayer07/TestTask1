using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<CreditController> _logger;
        private readonly IMapper _mapper;
        private readonly IBalanceService _balanceService;

        public BalanceController(
            ILogger<CreditController> logger,
            IMapper mapper,
            IBalanceService balanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _balanceService = balanceService;
        }

        [HttpPost]
        public async Task<IActionResult> Balance(Guid transactionId)
        {
            _logger.LogInformation($"Balance:{transactionId}");
            ITransactionResult transaction = await _balanceService.GetTransaction(transactionId);
            BalanceModel model = _mapper.Map<BalanceModel>(transaction);
            return Ok(model);
        }
    }
}
