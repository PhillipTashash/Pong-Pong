#!/usr/bin/env bash
# Creates the "Multiplayer" milestone issues for PhillipTashash/Pong-Pong.
#
# Requirements: GitHub CLI (`gh`), logged in with `gh auth login`.
# Run from Git Bash:   bash create_multiplayer_issues.sh
# Preview only:        DRY_RUN=1 bash create_multiplayer_issues.sh
#
# Safe to run more than once: it reuses the existing "Multiplayer" milestone
# and skips any issue whose title already exists in that milestone.

set -euo pipefail

REPO="PhillipTashash/Pong-Pong"
MILESTONE="Multiplayer"
MILESTONE_DESC="Online 1v1 with join codes. One player hosts and gets a code, the other types it in. Local play keeps working."
DRY_RUN="${DRY_RUN:-0}"

if ! command -v gh >/dev/null 2>&1; then
  echo "GitHub CLI (gh) not found. Install it (winget install GitHub.cli), then run: gh auth login"
  exit 1
fi
if [ "$DRY_RUN" != "1" ] && ! gh auth status >/dev/null 2>&1; then
  echo "gh is not logged in. Run: gh auth login"
  exit 1
fi

# --- Milestone: reuse "Multiplayer" if it exists, otherwise create it ---------
if [ "$DRY_RUN" = "1" ]; then
  echo "[dry run] would use or create milestone: $MILESTONE"
  EXISTING_TITLES=""
else
  MS_NUMBER=$(gh api "repos/$REPO/milestones?state=all&per_page=100" \
    --jq ".[] | select(.title==\"$MILESTONE\") | .number")
  if [ -z "$MS_NUMBER" ]; then
    gh api "repos/$REPO/milestones" -f title="$MILESTONE" -f description="$MILESTONE_DESC" >/dev/null
    echo "Created milestone: $MILESTONE"
  else
    gh api -X PATCH "repos/$REPO/milestones/$MS_NUMBER" -f description="$MILESTONE_DESC" >/dev/null
    echo "Using existing milestone #$MS_NUMBER: $MILESTONE"
  fi
  EXISTING_TITLES=$(gh issue list --repo "$REPO" --milestone "$MILESTONE" --state all \
    --limit 200 --json title --jq '.[].title')
fi

create_issue() {
  local title="$1"
  local body="$2"
  if printf '%s\n' "$EXISTING_TITLES" | grep -Fxq "$title"; then
    echo "Skip (already exists): $title"
    return
  fi
  if [ "$DRY_RUN" = "1" ]; then
    echo "[dry run] $title"
    return
  fi
  gh issue create --repo "$REPO" --milestone "$MILESTONE" --title "$title" --body "$body" >/dev/null
  echo "Created: $title"
  sleep 1
}

# --- Issues, in the order to work on them ------------------------------------

create_issue "Install the multiplayer packages" "$(cat <<'EOF'
Add the packages for online play and the in-Editor two-player tester.

- Netcode for GameObjects: `com.unity.netcode.gameobjects`
- Multiplayer services: `com.unity.services.multiplayer`
- Multiplayer Play Mode: `com.unity.multiplayer.playmode`

Add the new assemblies to `Pong.Features` (and `Pong.Shared` if it needs them): `Unity.Netcode.Runtime`, `Unity.Services.Core`, `Unity.Services.Authentication`, `Unity.Services.Multiplayer`.

Work on a `multiplayer` branch so the finished game on `main` stays safe.

Done when: the project compiles with no errors and Multiplayer Play Mode can open a second virtual player.
EOF
)"

create_issue "Link the project to Unity Cloud and sign in players" "$(cat <<'EOF'
Online features need a Unity Cloud project and a signed-in player.

- Edit > Project Settings > Services: link or create a Unity Cloud project.
- Add a small persistent script (e.g. `OnlineServices`) that runs once on start:
  `await UnityServices.InitializeAsync();` then `await AuthenticationService.Instance.SignInAnonymouslyAsync();`
- Anonymous sign-in means no account or password for the player.

Done when: the Console logs a player ID on start, and starting the game twice doesn't sign in twice.
EOF
)"

create_issue "Add online buttons to the main menu" "$(cat <<'EOF'
UI only, no networking yet.

- Rename Play to "Play Local".
- Add "Host Online", "Join Online" and a join-code TextField.
- Add a status label for messages such as "Waiting for opponent..." or "Code not found".

Reuse `.menu-button` and the other shared styles in `MenuStyle.uss`.

Done when: the new buttons look like the rest of the menu, and Play Local still works.
EOF
)"

create_issue "Add the NetworkManager" "$(cat <<'EOF'
The NetworkManager runs the connection between the two players.

- Add a NetworkManager object to the MainMenu scene, with a Unity Transport component.
- Create a Network Prefabs List and assign it. The Ball and Paddle prefabs get added to it later.
- Tick Enable Scene Management.

The NetworkManager survives scene loads on its own, like your AudioManager.

Done when: the game runs with the NetworkManager in the scene and no errors.
EOF
)"

create_issue "Host a game and show the join code" "$(cat <<'EOF'
Clicking "Host Online" creates a 2-player game and shows its code.

```csharp
var options = new SessionOptions { MaxPlayers = 2 }.WithRelayNetwork();
var session = await MultiplayerService.Instance.CreateSessionAsync(options);
statusLabel.text = "Code: " + session.Code;
```

`WithRelayNetwork()` connects players across home networks without anyone opening router ports, and starts the host automatically.

