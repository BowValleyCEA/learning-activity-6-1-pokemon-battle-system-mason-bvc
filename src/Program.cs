using PokemonGame;

internal class Program
{
    private static readonly Action[] _playerActions = [
        () => {
            int? currentPlayer = (int?)_battle?.CurrentPlayer + 1;
            int? damageDealt = _battle?.CurrentPokemonAttack();

            Console.WriteLine($"Player {currentPlayer}'s {_battle?.CurrentBattlingPokemon.Pokemon.Description.Name} attacks...");

            if (damageDealt > 0)
            {
                Console.WriteLine($"...and deals {damageDealt} damage.");
                _battle?.EndTurn();
                return;
            }

            Console.WriteLine("...and misses!");
            _battle?.EndTurn();
        },
        () => {
            int? currentPlayer = (int?)_battle?.CurrentPlayer + 1;
            int? amountHealed = _battle?.CurrentPokemonUseItem();

            if (amountHealed == 0)
            {
                Console.WriteLine("Your Pokemon is already at full health!");
                return;
            }

            Console.WriteLine($"Player {currentPlayer}'s {_battle?.CurrentBattlingPokemon.Pokemon.Description.Name} used a Potion and recovered {amountHealed} health!");
            _battle?.EndTurn();
        },
        () => {
            int? currentPlayer = (int?)_battle?.CurrentPlayer + 1;

            _battle?.CurrentPokemonDefend();
            Console.WriteLine($"Player {currentPlayer}'s {_battle?.CurrentBattlingPokemon.Pokemon.Description.Name} chose to defend.");
            _battle?.EndTurn();
        },
    ];

    private static Battle? _battle;

    private static void Main()
    {
        Console.WriteLine("Welcome trainers, to the Pokestadium (TM)!");

        for (bool quit = false; !quit;)
        {
            _battle = new Battle(new Pokemon(PokemonDescriptionFactory.CreateRandom()), new Pokemon(PokemonDescriptionFactory.CreateRandom()));

            Console.WriteLine($"Player 1: \"I choose you, {_battle.PokemonFor[Battle.EPlayer.One].Pokemon.Description.Name}!\"");
            Console.WriteLine($"Player 2: \"Go! I choose {_battle.PokemonFor[Battle.EPlayer.Two].Pokemon.Description.Name}!\"");

            while (!_battle.IsDone)
            {
                int currentPlayer = (int)_battle.CurrentPlayer + 1;

                Console.Write($"Player {currentPlayer} ({_battle.CurrentBattlingPokemon.Pokemon.Description.Name}, {_battle.CurrentBattlingPokemon.Pokemon.Health} HP) Turn:\n1: Fight\n2: Item\n3: Defend\nEnter your choice: ");

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

                _playerActions[choice - 1].Invoke();
            }

            foreach (var p in (ICollection<Pokemon>)[_battle!.CurrentBattlingPokemon.Pokemon, _battle!.CurrentBattlingTargetPokemon.Pokemon])
            {
                int printableWinnerNumber = ((int)_battle.CurrentPlayer) + 1;
                int printableVictimNumber = ((int)_battle.CurrentBattlingPokemon.Trainer) + 1;

                if (p.Health <= 0)
                {
                    Console.WriteLine($"Player {printableVictimNumber}'s {_battle.CurrentBattlingPokemon.Pokemon.Description.Name} fainted!\nPlayer {printableWinnerNumber} wins!");
                }
            }

            while (true)
            {
                Console.Write("Play again? (Y/N): ");

                if (char.TryParse(Console.ReadLine(), out char c))
                {
                    char cc = Char.ToLower(c);

                    if (cc == 'y')
                    {
                        break;
                    }
                    else if (cc == 'n')
                    {
                        Console.WriteLine("Thanks for playing!");
                        quit = true;
                        break;
                    }

                    Console.WriteLine("Didn't quite catch that.");
                }
            }
        }
    }
}
