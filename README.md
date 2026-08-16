# Zoo World

A Unity prototype: animals spawn every 1–2 seconds, move around, collide by physics and eat each other according to food chain rules.

## Stack

| | |
|---|---|
| Unity | 6.3 LTS (6000.3.21f1) |
| Render | URP 17.3 |
| DI | [VContainer](https://github.com/hadashiA/VContainer) 1.19 |
| UI | uGUI (Canvas + TextMeshPro, shipped inside `com.unity.ugui`); UI Toolkit is not used |
| ECS | not used, as required by the task |

## Running

1. Open the project in Unity 6.3 LTS.
2. Open `Assets/_Project/Scenes/ZooWorld.unity`.
3. Press Play.

Tests: `Window → General → Test Runner → EditMode → Run All`.

## Architecture

The key requirement is *"let's assume that we will add 1000 different animals"*. That is why an animal here **is not a class**. A hierarchy like `Animal → Prey → Frog` breaks on the first non-standard creature — a bird eats insects but is eaten by a snake — so instead of inheritance the model is a composition of three independent axes:

```
AnimalDefinition (ScriptableObject)   ← species data
        │
        ├── MovementConfig  ──► IMovementBehaviour   how it moves
        ├── Traits                                   what it is to the others
        └── Eats                                     what it eats
                     │
                     ▼
          IFoodChainResolver                         "who eats whom" rules
```

Runtime composition:

```
GameLifetimeScope (composition root)
        │
        ├── AnimalSpawner ──► IAnimalFactory ──► Animal
        │                                          │  composed of:
        │                                          ├── IMovementBehaviour
        │                                          ├── IWorldBounds
        │                                          └── IFoodChainResolver
        │
        └── IGameEvents ──► GameStats ──► StatsPresenter (uGUI)
                        └─► KillLabelSpawner
```

`Animal` knows nothing about the UI or the counters: it publishes events to the bus, and subscribers decide what to do with them.

### Adding a new animal

No code required:

1. `Create → Zoo → Animal Definition`
2. Set the prefab, the movement config and the `Traits` / `Eats` tags
3. Add the asset to the `Animals` list in `GameSettings`

A bird, a spider or a fish is added the same way and needs no code at all, as long as it reuses one of the existing movement behaviours. A genuinely new way of moving costs one `IMovementBehaviour` implementation plus its `MovementConfig` — after that it is available to every species from the inspector.
