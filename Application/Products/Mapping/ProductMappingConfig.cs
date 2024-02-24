using Application.Products.Commands.CreateProduct;
using Domain.Category.ValueObject;
using Domain.Product;
using Domain.Product.ValueObject;
using Mapster;

namespace Application.Products.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CreateProductCommand, Product>()
            .MapWith(
                src =>
                    Product.Create(
                        src.Name,
                        src.Description,
                        Price.Create(src.Price),
                        CategoryId.Create(src.CategoryId)
                    )
            );
    }
}
