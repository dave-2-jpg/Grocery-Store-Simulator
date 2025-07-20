using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO.Pipelines;

class Program {
public static int itemCounter = 0;
public static int week = 1;
public static double rent = 10;
public static double tax = 0.13;
public static int balance = 100;

public static int sales = 0;
public static void Main(string[] args) {
Console.Clear();
//PrintIntro();
List<Tools> shop = new List<Tools>();
//Creating Items
while (itemCounter <= 2) {

try {
Console.WriteLine($"Item({itemCounter + 1}): ");
string input = Console.ReadLine();
if (string.IsNullOrEmpty(input)) {
    throw new Exception("Error: Item must have a name!");
}
if (ContainsNumbers(input)) {
throw new Exception("Error: Item cannot contain numbers!");
}

foreach (Tools item in shop.Where(n => n != null)) {
if (item.name.Equals(input, StringComparison.OrdinalIgnoreCase)) {
    throw new Exception("You already have that in your shop!");
}
}

Tools tool = new Tools {name = input};
shop.Add(tool);
Console.WriteLine($"|Added!|");
Thread.Sleep(500);
itemCounter++;
rent += 20;
Console.Clear();

}

catch (Exception e) {
    Console.WriteLine($"Error: {e.Message}");
}

}
Console.Clear();

int itemPCounter = 0;
//Pricing Items
while (itemPCounter <= 2) {
    try {
        Console.WriteLine($"Enter Price of: {shop[itemPCounter].name}");
        int price = Convert.ToInt32(Console.ReadLine());
        if (ContainsLetters(price.ToString())) {
            throw new Exception("Invalid Price!");

        }
        if (string.IsNullOrEmpty(price.ToString())) {
            throw new Exception("Must enter a price!");
        }
        shop[itemPCounter].Cost = price;
        Console.WriteLine($"|Priced!|");
        Thread.Sleep(500);
        itemPCounter++;
        Console.Clear();
    }
    catch (Exception e) 
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}
//print shop
//game

bool gameEnd = false;

while (!gameEnd) {
PrintShop(shop, balance, week);
Console.WriteLine("Action: | (a)dd item | (r)emove item | (s)imulate week | (q)uit game|");
string response = Convert.ToString(Console.ReadLine());
response = response.ToLower();
    switch (response) {
    case"a":
    if (itemCounter < 10) {
        AddItem(shop, itemCounter);
        itemCounter++;
    }
    else {
        Console.WriteLine("|SHOP IS FULL!|");
        Thread.Sleep(1000);
        Console.Clear();
    }
    
    break;
    case "r":
    Console.Clear();
    PrintShop(shop);
    if (itemCounter >= 1) {
        RemoveItem(shop);
    }
    else {
        Console.WriteLine("|SHOP IS EMPTY!|");
        Thread.Sleep(1000);
        Console.Clear();
    }
    
    
    break;
     case"s":
     Console.Clear();
     for (int i = 0; i < 3; i++) {
        Console.Write("Simulating");
        Thread.Sleep(50);
        Console.Clear();
        Console.Write("Simulating.");
        Thread.Sleep(50);
        Console.Clear();
        Console.Write("Simulating..");
        Thread.Sleep(50);
        Console.Clear();
        Console.Write("Simulating...");
        Thread.Sleep(50);
        Console.Clear();
        
     }
     SimulationAI(shop);
     week++;
     tax = tax * 1.02;
    //simulate
    break;
     case"q":
     gameEnd = true;
    
    break;
    
    default:
    Console.WriteLine("Invalid!");
    break;
}
if (week % 7 == 0) 
{
    Console.Clear();
    double dblaance = Convert.ToDouble(balance);
    double result = dblaance * tax;
    int intresult = Convert.ToInt32(result);
    balance -= intresult;
    Console.WriteLine($"Weekly Tax Report: ${intresult}");
    Thread.Sleep(5000);
}
if (week % 14 == 0) 
{
    Console.Clear();
    double inflation = rent * 1.5;
    int intInflation = Convert.ToInt32(inflation);
    balance -= intInflation;
    Console.WriteLine($"Rent Report: ${intInflation}");
    Thread.Sleep(5000);
}
if (balance <= 0) {
    gameEnd = true;
}
}
if (gameEnd) {
    Console.Clear();
    Console.WriteLine($"Lifetime Item Sold: {sales}");
    Console.WriteLine($"Days in buisness: {week * 7}");
    Thread.Sleep(10000);
}


}
public static void SimulationAI(List<Tools> list) {
    Random random = new Random();
    int hit = random.Next(100);
    switch (hit) {
        case int n when n < 15: //one item bought
        int itemNumber = random.Next(0, list.Count);
        Tools item = list[itemNumber];
        if (item.stock > 0) 
        {
            int items = random.Next(1, item.stock);
            item.stock -= items;
            sales += items;
            balance += item.Cost * items;
            string text = $"A costumer has bought {item.name} x{items}!";
            string[] _text = Regex.Split(text, @"\s");
            foreach(string word in _text) {
                Console.Write(word + " ");
                Thread.Sleep(100);
                Thread.Sleep(500);
            }
        }
        else 
        {
            string text = $"A costumer tried to buy {item.name}, however it wasn't in stock!";
            string[] _text = Regex.Split(text, @"\s");
            foreach(string word in _text) {
                Console.Write(word + " ");
                Thread.Sleep(100);
                Thread.Sleep(500);
            }
        }
       
        break;
        case int n when n < 100 && n > 66: //two items bought
         int itemNumber1 = random.Next(0, list.Count);
         int itemNumber2 = random.Next(0, list.Count);
         while (itemNumber1 == itemNumber2) {
            itemNumber2 = random.Next(0, list.Count);
         }

        Tools item1 = list[itemNumber1];
        Tools item2 = list[itemNumber2];
        if (item1.stock > 0 && item2.stock > 0) 
        {
            int items1 = random.Next(1, item1.stock);
            item1.stock -= items1;
            balance += item1.Cost * item1.stock;
            //second
            int items2 = random.Next(1, item2.stock);
            item2.stock -= items2;
            balance += item1.Cost * items1;
            balance += item2.Cost * items2;
            //sales
            sales += items1 + items2;
            string text = $"A costumer has bought {item1.name} x{items1} and {item2.name} x{items2}!";
            string[] _text = Regex.Split(text, @"\s");
            foreach(string word in _text) {
                Console.Write(word + " ");
                Thread.Sleep(100);
                Thread.Sleep(500);
            }
        }
        else 
        {
            string text = $"A costumer tried to buy {item1.name} and {item2.name}, however one wasn't in stock!";
            string[] _text = Regex.Split(text, @"\s");
            foreach(string word in _text) {
                Console.Write(word + " ");
                Thread.Sleep(100);
                Thread.Sleep(500);
            }
        }
      
        break;
        default:
        string text2 = $"No customers came by this week!";
            string[] _text2 = Regex.Split(text2, @"\s");
            foreach(string word in _text2) {
                Console.Write(word + " ");
                Thread.Sleep(170);
                Thread.Sleep(500);
            }
        
        break;
    } 
}
public static void RemoveItem(List<Tools> array) {
    Console.WriteLine("");
    Console.WriteLine("Item name: ");
    string deletedItem = Console.ReadLine();
     int removedCount = array.RemoveAll(tool => tool.name.Equals(deletedItem, StringComparison.OrdinalIgnoreCase));
     if (removedCount > 0) {
        Console.WriteLine($"Removed {deletedItem}");
        itemCounter--;
        rent -= 20;
     }
     else {
        Console.WriteLine($"'{deletedItem}' is not in your shop!");
     }
     Thread.Sleep(500);
     Console.Clear();
}
public static void AddItem(List<Tools> array, int counter) {
Console.Clear();
bool itemNamed = false;

//name
while (!itemNamed) {
try {
    Console.WriteLine($"Item({counter + 1}): ");
    string input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) {
    throw new Exception("Error: Item must have a name!");
}
if (ContainsNumbers(input)) {
throw new Exception("Error: Item cannot contain numbers!");
}
foreach (Tools item in array.Where(n => n != null)) {
if (item.name.Equals(input, StringComparison.OrdinalIgnoreCase)) {
    throw new Exception("You already have that in your shop!");
}
}
Tools tool = new Tools {name = input};
array.Add(tool);

Console.WriteLine($"|Added!|");
Thread.Sleep(500);
itemNamed = true;
Console.Clear();
}
catch (Exception e) 
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}

bool itemPriced = false;
//price
while (!itemPriced) {
try {
        Console.WriteLine($"Enter Price of: {array[counter].name}");
        int price = Convert.ToInt32(Console.ReadLine());
        if (ContainsLetters(price.ToString())) {
            throw new Exception("Invalid Price!");

        }
        if (string.IsNullOrEmpty(price.ToString())) {
            throw new Exception("Must enter a price!");
        }
        array[counter].Cost = price;
        Console.WriteLine($"|Priced!|");
        Thread.Sleep(500);
        itemPriced = true;
        Console.Clear();
    }
    catch (Exception e) 
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}
rent += 20;

    
}
public static bool ContainsNumbers(string stuff) {

return Regex.IsMatch(stuff, @"\d");

}
public static bool ContainsLetters(string stuff) {

return Regex.IsMatch(stuff, @"\D");

}
public static void PrintShop(List<Tools> array, int balance, int week) {

var initializedItems = array.Where(n => n != null);

Console.WriteLine($"Week: {week}");
Console.WriteLine("--------------------");
foreach(Tools tool in initializedItems) {
Console.WriteLine($"|{tool.name}: ${tool.Cost}| Stock: {tool.stock}");
}
Console.WriteLine("--------------------");
Console.WriteLine($"Balance: ${balance}");
}
public static void PrintShop(List<Tools> array) {

var initializedItems = array.Where(n => n != null);

Console.WriteLine("xxxxxxxxxxxxxxxxxxxx");
foreach(Tools tool in initializedItems) {
Console.WriteLine($"|{tool.name}: ${tool.Cost}| Stock: {tool.stock}");
}
Console.WriteLine("xxxxxxxxxxxxxxxxxxxx");
}
public static void PrintIntro() {
    Console.WriteLine("----------------------");
Console.WriteLine("GROCERY TYCHOON");
Console.WriteLine("----------------------");
Thread.Sleep(10000);
Console.Clear();
Console.WriteLine($"Tax occurs every 7 days: {tax * 100}%");
Thread.Sleep(3000);
Console.WriteLine($"It increases each week by: 2% ");
Thread.Sleep(10000);
Console.Clear();
Console.WriteLine($"Rent is  ${rent}");
Console.WriteLine("Rent INCREASES when you add items to your shop.");
Thread.Sleep(3000);
Console.WriteLine("...and DECREASES when you remove items to your shop.");
Thread.Sleep(3000);
Console.WriteLine("Inflation causes your rent to increase by 50% every 14 weeks");
Thread.Sleep(10000);
Console.Clear();
Console.WriteLine("CAN YOU AVOID BANKRUPCY?");
Thread.Sleep(5000);
Console.Clear();
}
public class Item {
public string name {get; set;}
public int stock = 5;
private int cost;
public int Cost {
    get {return cost;} 
set {
if (value > 1000) {
    cost = 999;
}
else {
    cost = value;
}
}}

}

public class Tools : Item {

}

}


