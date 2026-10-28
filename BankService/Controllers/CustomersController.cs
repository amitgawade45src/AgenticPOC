using BankService.Application.DTOs;
using BankService.Application.UseCases.Commands.CustomerManagement;
using BankService.Application.UseCases.Queries.CustomerManagement;
using BankService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Controllers;

/// <summary>
/// Customer Management bounded context: register, update, deactivate and query customers.
/// </summary>
[ApiController]
[Route("api/customers")]
[Tags("Customers")]
[Produces("application/json")]
public sealed class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Registers a new customer.</summary>
    /// <remarks>Raises <c>CustomerRegistered</c>. Returns the created customer profile.</remarks>
    [HttpPost]
    [EndpointSummary("Register customer")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CustomerUpsertRequest request, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(new RegisterCustomerCommand(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.MobileNumber,
            request.Address), cancellationToken);

        return Ok(customer);
    }

    /// <summary>Updates an existing customer's profile.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpPut("{customerId:guid}")]
    [EndpointSummary("Update customer")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid customerId, [FromBody] CustomerUpsertRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _mediator.Send(new UpdateCustomerCommand(
                customerId,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Email,
                request.MobileNumber,
                request.Address), cancellationToken);

            return Ok(customer);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Deactivates a customer.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpPost("{customerId:guid}/deactivate")]
    [EndpointSummary("Deactivate customer")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid customerId, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _mediator.Send(new DeactivateCustomerCommand(customerId), cancellationToken);
            return Ok(customer);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Gets a customer by identifier.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpGet("{customerId:guid}")]
    [EndpointSummary("Get customer by id")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery(customerId), cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    /// <summary>Searches customers by name, email or mobile number.</summary>
    /// <param name="searchTerm">Optional case-insensitive search term. Empty returns all customers.</param>
    [HttpGet]
    [EndpointSummary("Search customers")]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        var customers = await _mediator.Send(new SearchCustomersQuery(searchTerm ?? string.Empty), cancellationToken);
        return Ok(customers);
    }

    /// <summary>Gets the customer profile.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpGet("{customerId:guid}/profile")]
    [EndpointSummary("Get customer profile")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(new GetCustomerProfileQuery(customerId), cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }
}
