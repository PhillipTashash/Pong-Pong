# Project Instructions

## Helping the learner

- Use clear, descriptive language. When explaining code, define unfamiliar terms and walk through important expressions in small steps, especially C# syntax, Unity APIs, and math.
- When adding or changing code, briefly explain what changed and why. Prefer descriptive names and straightforward code over clever shortcuts or unnecessary abstractions.
- Keep explanations concise and connected to the user's question. Do not assume the user already understands a concept just because it is common in programming.

## Project structure

- This is a Unity 2D Pong project. The Unity Editor version is `6000.3.25f1`.
- Runtime scripts live under `Assets/_Project/Features/`, organized by feature (`Ball`, `Camera`, and `Paddle`), with scripts in each feature's `Scripts` folder. Put new gameplay code with the feature that owns its behavior.
- Tests live under `Assets/_Project/Tests/EditMode/` and `Assets/_Project/Tests/PlayMode/`. Follow the matching test assembly and existing Unity Test Framework conventions.
- Unity scene, prefab, and asset references can be important to script behavior. Check the relevant Inspector setup and serialized references when a code-only explanation does not account for what happens in play mode.

## Unity workflow

- Use the Unity Editor and its Test Runner for Edit Mode and Play Mode tests. The checked-in test framework package is `com.unity.test-framework`; use the Unity project version above when opening the project.
- Treat `.csproj` and `.slnx` files as generated IDE support files, not the source of truth. Make project changes through Unity or the underlying files in `Assets/`, `Packages/`, and `ProjectSettings/` as appropriate.
- Preserve Unity `.meta` files and GUID-based asset references when adding, moving, or deleting assets. Avoid editing generated or cached folders such as `Library/`, `Temp/`, and `Logs/`.