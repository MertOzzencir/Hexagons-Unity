Hex-Based Automation Prototype
A hex grid automation game prototype inspired by Factorio and Astroneer. Features modular extractors with swappable components (drill, feeder, storage) and procedural automation arms for material transport.
Show Image
Modular extractor system with automation arm transferring materials
🎯 Project Goals
Build a hex-based automation game where machines are modular — instead of single-purpose buildings, players assemble extractors from interchangeable parts. Prototype core systems before committing to full development.
Learning Objectives:

Modular building system with component dependencies
Procedural arm animation using inverse kinematics concepts
Hex grid resource management
Event-driven machine state management

⚠️ Project Status
This is an unfinished prototype. Development paused after core systems were implemented to assess viability. The foundation works, but lacks game loop, progression, and polish.
What's Working:

✅ Modular extractor assembly (drill + feeder + storage)
✅ Automation arm rotation and material transfer
✅ Resource hex tiles with depletion
✅ Component hot-swapping

What's Missing:

❌ Conveyor belts / transport systems
❌ Processing machines
❌ Player progression / tech tree
❌ Save system
❌ UI / feedback systems

🔧 Core Systems
1. Modular Extractor System
Extractors aren't single objects — they're composed of three swappable components:
Component Architecture:
ExtractorBase (hub)
├── Drill (mining component)
├── Feeder (transfer component)  
└── Storage (inventory component)
How It Works:

Place ExtractorBase on resource hex
Attach drill, feeder, storage in any order
Machine activates only when all 3 components present
Remove any component → machine pauses
Swap components → different extraction rates/storage

Why Modular:

Encourages experimentation (fast drill + slow storage = backlog)
Creates upgrade paths (replace drill, keep infrastructure)
More interesting than fixed buildings

2. Component Dependencies
Components communicate via events to maintain state consistency:
csharp// Storage full → pause drill & feeder
BaseStorage.OnStorageAvailable += ToolsActiveMode;

// All components present → start extraction
if (Drill != null && Feeder != null && Storage != null)
    TryStartWorking();
State Machine:
Incomplete → (component added) → Checking → (all present) → Working
Working → (storage full) → Paused
Working → (component removed) → Incomplete
This prevents resource waste and ensures machines respond to dynamic changes.
3. Automation Arm (Procedural Animation)
The arm uses sequential joint rotation to reach targets — inspired by IK but simplified:
3-Joint System:

Root joint rotates toward target (Y-axis rotation)
Middle joint adjusts angle based on distance
Grab joint fine-tunes final orientation

Animation Sequence:
csharpIEnumerator RotateToTarget(Transform goal) {
    // Step 1: Rotate root toward target
    Quaternion rootTarget = Quaternion.FromToRotation(root.right, direction);
    yield return RotateJoint(root, rootTarget);
    
    // Step 2: Adjust middle joint for reach
    float angle = CalculateAngle(middle.position, goal.position);
    yield return RotateJoint(middle, angle);
    
    // Step 3: Grab joint reaches target
    yield return RotateJoint(grab, finalAngle);
    
    // Step 4: Transfer material
    TakeOrGiveMaterial();
    
    // Step 5: Return to neutral
    yield return ResetToNeutral();
}
Animation Curves:
Each joint has customizable curves for easing — creates natural, non-robotic movement.
4. Resource Depletion
Hex tiles track resource amounts:
csharppublic void Dig(out Materials material) {
    currentAmount--;
    material = resourceType;
    
    if (currentAmount <= 0)
        TileDepletedEvent?.Invoke();
}
When depleted, extractors stop and must be relocated.
5. Component Hot-Swapping
Players can remove components from active machines:
csharp// Component removed mid-operation
public void OnPickedFromPlacement() {
    Detach();
    CurrentBase.RemoveTool(this);
    enabled = false;  // Stop processing
}
This creates interesting gameplay — steal a drill from one machine to upgrade another.
💡 What I Learned
Modular Design

Event-driven component communication beats polling
Dependencies should be explicit (all-or-nothing activation)
Hot-swapping requires careful state cleanup
Modular != always better (added complexity vs fixed buildings)

Procedural Animation

Sequential joint rotation is simpler than full IK
Animation curves make huge difference in feel
Local vs world space rotations matter
Coroutines sequence complex animations elegantly

Hex Grid Systems

Resource tiles need state management
Neighbor detection for automation routing
Placement validation (resource vs empty tiles)

🎓 Why I Paused Development
After implementing core systems, I realized:
Technical Challenges:

Conveyor routing on hex grids is complex
Full automation needs pathfinding
UI for component selection would be extensive

Design Questions:

Is modular assembly fun, or just tedious?
Does hex grid add value vs square grid?
Scope too large for solo prototype

Decision: Pause and prototype smaller, focused projects instead of committing months to uncertain design.
🛠️ Technical Stack

Unity 6000.0.2
Hex grid system (axial coordinates)
Event-driven architecture
Coroutine-based animation
ScriptableObject data system

📂 Code Structure
Assets/_Scripts/
├── WorldObject/
│   ├── ExtractorBase.cs          # Modular hub
│   ├── Drill.cs                  # Mining component
│   ├── Feeder.cs                 # Transfer component
│   └── Storage.cs                # Inventory component
├── AutomationArmController.cs    # Procedural arm animation
├── Hex/
│   ├── HexTile.cs               # Base hex tile
│   └── ResourceHexTile.cs       # Mineable tiles
└── Player/
    └── InteractionController.cs  # Placement system
🔍 Challenges Solved
Component Synchronization

Problem: Drill runs when storage full
Solution: Event-based pause system

Arm Reach Calculation

Problem: Arm couldn't reach all angles
Solution: Added random angle offset + fallback neutral position

Hot-Swap Cleanup

Problem: Removed components left machine in broken state
Solution: Explicit Detach() method + state reset

🎯 What Would Come Next
If development continued:

 Conveyor belt system with hex pathfinding
 Processing machines (smelters, assemblers)
 Power grid system
 Tech tree / progression
 Save/load system
 Polish + VFX

📚 Lessons for Future Projects
Prototype Core Loop First:
Before building modular systems, validate that base gameplay is fun.
Scope Carefully:
Hex grids + modular buildings + automation = massive scope. Start simpler.
Question Your Assumptions:
"Modular is more interesting" seemed obvious, but playtest first.

Developer: Mert Özzencir
GitHub: MertOzzencir
Unity Version: 6000.0.2
Learning Focus: Modular systems, event-driven architecture, procedural animation, prototype evaluation