namespace PokemonGame;

public class Pokemon(Pokemon.PokemonDescription description)
{
    public readonly struct PokemonDescription(string prettyName, EPokemonType type, int health, int baseAttack)
    {
        public readonly string Name = prettyName;
        public readonly EPokemonType Type = type;
        public readonly int Health = health;
        public readonly int BaseAttack = baseAttack;
    }

    public readonly PokemonDescription Description = description;
    public int Health { get; set; } = description.Health;
}
