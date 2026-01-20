# Santa's Savior: High-Performance C# Raylib Engine

A Native 2D Platformer built from scratch using .NET 8 and Raylib.

## Overview

Santa's Savior is a technical demonstration of game engine architecture built using C# and the Raylib graphics library. Unlike projects built in commercial engines like Unity or Unreal, this application implements its own core systems—including physics, rendering, state management, and asset handling—providing a transparent view into the low-level logic required to drive a real-time application.

The project focuses on raw performance and architectural control, bypassing heavy middleware to interact directly with the graphics and audio hardware abstraction layers provided by Raylib. It serves as a capstone for understanding the Update/Draw game loop lifecycle and Object-Oriented software design patterns.

## Key Features

* **Custom Physics Engine:** Implements AABB (Axis-Aligned Bounding Box) collision detection and resolution for kinematic bodies, gravity simulation, and platform interactions.
* **Sprite-Based Animation System:** A custom animation controller capable of handling state transitions (Idle, Run, Jump, Death) and frame-timing logic independent of the rendering frame rate.
* **Entity State Management:** A robust system for managing diverse game entities (Players, Enemies, Moving Platforms, Collectibles) with independent behavior logic.
* **Scene Management:** Architecture supporting seamless transitions between Main Menu, Gameplay Levels, and Game Over states.
* **Persistent Data:** Local save system utilizing serialization to track high scores and level progression.

## Technical Complexity & Architecture

This project moves beyond scripting behavior to implementing the systems that drive behavior.

### 1. The Core Game Loop
At the heart of `Program.cs` and `Game.cs` lies a custom-implemented Game Loop. This manages the delicate balance between:
* **Update Logic:** processing physics, input, and AI on a deterministic timescale.
* **Render Logic:** interfacing with `TextureRenderer` to draw frames to the buffer.
This separation ensures that game logic remains consistent regardless of the user's monitor refresh rate (DeltaTime management).

### 2. Object-Oriented Entity Hierarchy
The codebase utilizes a strong inheritance structure to manage complexity:
* **Base Classes:** `TextureObject` provides fundamental properties (position, scale, texture data) to all renderable items.
* **Polymorphism:** Classes like `Player`, `Enemy`, `SantaClaus`, and `Barel` extend base functionality with specific behavioral overrides, allowing the main engine to treat all objects generically during the update cycle.

### 3. Resource & Asset Management
To prevent memory leaks and ensure performance, the project implements centralized managers (`AudioMap.cs`, `TextureMap.cs`). These static classes:
* Pre-load assets into memory at initialization.
* Provide O(1) access to resources via string or enum keys.
* Manage the disposal of unmanaged resources (raw texture pointers and audio streams) when the application closes.

### 4. Custom Raylib Integration
Rather than using high-level wrappers, the project makes direct calls to the Raylib C bindings. This requires manual management of:
* Drawing textures to specific screen coordinates.
* Handling audio buffers for sound effects (Jump, Death, Pickup).
* Managing window events and input polling (Keyboard state).

## Tech Stack

* **Language:** C# (.NET 8.0)
* **Graphics/Input:** Raylib-cs (C bindings for Raylib)
* **Audio:** RayAudio
* **Architecture:** Object-Oriented Programming (OOP)
* **Platform:** Windows / Linux / macOS (Cross-platform support via .NET)

## Getting Started

This is a local C# application that requires the .NET SDK to run.

### Prerequisites
1.  .NET 8.0 SDK
2.  An IDE compatible with C# (Visual Studio, VS Code, or Rider)
