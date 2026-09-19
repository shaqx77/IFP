using System;

namespace FunctionalDeliveryCalculator
{
    public enum DeliveryType
    {
        Pickup,
        Courier,
        DoorToDoor
    }

    public enum DeliveryZone
    {
        City,
        OutsideCity,
        Remote
    }

    public class Program
    {
        public static decimal ApplyRule(decimal price, Func<decimal, decimal> rule) => rule(price);

        public static decimal RoundPrice(decimal price) => Math.Round(price, 2, MidpointRounding.AwayFromZero);

        public static void Main(string[] args)
        {
            Console.WriteLine("=== Delivery Cost Calculator ===");

            // Ввод и валидация базовой цены
            Console.Write("Enter base delivery price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal basePrice) || basePrice < 0)
            {
                Console.WriteLine("Error: Invalid base price. Must be a non-negative number.");
                return;
            }

            // Ввод и валидация количества товаров
            Console.Write("Enter number of items: ");
            if (!int.TryParse(Console.ReadLine(), out int itemCount) || itemCount < 1)
            {
                Console.WriteLine("Error: Invalid item count. Must be a positive integer.");
                return;
            }

            // Ввод и валидация типа доставки
            Console.Write("Enter delivery type (Pickup, Courier, DoorToDoor): ");
            if (!Enum.TryParse<DeliveryType>(Console.ReadLine(), true, out var deliveryType))
            {
                Console.WriteLine("Error: Invalid delivery type.");
                return;
            }

            // Ввод и валидация зоны доставки
            Console.Write("Enter delivery zone (City, OutsideCity, Remote): ");
            if (!Enum.TryParse<DeliveryZone>(Console.ReadLine(), true, out var deliveryZone))
            {
                Console.WriteLine("Error: Invalid delivery zone.");
                return;
            }

            // Ввод и валидация статуса экспресс доставки
            Console.Write("Is express delivery? (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out bool isExpress))
            {
                Console.WriteLine("Error: Invalid express status. Use 'true' or 'false'.");
                return;
            }

            // Вызов чистой логики расчёта
            decimal finalPrice = CalculateTotalDeliveryCost(basePrice, itemCount, deliveryType, deliveryZone, isExpress);

            // Вывод результата
            Console.WriteLine($"\nFinal Delivery Cost: {finalPrice:F2}");
        }

        // 4. Чистая функция расчёта стоимости
        public static decimal CalculateTotalDeliveryCost(
            decimal basePrice,
            int itemCount,
            DeliveryType type,
            DeliveryZone zone,
            bool isExpress)
        {
            decimal currentPrice = basePrice;

            // Расчёт надбавки за количество предметов
            Func<decimal, decimal> itemsRule = price =>
            {
                if (itemCount >= 8) return price * 1.20m;
                if (itemCount >= 4) return price * 1.10m;
                return price;
            };

            // Расчёт надбавки за тип доставки
            Func<decimal, decimal> typeRule = price => type switch
            {
                DeliveryType.Pickup => price * 0.80m,
                DeliveryType.DoorToDoor => price * 1.15m,
                _ => price // Courier
            };

            // Применение правил через функцию высшего порядка ApplyRule
            currentPrice = ApplyRule(currentPrice, itemsRule);
            currentPrice = ApplyRule(currentPrice, typeRule);

            // Использование Func
            Func<decimal, decimal> zoneRule = price => CalculateZoneAdjustment(price, zone);
            currentPrice = ApplyRule(currentPrice, zoneRule);

            // Экспресс     доставка
            Func<decimal, decimal> expressRule = price => isExpress ? price * 1.30m : price;
            currentPrice = ApplyRule(currentPrice, expressRule);

            // Округление результата
            return RoundPrice(currentPrice);
        }

       
        public static decimal CalculateZoneAdjustment(decimal price, DeliveryZone zone) => zone switch
        {
            DeliveryZone.OutsideCity => price * 1.25m,
            DeliveryZone.Remote => price * 1.50m,
            _ => price // City
        };
    }
}