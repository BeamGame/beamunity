using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Service;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public event Action OnUpdated;

    public List<Pokemon> Pokemons {
        get {
            return pokemons;
        }
        set {
            pokemons = value;
            OnUpdated?.Invoke();
        }
    }

    private async  void Awake()
    {

        Pokemons = new List<Pokemon>();
        var monsters = await MonsterService.GetMonsters();
        if (monsters.Count > 0)
        {
            foreach (var item in monsters)
            {
                var pokeDb = PokemonDB.GetObjectByName(item.Beamon.Name);
                var pokemon = new Pokemon(pokeDb, item.Level);
                pokemon.TokenId = item.MonsterId;
                Pokemons.Add(pokemon);
            }
            OnUpdated?.Invoke();
        }
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    private void Start()
    {

    }

    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }

    public async void GetStarter(Pokemon newPokemon)
    {
        await MonsterService.MintStarter();
        pokemons.Add(newPokemon);
        OnUpdated?.Invoke();
    }

    public async void AddPokemon(Pokemon newPokemon)
    {
        if (pokemons.Count < 6)
        {
            AddMonsterDto info = new AddMonsterDto() { Level = newPokemon.Level, Name = newPokemon.Base.Name };
            await MonsterService.MintMonster(info);
            pokemons.Add(newPokemon);
            OnUpdated?.Invoke();
        }
        else
        {
            // TODO: Add to the PC once that's implemented
        }
    }

    public bool CheckForEvolutions()
    {
        return pokemons.Any(p => p.CheckForEvolution() != null);
    }

    public IEnumerator RunEvolutions()
    {
        foreach (var pokemon in pokemons)
        {
            var evoution = pokemon.CheckForEvolution();
            if (evoution != null)
            {
                yield return EvolutionManager.i.Evolve(pokemon, evoution);
            }
        }
    }

    public void PartyUpdated()
    {
        OnUpdated?.Invoke();
    }

    public static PokemonParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PokemonParty>();
    }
}
