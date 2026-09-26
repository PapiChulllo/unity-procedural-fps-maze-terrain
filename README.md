# Procedural FPS Maze & Terrain

**Two Unity 6 procedural-generation experiments in one project:** a randomized 2D tile maze with start/goal placement, and a Perlin-noise voxel terrain with enclosed-cell culling. A first-person CharacterController player and mouse-look camera sit alongside them. This is an educational systems sample, not a finished game.

---

## What it demonstrates

- **2D maze (`MapGenerator`):** fills an `n × n` grid from regular tile prefabs (random pick + 90° rotations), then replaces one cell with a start tile (player moved ten units above it) and other cells with goal tiles.
- **3D terrain (`PerlinMapGenerator`):** samples a random Perlin scale and XZ offsets, instantiates voxels whose height falls under the noise threshold, tracks occupancy in a `List` + `HashSet`, and disables `MeshRenderer` / `BoxCollider` on fully enclosed voxels. Press **P** in Play mode to clear and regenerate.
- **Player / camera:** legacy `Input` axes for move/jump and mouse look; optional `AgentBehavior` NavMesh pursuit toward the player.

**Status / limitations:** educational prototype. The maze and terrain live in separate scene setups, not one gameplay loop. Goal selection skips the start index but can pick duplicate goal indices. Input uses the legacy `Input` API even though the Input System package is declared. No automated tests. Unity was unavailable for this documentation pass, so Editor compilation, Play mode, and builds were not re-verified here.

## Tech stack

| Area | What it uses |
|---|---|
| Engine | **Unity 6** (`6000.0.34f1`) |
| Render pipeline | **URP** (`com.unity.render-pipelines.universal` 17.0.3) |
| Navigation | **AI Navigation** 2.0.6 (`NavMeshAgent` in `AgentBehavior`) |
| Input | Legacy `Input` API in authored scripts; **Input System** 1.11.2 is in the manifest but unused by those scripts |
| UI / tooling | uGUI 2.0; Visual Effect Graph / Visual Scripting / Timeline appear in the manifest but are not used by the authored generators |

## What's in the project

Most of the repo is a standard Unity URP project shell plus models, materials, textures, and prefabs for the tile/voxel experiments. The authored gameplay/systems code is small:

| System | Key files |
|---|---|
| Randomized 2D maze grid, start placement, goal replacement | `Assets/_Scripts/MapGenerator.cs` |
| Perlin voxel generation, occupancy tracking, enclosed-voxel culling | `Assets/_Scripts/PerlinMapGenerator.cs` |
| First-person move / jump / gravity / ground check | `Assets/_Scripts/PlayerBehaviour.cs` |
| Mouse-look camera | `Assets/_Scripts/CameraController.cs` |
| NavMesh agent that chases the player | `Assets/AgentBehavior.cs` |

Authored C# is five scripts (~7.7 KB total source). Prefabs under `Assets/Prefabs/` wire the generators and tiles; visual assets under `Assets/Models/`, `Materials/`, and `Textures/` support the scenes.

### Code / system highlights

- **`MapGenerator`:** nested loops instantiate `mapSize²` tiles at `tileSize` spacing, store them in `maze`, pick a random start index, replace that tile, reposition `PlayerBehaviour`, then attempt `numberOfGoalTiles` replacements that must not equal the start index.
- **`PerlinMapGenerator`:** coroutine walks `width × depth` columns (yielding once per depth row), instantiates voxels for `y < perlinValue`, then runs a six-neighbor enclosure pass. Regeneration destroys tracked voxels and clears both collections before spawning again.
- **`PlayerBehaviour` + `CameraController`:** CharacterController locomotion with sphere ground checks; separate mouse-look script for the FPS view.

## Scenes

| Scene | Purpose |
|---|---|
| `Assets/Scenes/ProceduralGeneratedMap.unity` | **3D terrain experiment.** Committed setup enables `PerlinMapGenerator` (configured `32 × 32 × 32`, noise scale range 16–24). |
| `Assets/Scenes/SampleScene.unity` | **2D maze experiment.** Contains a `MapGenerator` prefab instance (map size 16, tile spacing 16; scene overrides goal count to 3). The generator may be inactive in the committed Hierarchy — activate it before Play. Listed in Editor Build Settings. |
| `Assets/Scenes/Experiments.unity` | Additional experiment scene present in the project. |

Open scenes directly in the Editor for the intended experiment. No verified standalone build is committed.

## Third-party / bundled assets

| Asset / package | Notes |
|---|---|
| Unity URP template packages | Project shell, render pipeline assets under `Assets/RenderPipeline/` / Settings |
| Models, materials, textures under `Assets/` | Used by tile/voxel prefabs; original authorship and redistribution terms are **not** documented in-repo — no claim of ownership is made for those binaries |

Portfolio claims apply only to the authored C# listed above.

## About this repository

Public portfolio / educational showcase for procedural generation systems authored under the **PapiChulllo** account. Conservative documentation only: no invented screenshots, tests, or runtime verification in this pass. No repository-wide license establishing rights to every included asset is provided.
