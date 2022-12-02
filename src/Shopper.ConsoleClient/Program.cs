using Shopper;
using Shopper.Domain;

Console.WriteLine("Hello, MS Tech Summit 2022!");

while (true)
{    
    var order = new Order();
    
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine(order);
    Console.ResetColor();

    order.Status.Dump();

    while (!(order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Completed))
    {
        if (order.CanConfirm)
            Console.Write("Confirm (Enter) ");

        if (order.CanCancel)
            Console.Write("(C)ancel ");

        var key = Console.ReadKey().Key;

        Console.WriteLine();

        if (key == ConsoleKey.Enter)
            order.Confirm();

        if (key == ConsoleKey.C)
            order.Cancel();

        order.Status.Dump();

    }

    Console.Clear();

}







