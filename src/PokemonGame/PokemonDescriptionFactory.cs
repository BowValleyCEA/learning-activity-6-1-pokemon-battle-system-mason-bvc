namespace PokemonGame;

public static class PokemonDescriptionFactory
{
    private static readonly Random _random = new();

    private static readonly Dictionary<EPokemonName, Func<Pokemon.PokemonDescription>> _pokemonMap = new()
    {
        [EPokemonName.Bulbasaur] = () => new Pokemon.PokemonDescription
        (
            "Bulbasaur",
            EPokemonType.Grass | EPokemonType.Poison,
            45,
            4
        ),
        [EPokemonName.Charmander] = () => new Pokemon.PokemonDescription
        (
            "Charmander",
            EPokemonType.Fire,
            39,
            6
        ),
        [EPokemonName.Pidgey] = () => new Pokemon.PokemonDescription
        (
            "Pidgey",
            EPokemonType.Normal | EPokemonType.Flying,
            40,
            5
        ),
        [EPokemonName.Pikachu] = () => new Pokemon.PokemonDescription
        (
            "Pikachu",
            EPokemonType.Electric,
            35,
            6
        ),
        [EPokemonName.Squirtle] = () => new Pokemon.PokemonDescription
        (
            "Squirtle",
            EPokemonType.Water,
            44,
            6
        ),
        [EPokemonName.Weedle] = () => new Pokemon.PokemonDescription
        (
            "Weedle",
            EPokemonType.Bug | EPokemonType.Poison,
            40,
            3
        ),
    };

    public static Pokemon.PokemonDescription? Create(EPokemonName pokemon) => _pokemonMap[pokemon]?.Invoke();

    public static Pokemon.PokemonDescription CreateRandom()
    {
        EPokemonName[] values = Enum.GetValues<EPokemonName>();
        EPokemonName chosen = values[_random.Next(0, values.Length)];
        return _pokemonMap[chosen].Invoke();
    }
}
