// DESAFIO: Processamento de Pedidos - SOLUÇÃO: Padrão Template Method

using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    public abstract class OrderProcessor
    {
        public void ProcessOrder(string id, List<string> items, decimal amount)
        {
            Console.WriteLine($"\n=== Processando Pedido ===");
            if (!Validate(id, amount)) return;
            Console.WriteLine("✓ Validado");

            Console.WriteLine("Verificando estoque...");
            foreach (var i in items) Console.WriteLine($"  → {i}: Disponível");
            Console.WriteLine("✓ Estoque OK");

            var total = CalculateTotal(amount, items);
            Console.WriteLine($"Total: R$ {total:N2}");

            ProcessPayment(id, total);
            Console.WriteLine("Separando itens...");
            ScheduleShipping(id);
            NotifyCustomer(id, total);
            Console.WriteLine("✅ Pedido processado!\n");
        }

        protected abstract bool Validate(string id, decimal amount);
        protected abstract decimal CalculateTotal(decimal amount, List<string> items);
        protected abstract void ProcessPayment(string id, decimal total);
        protected abstract void ScheduleShipping(string id);
        protected abstract void NotifyCustomer(string id, decimal total);
    }

    public class OnlineOrderProcessor : OrderProcessor
    {
        protected override bool Validate(string id, decimal amount) => !string.IsNullOrEmpty(id);
        protected override decimal CalculateTotal(decimal amount, List<string> items) => amount + 15m;
        protected override void ProcessPayment(string id, decimal total) => Console.WriteLine("[Online] Pagamento cartão OK");
        protected override void ScheduleShipping(string id) => Console.WriteLine("[Online] Envio Correios");
        protected override void NotifyCustomer(string id, decimal total) => Console.WriteLine("[Online] Email enviado");
    }

    public class WholesaleOrderProcessor : OrderProcessor
    {
        protected override bool Validate(string id, decimal amount) => !string.IsNullOrEmpty(id) && amount >= 1000m;
        protected override decimal CalculateTotal(decimal amount, List<string> items) => amount * 0.90m;
        protected override void ProcessPayment(string id, decimal total) => Console.WriteLine("[Atacado] Boleto gerado");
        protected override void ScheduleShipping(string id) => Console.WriteLine("[Atacado] Coleta agendada");
        protected override void NotifyCustomer(string id, decimal total) => Console.WriteLine("[Atacado] Email + SMS");
    }

    public class MarketplaceOrderProcessor : OrderProcessor
    {
        protected override bool Validate(string id, decimal amount) => !string.IsNullOrEmpty(id);
        protected override decimal CalculateTotal(decimal amount, List<string> items) => amount;
        protected override void ProcessPayment(string id, decimal total) => Console.WriteLine("[Marketplace] Split payment");
        protected override void ScheduleShipping(string id) => Console.WriteLine("[Marketplace] Envio vendedor");
        protected override void NotifyCustomer(string id, decimal total) => Console.WriteLine("[Marketplace] Cliente + Vendedor");
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Processamento de Pedidos (Template Method) ===\n");

            var items = new List<string> { "Notebook", "Mouse" };

            new OnlineOrderProcessor().ProcessOrder("CUST001", items, 2500m);
            new WholesaleOrderProcessor().ProcessOrder("COMP001", items, 5000m);
            new MarketplaceOrderProcessor().ProcessOrder("SELL001", items, 3000m);

        }
    }
}
