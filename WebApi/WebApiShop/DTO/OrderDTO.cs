namespace DTO
{
    public record OrderDTO
    (
        int OrderId,
        int UserId,
        bool CurrentStatus,
        DateOnly? OrderDate,
        double OrdersSum,
        string? OriginalSchema,
        string? GeneratedCode,
        List<OrderItemDTO>? OrdersItems
    );
}
