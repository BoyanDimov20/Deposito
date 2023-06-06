﻿using Deposito.DB.Enums;
using Deposito.Web.Services;
using Deposito.Web.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Deposito.Web.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class DepositController : ControllerBase
{
    private readonly IDepositService _depositService;

    public DepositController(IDepositService depositService)
    {
        _depositService = depositService;
    }

    [HttpGet]
    public async Task<IEnumerable<DepositDto>> GetDeposits(double amount, Currency currency, int period, PayoutType payoutType)
    {
        return await _depositService.GetDeposits(amount, currency, period, payoutType);
    }

    [HttpGet]
    public async Task<CalculatedDepositDto> CalculateDeposit(string id, double amount)
    {
        return await _depositService.CalculateDeposit(id, amount);
    }
}