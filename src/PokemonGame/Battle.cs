namespace PokemonGame;

public class Battle
{
    public enum EPlayer
    {
        One,
        Two,
    }

    // I'm using encapsulation as a means of separating concerns and getting rid
    // of usage assumptions in the actual Pokemon class.
    public class BattlingPokemon(Pokemon pokemon, EPlayer trainer)
    {
        public Pokemon Pokemon = pokemon;
        public EPlayer Trainer = trainer;
        public bool IsDefending;
    }

    /*
     * Actual Pokemon weakness system is obviously more in-depth than this.
     */
    private readonly Dictionary<EPokemonType, EPokemonType> _weaknesses = new()
    {
        [EPokemonType.Bug] = EPokemonType.Flying,
        [EPokemonType.Electric] = EPokemonType.Grass,
        [EPokemonType.Fire] = EPokemonType.Water,
        [EPokemonType.Flying] = EPokemonType.Ground,
        [EPokemonType.Grass] = EPokemonType.Poison,
        [EPokemonType.Normal] = EPokemonType.Rock,
        [EPokemonType.Poison] = EPokemonType.Poison,
        [EPokemonType.Water] = EPokemonType.Dragon,
    };

    /*
     * Real Pokemon games have a Trainer-to-many-Pokemon relationship.
     * so it would be like Dictionary<EPlayer, Pokemon[]>.
     */
    private readonly Dictionary<EPlayer, BattlingPokemon> _pokemonFor = [];
    private readonly Random _random = new();

    public EPlayer CurrentPlayer { get; private set; } = EPlayer.One;
    public EPlayer CurrentTarget { get; private set; } = EPlayer.Two;
    public IReadOnlyDictionary<EPlayer, BattlingPokemon> PokemonFor => _pokemonFor;
    public BattlingPokemon CurrentBattlingPokemon => PokemonFor[CurrentPlayer];
    public BattlingPokemon CurrentBattlingTargetPokemon => PokemonFor[CurrentTarget];
    public bool IsDone { get; private set; }

    /*
     * Reason for dependency-injecting the Pokemon: In the actual game, Pokemon
     * exist before battle and continue to exist after battle (I would hope).
     * Besides, it should be the higher-level module's responsibility to
     * allocate (in this case, the driver program).
     */
    public Battle(Pokemon pokemon1, Pokemon pokemon2)
    {
        _pokemonFor[EPlayer.One] = new BattlingPokemon(pokemon1, EPlayer.One);
        _pokemonFor[EPlayer.Two] = new BattlingPokemon(pokemon2, EPlayer.Two);
    }

    public void EndTurn()
    {
        IsDone = CurrentBattlingPokemon.Pokemon.Health <= 0 || CurrentBattlingTargetPokemon.Pokemon.Health <= 0;

        if (IsDone)
        {
            return;
        }

        CurrentTarget = CurrentPlayer;
        CurrentPlayer += 1;
        CurrentPlayer = (EPlayer)((int)CurrentPlayer % Enum.GetValues<EPlayer>().Length);
    }

    public int CurrentPokemonAttack()
    {
        CurrentBattlingPokemon.IsDefending = false;

        int damageDealt = CalculateDamage();
        bool shouldMiss = damageDealt <= 0;

        shouldMiss |= (CurrentBattlingTargetPokemon.Pokemon.Description.Type & EPokemonType.Flying) != 0
                   && _random.Next() % 3 == 0;

        if (shouldMiss)
        {
            damageDealt = 0;
        }

        CurrentBattlingTargetPokemon.Pokemon.Health -= damageDealt;

        return damageDealt;
    }

    public int CurrentPokemonUseItem()
    {
        CurrentBattlingPokemon.IsDefending = false;

        int previousHealth = CurrentBattlingPokemon.Pokemon.Health;

        CurrentBattlingPokemon.Pokemon.Health += 20;
        CurrentBattlingPokemon.Pokemon.Health = Math.Min(CurrentBattlingPokemon.Pokemon.Health, CurrentBattlingPokemon.Pokemon.Description.Health);

        int amountHealed = CurrentBattlingPokemon.Pokemon.Health - previousHealth;

        return amountHealed;
    }

    public void CurrentPokemonDefend()
    {
        CurrentBattlingPokemon.IsDefending = true;
    }

    public int CalculateDamage()
    {
        int damage = _random.Next(0, CurrentBattlingPokemon.Pokemon.Description.BaseAttack);

        foreach (EPokemonType type in Enum.GetValues<EPokemonType>())
        {
            EPokemonType isolatedTypeOfTargetPkmn = CurrentBattlingTargetPokemon.Pokemon.Description.Type & type;

            if (isolatedTypeOfTargetPkmn == 0)
            {
                continue;
            }

            if ((_weaknesses[isolatedTypeOfTargetPkmn] & CurrentBattlingPokemon.Pokemon.Description.Type) != 0)
            {
                damage *= 2;
            }
        }

        if (CurrentBattlingTargetPokemon.IsDefending)
        {
            damage /= 2;
        }

        return damage;
    }
}
