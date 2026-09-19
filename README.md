# Functional Delivery Calculator

## Altynbayev Yerassyl

## Run Instructions
1. Open the solution in Visual Studio.
2. Build the project (Ctrl + Shift + B) to ensure there are no compilation errors.
3. Run the application (F5 or the green Play button).
4. The console window will appear. Follow the on-screen prompts to enter the base price, number of items, delivery type, delivery zone, and express status.
5. To test invalid input, type letters instead of numbers or type non-existent delivery options.

---

## Answers to Questions

1. Which parts of your program handle user input and output?
The `Main` method acts as the I/O boundary. It reads inputs using `Console.ReadLine()`, handles validation errors, displays error messages, and prints the final result using `Console.WriteLine()`. No other methods interact with the Console.

2. Which functions perform only delivery price calculations?
The pure calculation methods are `CalculateTotalDeliveryCost`, `CalculateZoneAdjustment`, `RoundPrice`, and the higher-order function `ApplyRule`. The internal `Func<decimal, decimal>` lambda expressions also exclusively perform mathematical logic.

3. How is Func<...> used to apply delivery pricing rules?
`Func<decimal, decimal>` delegates are created for each step of the calculation (item count adjustments, type modifiers, zone multipliers, and express markup). These delegates encapsulate individual rules and are passed to the higher-order method `ApplyRule` to transform the price step-by-step.

4. Why is TryParse useful when processing delivery data entered by the user?
`TryParse` safely converts string inputs into target types (decimal, int, bool, Enum) without throwing unhandled runtime exceptions if the user enters invalid, empty, or wrongly-typed data. It returns `false` on failure, allowing the program to do an early return and display a clear error message instead of crashing.

---

## Documented Test Cases

### Test Case 1: Standard Courier Delivery
* **Input**: 
  - Base price: 1000
  - Number of items: 2 (no adjustment)
  - Type: Courier (no adjustment)
  - Zone: City (no adjustment)
  - Express: false (no adjustment)
* **Expected Output**: Final Delivery Cost: 1000.00
* **Result**: Pass

### Test Case 2: Maximum Modifiers
* **Input**:
  - Base price: 1000
  - Number of items: 5 (+10% -> 1100)
  - Type: DoorToDoor (+15% -> 1265)
  - Zone: OutsideCity (+25% -> 1581.25)
  - Express: true (+30% -> 2055.625)
* **Expected Output**: Final Delivery Cost: 2055.63 (rounded)
* **Result**: Pass

### Test Case 3: Bulk Pickup (Discount)
* **Input**:
  - Base price: 2000
  - Number of items: 10 (+20% -> 2400)
  - Type: Pickup (-20% -> 1920)
  - Zone: City (no adjustment)
  - Express: false
* **Expected Output**: Final Delivery Cost: 1920.00
* **Result**: Pass

### Test Case 4: Invalid Base Price (Negative or Text)
* **Input**: Base price: "-500" or "five"
* **Expected Output**: "Error: Invalid base price. Must be a non-negative number." Application exits.
* **Result**: Pass

### Test Case 5: Invalid Enum Type
* **Input**: 
  - Base price: 1000
  - Number of items: 1
  - Type: "Teleport" (invalid)
* **Expected Output**: "Error: Invalid delivery type." Application exits.
* **Result**: Pass

### Test Case 6: Invalid Boolean (Express status)
* **Input**:
  - Base price: 1000
  - Number of items: 1
  - Type: Courier
  - Zone: City
  - Express: "yes" (invalid, must be true/false)
* **Expected Output**: "Error: Invalid express status. Use 'true' or 'false'." Application exits.
* **Result**: Pass