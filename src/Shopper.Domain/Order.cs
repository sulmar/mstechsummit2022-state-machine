namespace Shopper.Domain;
public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public virtual OrderStatus Status { get; set; }

    public virtual bool CanConfirm => Status == OrderStatus.Pending || Status == OrderStatus.Completation || Status == OrderStatus.Sent || Status == OrderStatus.Delivered;
    public virtual bool CanCancel => Status == OrderStatus.Pending || Status == OrderStatus.Delivered;
    public Order(OrderStatus status = OrderStatus.Pending)
    {
        Status = status;
        OrderDate = DateTime.Now;
        Id = Guid.NewGuid();
    }    

    public virtual void Confirm()
    {
        if (Status == OrderStatus.Pending)
        {
            Status =  OrderStatus.Completation;
        }
        else if (Status == OrderStatus.Completation)
        {
            Console.WriteLine($"Your order {Id} is ready to send.");
            Status = OrderStatus.Sent;
            Console.WriteLine($"Your order {Id} was sent.");
        }
        else if (Status == OrderStatus.Sent)
        {
            Status = OrderStatus.Delivered;
        }
        else if (Status == OrderStatus.Delivered)
        {
            Status = OrderStatus.Completed;
        }
    }

    public virtual void Cancel()
    {
        if (CanCancel)
            Status = OrderStatus.Cancelled;
    }

    public override string ToString() => $"Order {Id} created on {OrderDate}{Environment.NewLine}";
}

public enum OrderStatus
{
    Pending,    
    Completation,
    Sent,
    Delivered,
    Completed,
    Cancelled
}


