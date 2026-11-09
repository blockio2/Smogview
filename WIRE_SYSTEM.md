# Puzzle One wire system

Open **`Smogview/Assets/Scenes/puzzle 1.unity`** and press Play. The wire system is installed in the team's existing level. No setup command or separate demo scene is needed. The existing map, player position/scale, following camera, dialogue canvases, input actions and lens HUD are retained.

## Existing objects used

| Scene object | Wire role |
| --- | --- |
| `(TEMP)breaker box` | Breaker at the north end of the left corridor |
| `(TEMP)power extender (1)` | Relay below the breaker; three prewired inputs with broken lights |
| `(TEMP)power extender` | Third box at the south end of the left corridor; tangle immediately below it |
| `(TEMP)power extender (4)` | Closet box in the small room on the left |
| `bedroom-pc-set-full` | Team's updated computer artwork in the central room |
| `(TEMP)power extender (3)` | Optional extension box along the main hallway |
| `(TEMP)power extender (2)` | Grate box in the elevator hallway |
| `(TEMP)power reciever` | Final red/blue elevator sockets; original scene spelling retained |
| `elevator` | Existing elevator; E completes the puzzle when powered |

Labels, socket lamps, wires, grate bars and an elevator power lamp are added at those positions. The old unused computer placeholder remains unchanged. `Puzzle One Wire System` holds the shared network, progression controller and overlay. Individual box components are attached to the existing objects.

## Playtest controls and solution

- **WASD** move, **E** inspect, mouse select a wire/socket, **Esc** close the overlay, **R** return a carried wire in Physical lens.
- **1 Default** reveals socket colors and breaker output lamps. **2 Physical** lets you pick up/connect wires. **4 Mental** reveals wire colors after inspecting the powered third box's tangle. Switching lenses preserves the carried wire. Social has no additional wire effect.
- Turn on the breaker. Trace its prewired leads to the relay, then carry the relay's three outputs to matching third-box sockets. Wiring before turning on power is allowed.
- Power all third-box inputs, then inspect its tangle to unlock Mental. In Physical, E at the tangle picks a random free lead. Use Mental to identify it and connect all three to the closet. E resets the tangle if all its leads are connected.
- Connect the closet's green lead to the actual computer. **E at that same computer opens its socket while carrying a wire, and opens power controls when powered with no wire in hand.**
- The three computer buttons follow closet output order. Exactly one red/yellow/blue output is OFF; yellow starts OFF. Selecting another output switches it OFF and restores the previous one. Green remains independent.
- Connect yellow to the grate's left socket. Turn red or blue OFF at the computer to power yellow and open the grate. Connect red/blue through the grate to the final elevator sockets.
- Turn yellow OFF. The grate closes without disconnecting wires; red and blue power the elevator. A light/chime indicates power. E at the elevator completes the puzzle.
- The optional extension box passes the three channels through a shuffled output order, fixed for that attempt. The saved wire lengths allow corridor routes in this map, including a direct route without the extension.

## Code and integration

New runtime scripts are in `Assets/Scripts/Wires`, namespace `Smogview.Wires`:

- `Wire`, `WireSocket`, `PowerBox`, `WireNetwork`: serialized connections, channel routing and recalculation of downstream power. Wrong colors do not power sockets. Replacing a connection returns the old lead to its source. Cross-network links, overlength connections, cycles and inaccessible grate ports are rejected.
- `PlayerWireCarrier`: one carried lead, Physical-only handling, return on exceeding reach; carrying does not stop movement.
- `WireStation`, `WireInteractionUI`: use the team's existing `Interactable` / `InteractionHitbox` convention. The overlay pauses walking and world E interactions while retaining number-key lens controls.
- `PuzzleOnePowerController`: Mental unlock, computer switching, grate access, elevator power and completion. Events support later dialogue/art/level-transition integration.
- `WireVisual`, `WireSocketVisual`, `PuzzlePowerFeedback`: lens-dependent clues, simple cables, grate bars and elevator light/status/chime.

## Changes to existing team scripts

Yes: the wire implementation modifies four pre-existing scripts. These are small integration points used by the new wire components.

