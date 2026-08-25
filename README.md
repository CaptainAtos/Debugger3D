Debugger3D

A first-person 3D game built in Unity 6 (URP), set inside a malfunctioning server/computer environment. 
The core system is procedural dungeon generation — every playthrough builds a new layout of rooms and corridors.

Status: In active development — student project for the SAE Institute Games Programming portfolio.

Premise:
You're an overworked IT specialist, and after a long day of work you fall asleep. The fever dream begins: you're sent into the server basement to fix a critical outage.
The door locks behind you, the lights cut out, and you have to work your way through the generated corridors, reactivating servers one by one before the lights — and the door — come back on.

Core Features:
- Procedural dungeon generation — rooms, corridors and corners are assembled at runtime from a connector-based system (BFS room placement, no fixed layout)
- Runtime NavMesh baking — the navigation mesh is rebuilt after generation, so AI can navigate any generated layout
- Enemy AI — bug enemies with a patrol/chase/attack state machine, plus swarm behavior for larger groups
- First-person player controller

In Progress:
- Interactable servers activated via quick-time events
- Combat system (bug spray, lures, bombs)
- Procedural furniture placement
- Main menu, options menu, loading screen
- Custom shaders & particle effects (electrical sparks, rising smoke, monitor static, wall-crawling bug silhouettes)

Tech:
- Unity 6, Universal Render Pipeline (URP)
- C#
- Originally prototyped as a 2D top-down game, rebuilt as 3D first-person
- Built as part of SAE Institute coursework — several module assignments (procedural content generation, AI/simulation, engine tooling, shaders/VFX, copyright & licensing) are combined into this one project.
