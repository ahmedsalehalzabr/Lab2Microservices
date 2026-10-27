namespace WarehouseService;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Status { get; set; } = "Available"; // Available, Reserved, Ordered
}