| Existing script | Exact change | Why it is needed |
| --- | --- | --- |
| `Assets/Scripts/Objects/Interactables/Interactable.cs` | Added virtual `CanInteract`, defaulting to `isActiveAndEnabled`. | Lets a wire station report that it is unavailable while a panel is open. Other interactables retain the default enabled-object behavior. |
| `Assets/Scripts/Player/Player.cs` | Added `InteractionBlocked`; `FindInteractionTarget()` returns no target while blocked and skips targets whose `CanInteract` is false. | The existing E action and E prompt both respect the wire overlay, avoiding world interactions behind a menu. |
| `Assets/Scripts/Player/PlayerMovement.cs` | Added `ControlsBlocked`; `FixedUpdate()` movement now requires `_moving && CanMove && !ControlsBlocked`. | Pauses walking during wire/socket selection while preserving the existing held-object `CanMove` behavior. Walking resumes after closing the panel. |
| `Assets/Scripts/LensManager.cs` | Added optional `CanSelectLens`; `SetLens(lens)` checks it before changing the lens; keys 1-4 call that same setter. | Locks Mental until the third-box tangle unlock and keeps the HUD consistent when selection is denied. Scenes without a gate keep the original lens availability. |

The shared Player prefab and input-action asset are unchanged by this task. The existing player scene instance receives `PlayerWireCarrier`; its existing lens callback is reused, or bound on that instance if missing. The controller restores the previous lens gate when disabled.

## Comments and current presentation limits

Explanatory comments have been added or expanded in all four modified team scripts and all eleven new runtime scripts. They cover why the integration flags exist, routing, replacement connections, carry distance, lens clues, Mental progression, the computer's two-output rule, and the separation between grate access and electrical power. The comments document the integration; the merged implementation was checked in Play mode with the current team scene and LensManager.

The prototype's status text has three distinct meanings:

- **Connected:** a lead is attached, but it may be the wrong color or lack upstream power.
- **POWERED:** the attached lead has power and matches the socket's required color.
- **LOCKED:** the closed grate prevents changing the connection. Existing wiring can still conduct power. The current UI prioritizes this label and hides the underlying power status; this is a presentation limitation, not a disconnected circuit.

Mental wire colors show channel identity, not whether the wire is powered. Default socket lamps are dim when unpowered and bright when powered.


This wire task begins with Physical already available. The earlier locked-door, hammer, trash and dialogue tasks are not implemented by these scripts. Mental currently unlocks immediately on inspecting the solved tangle; set `unlockMentalImmediately` false and call `UnlockMental()` from the team's dialogue completion event when that dialogue is ready. `onEscape` is available for the next level; currently completion is shown in this scene. Restarting the scene resets progress. Cables use straight-line range and simple rendering, not rope physics around walls.

## Verification and maintenance

The saved `puzzle 1` scene is ready to play; no generator or editor helper is required. The one-time scene builder and development test runners were removed from the project for this handoff. They are retained with the local validation copy and cleanup backup outside the repository.

The runtime scripts include explanatory comments, and the scene contains all configured object references. The separate demo scene/prefab is not included.

## Local review result and teammate changelog

The saved-scene Play-mode regression passed for movement, E and lens input, panels, matching/mismatched wires, replacements, tangle unlock/reset, carry limits, computer controls, grate access, elevator completion, power loss/restoration and the optional extension. Geometry checks found approaches to all ten wire interaction points and verified seven carrying routes through the existing map. A preservation check retained all 107 serialized objects from the current main scene, with no changes to existing map, sprite-renderer, camera or behaviour components; added components/children and scene-instance overrides provide the integration. Comment-only edits were checked against both their immediate backups and the tested source.

Unrelated local scene edits are separate from this wire-system task. The merge retains main's scene organization, StartScreen destination, NPC updates and project settings.

Short changelog: Implemented Puzzle One's wire system in the existing scene: lens-based wiring, Mental unlock, computer power switching, grate/elevator logic and an optional extension box. Reused the existing player/UI, added explanatory comments and playtest checks, and made small interaction, movement and lens-gating updates to four existing scripts. Changes are local only.

The merge with main retains the team's LensManager, smog effects, camera zoom, NPC/interaction changes, current player prefab and updated level objects. Wire components use LensManager; its optional gate blocks locked Mental selection before changing the HUD or zoom. The computer interaction is attached to the new bedroom-pc-set-full object.
