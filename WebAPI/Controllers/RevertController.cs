using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevertController : ControllerBase
    {
        private readonly ILogger<CreditController> _logger;
        private readonly IMapper _mapper;
        private readonly IBalanceService _balanceService;

        public RevertController(
            ILogger<CreditController> logger,
            IMapper mapper,
            IBalanceService balanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _balanceService = balanceService;
        }

        [HttpPost]
        public async Task<IActionResult> RevertTransaction(Guid transactionId)
        {
            _logger.LogInformation($"Revert:{transactionId}");
            ITransactionResult transaction = await _balanceService.RevertTransaction(transactionId);
            RevertTransactionModel model = _mapper.Map<RevertTransactionModel>(transaction);
            return Ok(model);
        }
    }
}
