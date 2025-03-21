namespace Core.Enitities;

public class DeliveryMethod : BaseEntity
{
  public required string ShortName { get; set; }
  public required string DeliveryTime { get; set; }
  public required string Description { get; set; }
  public double Price { get; set; }
}