Random random = new();

#region Pokemon

PokemonClass pikachu = new()
{
    Name = "Pikachu",
    Type = PokemonType.Electric,
    Health = 35,
};

PokemonClass charizard = new()
{
    Name = "Charizard",
    Type = PokemonType.Fire | PokemonType.Flying,
    Health = 78,
};

PokemonClass oddish = new()
{
    Name = "Oddish",
    Type = PokemonType.Grass | PokemonType.Poison,
    Health = 45,
};

#endregion

#region Items

ItemClass potion = new()
{
    Name = "Potion",
    Action = (IHealthHaver healthHaver) =>
    {
        healthHaver.Health += 5;
    },
};

#endregion

Player player;

Action[] playerActions = [
    () => {
        var damage = random.Next(0, 5);

        if (damage > 0)
        {
            Console.WriteLine($"Your Pokemon attacks and deals {damage} damage.");
            return;
        }

        Console.WriteLine($"Your Pokemon misses!");
    },
    () => {
    },
    () => {
        Console.WriteLine("You chose to defend.");
    },
];

PokemonClass currentPokemon;

while (true)
{
    currentPokemon = pokemonTypes[random.Next(0, pokemonTypes.Length)];

    Console.WriteLine($"You encountered a wild {currentPokemon.Name}!");

    while (true)
    {
        Console.WriteLine("1: Fight\n2: Item\n3: Defend");
        Console.Write("Enter your choice: ");

        int choice = 0;
        string? input = Console.ReadLine();
        bool shouldContinue = input is null
            || !int.TryParse(input, out choice)
            || choice < 1 || choice > 3;

        if (shouldContinue)
        {
            Console.WriteLine("Didn't quite get that.");
            continue;
        }

        playerActions[choice - 1].Invoke();
    }
}

#region Types

interface IHealthHaver
{
    public int Health { get; set; }
}

enum PokemonType
{
    Bug = 1 << 0,
    Dark = 1 << 1,
    Dragon = 1 << 2,
    Electric = 1 << 3,
    Fairy = 1 << 4,
    Fighting = 1 << 5,
    Fire = 1 << 6,
    Flying = 1 << 7,
    Ghost = 1 << 8,
    Grass = 1 << 9,
    Ground = 1 << 10,
    Ice = 1 << 11,
    Normal = 1 << 12,
    Poison = 1 << 13,
    Psychic = 1 << 14,
    Rock = 1 << 15,
    Steel = 1 << 16,
    Water = 1 << 17,
}

struct PokemonClass : IHealthHaver
{
    public string Name;
    public PokemonType Type;
    public int Health { get; set; }
}

struct ItemClass
{
    public string Name;
    public int Count;
    public Action<IHealthHaver> Action;
}

interface IItemHaver
{
    public ItemClass GetItemForName(string name);
}

readonly struct Player : IItemHaver
{
    private readonly Dictionary<string, ItemClass> _inventory = [];

    public Player() { }

    readonly ItemClass IItemHaver.GetItemForName(string name) => _inventory[name];
}

#endregion
