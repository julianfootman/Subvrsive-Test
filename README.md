**1. Overview**

This document provides an explanation of the design, approach, and assumptions behind the combat system implemented in Unity. 

The combat system involves ten characters as default (can be changed by user input), each with unique weapons, engaging in a battle simulation where the outcome is determined by their attributes and behavior.

![giphy](https://github.com/user-attachments/assets/42223893-27b0-4b1d-b167-35f9a933efcd)


**2. Design & Implementation**

- Character System (class Player)

  Each character in the game is represented by the Player class, which includes the following attributes:

  * Health: Defines how much damage the character can take before dying.

  * Weapon: Each character is assigned a weapon that has attack speed, range, and projectile behavior.

  * Movement & Rotation: Managed using Unity's NavMeshAgent to allow AI-controlled movement.

  Character Behavior

  * Target Selection: Each character randomly selects an opponent as their target.

  * Combat Execution: Characters continuously attack their chosen targets while alive.

  * Death Handling: Upon reaching 0 health, the character enters a death state and is removed from the battle.

 - Weapon System (class Weapon)

    Weapons are instantiated from prefabs and attached to characters. Each weapon includes:

    * Attack Speed: Determines how frequently the weapon can attack.

    * Range: Defines the distance within which an enemy can be attacked.

    * Bullet Mechanic: Each attack spawns a bullet that moves toward the target.

 - Bullet System (class Bullet)

    The Bullet class manages:

    * Damage Application: When a bullet reaches a target, it reduces the target’s health.

    * Travel Speed: Defines how fast bullets move through space.

    * Collision Handling: Upon hitting a target, the bullet destroys itself and applies damage.

- Game Manager (class GameManager)

    The GameManager class handles:

    * Player Spawning: Characters are instantiated at random valid positions using Unity's NavMesh.SamplePosition().

    * Tracking Alive Players: Keeps a list of all active characters.

    * Camera Switching: Allows the player to cycle through different camera perspectives (first-person and third-person).

    * Game End Logic: Declares a winner when only one character remains.

- UI System (class GameUI)

  The UI displays:

  * The remaining health of the active character.

  * The number of players still alive.
    
  * Key Bindings

  * A winner message when only one character remains.

**3. Approach**

  - Object-Oriented Design

    The system follows an object-oriented approach:

    * Encapsulation: Each class manages its own data and behavior.

    * Modularity: Players, weapons, bullets, and the game manager are separate entities with clear responsibilities.

    * Scalability: The system is designed to handle more characters with minimal modifications.

  - AI and Navigation

    * NavMeshAgent is used to handle AI movement.

    * Characters dynamically find and move toward enemies.

  - Optimization Considerations

    * Object pooling is used for bullets to reduce memory allocation overhead.

    * Efficient Targeting: Instead of checking all objects, target selection is limited to a predefined search radius.

**4. Assumptions**

  - All characters start with full health.

  - Weapons are assigned randomly at spawn.

  - Each character is an autonomous agent controlled by AI.

  - The game is fully AI-driven with no player input.

  - Only one character remains at the end of the battle.

**5. Future Enhancements**

  - Special Weapon Effects

    * Weapons could introduce status effects (e.g., fire damage, freezing enemies, poisoning targets).

    * Each weapon would have a unique script inheriting from a base Weapon class.

  - Inventory System

    * Characters could equip and switch between multiple weapons.

    * Players could pick up dropped weapons from defeated opponents.

**6. Conclusion**

  The combat system follows a modular and scalable design to allow AI-driven battles among multiple characters. Future improvements could include more advanced AI, additional weapon mechanics, and multiplayer functionality.

