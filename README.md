# Unity Grid & Shop System

This Unity project implements a modular **shop** and **grid** system, allowing the player to select tiles from an animated interface and place them onto a Tilemap via drag-and-drop with a ghost preview.

---

## 📋 Table of Contents

* [Description](#description)
* [Features](#features)
* [Architecture](#architecture)
* [Prerequisites](#prerequisites)
* [Installation](#installation)
* [Usage](#usage)
* [Core Scripts](#core-scripts)
* [Workflow](#workflow)
* [Future Improvements](#future-improvements)
* [License](#license)

---

## Description

This system enables the player to:

1. Open/close a **shop** using the `Tab` key (animated via `Animator`).
2. Select a **tile** in the shop, which then follows the cursor with a semi-transparent ghost effect.
3. Place the tile onto a **grid** (`Tilemap` + `Grid`), respecting marked and occupied cells.
4. Reuse tile objects via **Object Pooling** (`TilePool`) for optimized performance.

The modular and well-documented design simplifies future expansions such as additional states (upgrades, removal) and multiplayer support.

---

## Features

* **Animated Shop** (`ShopManager`)
* **Ghost Preview** for placement (`Tile.GhostEffect`)
* **Grid Cell Validation** and occupancy checks (`GridManager`)
* **UI Click Blocking** to prevent unintended interactions (`EventSystem`)
* **Tile Pooling** for performance optimization
* **ScriptableObject** (`TileItemData`) for tile data

---

## Architecture

```
 ┌─────────────┐      ┌─────────────┐      ┌────────────┐
 │ ShopManager ├─────▶│ TilePool    ├─────▶│ Tile       │
 └─────────────┘      └─────────────┘      └────────────┘
        │                                        ▲
        │                                        │
        ▼                                        │
  UI (Buttons)                                   │
                                                 │
                                          ┌─────────────┐
                                          │ GridManager │
                                          └─────────────┘
                                                 ▲
                                                 │
                                            ClickHandler
```

* **ShopManager**: Manages shop UI and animations.
* **TilePool**: Handles pooling of Tile objects.
* **Tile**: Contains preview, placement logic, and state management.
* **GridManager**: Coordinates with the `Tilemap` and tracks occupancy.
* **ClickHandler**: Dispatches click events using the new Input System.

---

## Prerequisites

* Unity 6 or later
* **TextMeshPro** package
* **Input System** package

## Usage

1. Press **Play** in the Unity Editor.
2. Press **Tab** to open/close the shop.
3. Click a tile in the shop to enter ghost placement mode.
4. Move the cursor over the grid and click to place the tile.
5. To cancel placement, click outside the grid area or on UI.

---

## Core Scripts

| Script            | Description                                                          |
| ----------------- | -------------------------------------------------------------------- |
| `ShopManager.cs`  | Manages shop UI, animations, and tile selection.                     |
| `Tile.cs`         | Handles tile preview, placement logic, and visual state.             |
| `GridManager.cs`  | Singleton that manages grid dimensions, tilemap, and cell occupancy. |
| `ClickHandler.cs` | Processes mouse clicks via the Unity Input System.                   |
| `TileItemData.cs` | A `ScriptableObject` storing tile properties (name, color, sprite).  |

---

## Workflow

1. **Shop Opens** via `ShopManager.OpenShop()`
2. Player **clicks a button** → `HandleButton` collects `TileItemData`.
3. **Shop Closes** → coroutine waits for the close animation.
4. **TilePool** provides an inactive `Tile`; calls `SetTileItemData` and `HandleSelection(true)`.
5. **Tile** enters ghost mode and follows the cursor.
6. Clicking on a valid, unoccupied cell (outside UI) triggers `PlaceTile`.
7. The tile finalizes at the chosen position and state becomes `Placed`.

---

## Future Improvements

* Migrate `TileStateType` to a full **FSM** with states like `Selected`, `Upgrading`, etc.
* Add tile removal and **undo** functionality.
* Integrate **costs** and **inventory** systems.
* Load tile data via **Addressables** for dynamic content updates.

---

## License

MIT © Murillo Gomes Yonamine
