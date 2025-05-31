# 🌿 Elemental Realms

_A 2D top-down sandbox game prototype inspired by the brilliant mechanics of_ **The Legend of Zelda: Breath of the Wild**.

## 📖 About

**Elemental Realms** is a gameplay-focused prototype developed for the **Game Technologies** course I'm taking at Hacettepe University. It explores emergent mechanics and modular game systems in a top-down 2D pixel art format.

At its core, the project is a small open-ended sandbox that experiments with very simple elemental interactions, physics-based interactions, and a scalable entity architecture. While not a complete game, it serves as a playground for systems-driven gameplay and design iteration.

[Play the game at itch.io (WebGL)](https://tetraizor.itch.io/elemental-realms)

## 🎮 Features

- 🌿 **Elemental Interactions** — Use suitable tools/environmental objects to burn or freeze enemies, or even yourself.
<br>
<p align="center">
<img src="docs/md/crates.gif" width="90%" alt="Gameplay Demo">
</p>

- ⚔️ **Melee Combat** — Fight enemies using close-range weapons or throw tools at them from afar instead.
<p align="center">
<img src="docs/md/melee.gif" width="90%" alt="Gameplay Demo">
</p>

- 🏹 **Ranged Combat** — Use your bow to snipe enemies and trigger reactions from a distance.
<p align="center">
<img src="docs/md/ranged.gif" width="90%" alt="Gameplay Demo">
</p>

- 🧱 **Breakable Objects** — Barrels, boxes, and environmental objects that respond to physics, and they may even contain some valuables.
<p align="center">
<img src="docs/md/crates.gif" width="90%" alt="Gameplay Demo">
</p>

- 🌍 **Open-ended Exploration** — Wander a small map with some secrets and valuables to make it easier to reach the portal.
<p align="center">
<img src="docs/md/discover.gif" width="90%" alt="Gameplay Demo">
</p>

## 🧪 Technical Highlights

The primary development goal was to create a **modular and scalable entity system**. Everything in the game like tools, items, and enemies are all built around a flexible architecture.

- Creating entities or tools is very simple.
- Use base entity classes, override what you want your custom tool to do, and you should be able to use it, throw it, interact with it as expected.
- Making entities is easy, just write simple state machine behaviours and you will have your custom entity in minutes.

## 🛠️ Technologies Used

- **Unity 6**
- **C#**
- **Pyxel Edit** (pixel art assets — see the `art/` directory)

## 🚀 Getting Started

To run the project locally:

1. Clone the repository:
   ```bash
   git clone https://github.com/Tetraizor/elemental-realms.git
   ```
2. From Unity Hub, open the `Elemental Realms/` directory.
3. Open "Game" scene in the `Elemental Realms/Scenes` directory.
4. Try it out!

## ⚠️ Crucial Missing Features

Since this is a demo I had to make in a very limited time, some of the most critical features are missing:

- ❌ No dying. When the health becomes zero, the player's character simply stops being able to move.

- ❌ No finishing the game. The player can reach the portal at the end of the level, but cannot interact with it.

- ❌ No menus, no end screens.

- ❌ No audio.

## 💡 Future Ideas

- Multiple worlds with different concepts, like an ice world, desert world, etc...
- Bigger maps.
- More types of enemies, especially better melee users.
- Bosses that the player would have to defeat using anything but ordinary attacks, things that make the player think creative strategies.
- More status effects, more elements and interactions, like electricity, wetness, sliminess, etc...
- Magic wands. Things that would make the player be able to experiment with unintended combinations. Those are the most fun ones.
- Meals, potions, things that defend the player or make them interact with environment in different ways.
