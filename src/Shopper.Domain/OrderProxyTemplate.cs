using Stateless;

namespace Shopper.Domain;

// dotnet add package Stateless
// https://github.com/dotnet-state-machine/stateless

public class OrderProxyTemplate : Order
{
    private readonly StateMachine<OrderStatus, OrderTrigger> machine;

    public OrderProxyTemplate(OrderStatus status = OrderStatus.Pending) : base(status)
    {
        // Initial state
        // machine = new StateMachine<OrderStatus, OrderTrigger>(OrderStatus.Pending);

        // External State Storage
        machine = new StateMachine<OrderStatus, OrderTrigger>(
            () => status, 
            s => status = s);

        machine.Configure(OrderStatus.Pending)            
            .Permit(OrderTrigger.Confirm, OrderStatus.Completation)
            .Permit(OrderTrigger.Cancel, OrderStatus.Cancelled);      

        machine.Configure(OrderStatus.Completation)
            .Permit(OrderTrigger.Confirm, OrderStatus.Sent)
            .Permit(OrderTrigger.Cancel, OrderStatus.Cancelled);

        machine.Configure(OrderStatus.Sent)
            .OnEntry(() => Console.WriteLine($"Your order {Id} is ready to send."), "Send email")
            .Permit(OrderTrigger.Confirm, OrderStatus.Delivered)            
            .OnExit(()=> Console.WriteLine($"Your order {Id} was sent."), "Send email");

        machine.Configure(OrderStatus.Delivered)
            .Permit(OrderTrigger.Confirm, OrderStatus.Completed)
            .Permit(OrderTrigger.Cancel, OrderStatus.Cancelled);

        machine.Configure(OrderStatus.Completed);

        machine.OnTransitioned(transition =>
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{transition.Source} -> {transition.Destination}");
            System.Console.ResetColor();
            
        });

    }

    // http://www.webgraphviz.com
    public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());
    public override OrderStatus Status { get => machine.State;  }
    public override bool CanConfirm => machine.CanFire(OrderTrigger.Confirm);

    public override void Confirm() => machine.Fire(OrderTrigger.Confirm);
    public override void Cancel() => machine.Fire(OrderTrigger.Cancel);
    public override bool CanCancel => machine.CanFire(OrderTrigger.Cancel);

    public override string ToString() => base.ToString() + Graph;
}

public enum OrderTrigger
{
    Confirm,
    Cancel
}