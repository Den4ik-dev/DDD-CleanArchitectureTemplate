using Application.Common.Interfaces;
using Domain.Category.ValueObject;
using Domain.Product;
using Domain.Product.ValueObject;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductId>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ProductId>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _context.Products.SingleOrDefaultAsync(p => p.Name == request.Name) != null)
            return Result.Fail<ProductId>("Product with name already exists");

        if (await _context.Categories.FindAsync(CategoryId.Create(request.CategoryId)) == null)
            return Result.Fail<ProductId>("Category with id not found");

        Product product = _mapper.Map<Product>(request);

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
