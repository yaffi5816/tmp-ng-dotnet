namespace DTO
{
    public record OrderItemDTO
    (
        int OrderItemId,
        int ProductId,
        int OrderId,
        int Quantity,
        ProductDTO? Product
    );
}
