using Api.Common;
using Application.Products.Commands.CreateProduct;
using Domain.Product.ValueObject;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/products")]
public class ProductController : ApiController
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IResult> CreateProduct(CreateProductCommand command)
    {
        Result<ProductId> result = await _sender.Send(command);

        if (result.IsFailed)
            return Problem(result.Errors);

        ProductId productId = result.Value;

        return Results.Ok(productId.Value);
    }
}
