using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.ValueObject;

namespace Domain.Category;

public class Category : AggregateRoot<CategoryId>
{
    private readonly List<ProductId> _productIds;

    public string Name { get; }
    public IReadOnlyList<ProductId> ProductIds => _productIds;

    private Category()
        : base(CategoryId.CreateUnique()) { }

    private Category(CategoryId categoryId, string name)
        : base(categoryId) { }

    public static Category Create(string name)
    {
        return new Category(CategoryId.CreateUnique(), name);
    }

    public void AddProductId(ProductId productId)
    {
        _productIds.Add(productId);
    }
}
