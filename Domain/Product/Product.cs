using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.Events;
using Domain.Product.ValueObject;

namespace Domain.Product;

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; }
    public string Description { get; }
    public Price Price { get; }
    public CategoryId CategoryId { get; }

    private Product()
        : base(ProductId.CreateUnique()) { }

    private Product(
        ProductId productId,
        string name,
        string description,
        Price price,
        CategoryId categoryId
    )
        : base(productId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }

    public static Product Create(
        string name,
        string description,
        Price price,
        CategoryId categoryId
    )
    {
        var product = new Product(ProductId.CreateUnique(), name, description, price, categoryId);

        product.AddDomainEvent(new CreateProductDomainEvent(product.Id, product.CategoryId));

        return product;
    }
}
