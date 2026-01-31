# TypeII

A greybox, PC-only, single-player **colony/base management** game inspired by Fallout Shelter, but redesigned around:
- **Professions + Tryouts** (crew backgrounds matter more than raw stats)
- **Block-based base expansion** (infinite-ish scaling via separately loaded base chunks)
- **Managers + automation** (less micromanagement, more strategy)
- Multiple ways to grow population (shuttle arrivals, engineering, genetics) + **schooling**

This project is built in **Unity** as a learning + portfolio project. Visuals/audio are intentionally minimal for now.

---

## Core Concept

You manage a base on a hostile planet. The base is split into **Blocks** (rectangular chunks).  
Each block contains rooms that produce/consume resources:

- **O**: Oxygen  
- **W**: Water  
- **F**: Food  
- **E**: Energy  
- **S**: School  

Every crew member is a **Master** with a **Profession** (background).  
To work in a specialized room, a Master can go through a **Tryout**. Their profession influences tryout results (score 1-100) and therefore room efficiency.

Each block also has a **Manager** (a special Master) who automates chores based on rules and priorities.

---

## Goals

- Make a playable management loop with real systems (not just UI mockups)
- Keep scope controlled: ship a clean greybox vertical slice first
- Showcase architecture: data-driven content, simulation, save/load, and scalability

---

## Current Status

- [F] Phase 0: Repo + Unity setup  
- [ ] Phase 1: Single Block + grid placement  
- [ ] Phase 2: Resources + room simulation (O/W/F/E/S)  
- [ ] Phase 3: Masters + professions + tryouts  
- [ ] Phase 4: Save/Load + block loading/unloading  
- [ ] Phase 5: Managers + automation rules  
- [ ] Phase 6: Shuttle arrivals (recruitment timer)  
- [ ] Phase 7: Leveling + School profession learning  
- [ ] Phase 8: Engineering + genetics population systems  
- [ ] Phase 9: Planet events + repairs

---

## Controls (planned)

- Left click: place/select
- Right click / Esc: cancel build
- WASD / Middle mouse: camera pan
- Mouse wheel: zoom

---

## Tech Notes (planned architecture)

- ScriptableObjects for data:
  - RoomType, Profession, EventType
- Deterministic tick simulation
- JSON save files
- Block-based content loading for scalability

---

## Credits

Made by: Turar Turakbay
Tools: Unity