Done when: the host sees a short code and "Waiting for opponent...".
EOF
)"

create_issue "Join a game with a code" "$(cat <<'EOF'
Clicking "Join Online" joins using the typed code.

```csharp
var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(code);
```

- Wrap it in `try/catch` and show a friendly status message for a wrong code or a full game.
- Disable the buttons while joining so it can't be clicked twice.

Done when: a second player (Multiplayer Play Mode) joins with the code, and a wrong code shows an error instead of crashing.
EOF
)"

create_issue "Load the Pong scene for both players" "$(cat <<'EOF'
When the second player connects, the host loads Pong for both.

- Listen for the client connecting (`NetworkManager.Singleton.OnClientConnectedCallback`).
- Host only: `NetworkManager.Singleton.SceneManager.LoadScene("Pong", LoadSceneMode.Single);`
- Local play keeps using the normal `SceneManager.LoadScene`.

Done when: both players land in the Pong scene at the same time.
EOF
)"

create_issue "Know whether a match is local or online" "$(cat <<'EOF'
Scripts in the Pong scene need to know which mode they're in.

- Add a simple flag, e.g. a static `GameMode.IsOnline`, set by the menu before loading.
- Or check `NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening`.

Done when: a test log in the Pong scene prints the correct mode for both Play Local and online.
EOF
)"

create_issue "Make the paddles networked" "$(cat <<'EOF'
Each player controls their own paddle and sees the other one move.

- Paddle prefab: add `NetworkObject` and `NetworkTransform` (owner authoritative). Add it to the Network Prefabs List.
- Host spawns one paddle per player: host on the left, joining player on the right.
- `PongMovement` becomes a `NetworkBehaviour` and only reads input when `IsOwner`.
- Online, both players can use the same keys (for example W/S).

Done when: each player moves only their own paddle and sees the opponent's paddle move. Local play still uses both paddles on one keyboard.
EOF
)"

create_issue "Make the ball networked" "$(cat <<'EOF'
The host runs the ball. The other player sees a copy.

- Ball prefab: add `NetworkObject`, `NetworkTransform` and `NetworkRigidbody2D`. Add it to the Network Prefabs List.
- `SpawnBall`: online, only the host spawns, using `Instantiate` then `GetComponent<NetworkObject>().Spawn()`.
- `BallMovement` / `BallScore`: only run bounce and scoring logic when `IsServer`.
- Replace `Destroy(gameObject)` with `NetworkObject.Despawn()` online.

Done when: the ball moves and bounces the same way on both screens, and a goal respawns it for both.
EOF
)"

create_issue "Sync the score and the winner" "$(cat <<'EOF'
Both players see the same score and the same winner.

- `scoreKeeper` becomes a `NetworkBehaviour`.
- `leftScore` and `rightScore` become `NetworkVariable<int>`. Only the host changes them.
- Update the score text from `OnValueChanged`, so it updates on both screens, including after a reset.
- Show the winner popup on both screens with an RPC.

Done when: the score matches on both screens for the whole match, and both see the same "Wins!" popup.
EOF
)"

create_issue "Play sounds on both screens" "$(cat <<'EOF'
Bounces and goals happen on the host, but both players should hear them.

- Send an RPC from the host for paddle/wall bounces.
- Goal sound: play it when the score's `OnValueChanged` fires, which happens on both screens.

The AudioManager itself doesn't change. Each computer plays sounds locally.

Done when: both players hear every bounce and goal exactly once.
EOF
)"

create_issue "Pause and quit in online matches" "$(cat <<'EOF'
`Time.timeScale` only affects one computer, so pausing works differently online.

- Online, Esc opens the pause menu as an overlay without freezing the game (no `timeScale` change).
- "Quit" leaves the game properly: `await session.LeaveAsync();` and `NetworkManager.Singleton.Shutdown();` then load MainMenu.
- Local play keeps the current freezing pause.

Done when: either player can open the menu and quit online without errors, and local pause still freezes the game.
EOF
)"

create_issue "Handle a player leaving" "$(cat <<'EOF'
If the other player quits or loses connection, don't leave anyone stuck.

- Host: when the client disconnects (`OnClientDisconnectCallback`), show "Opponent left" and offer Main Menu.
- Client: when the host goes away, show "Host left" and return to the main menu.
- Make sure `Time.timeScale` is 1 and the network is shut down before loading MainMenu.

Done when: closing either player's game sends the other back to the menu with a message, and they can start a new match.
EOF
)"

create_issue "Smooth out lag" "$(cat <<'EOF'
Pong is fast, so delay is easy to notice for the joining player.

- Turn on interpolation in the ball and paddle `NetworkTransform`s.
- Test with simulated delay (Unity Transport's network simulator, or Multiplayer Play Mode / Multiplayer Tools), e.g. 100 ms delay.
- Adjust until the ball moves smoothly and paddle hits look right.

Done when: with 100 ms of simulated delay, the ball looks smooth on the joining player's screen and doesn't visibly pass through paddles.
EOF
)"

create_issue "Test online with two computers" "$(cat <<'EOF'
Final check outside the Editor.

- Make a build and play a full match on two computers on different networks (e.g. one on a phone hotspot).
- Test: host, join by code, full match to 5, winner popup, quit, opponent leaving, rematch.
- Confirm Play Local still works exactly as before.
- Merge the `multiplayer` branch into `main`.

Done when: a full online match works start to finish on two computers and the branch is merged.
EOF
)"

echo "Done."
