# Noseferatu Valheim Mods

Two BepInEx 5 mods for Valheim 0.221.12, built and maintained for a small dedicated-server
group and released here for anyone to use. MIT licensed.

- **NoVikingLeftBehind** — quality-of-life and catch-up mechanics. One DLL, installed on the server and
  by every player. The server enforces every setting (via [ServerSync](https://github.com/blaxxun-boop/ServerSync)),
  so nobody has to agree on config by hand.
- **SmoothServer** — server-only networking tuning (frame rate, ZDO send cadence/budgets,
  telemetry). Nothing to install on the client.

Both are pre-release (see version plan below) and were built and tested against Valheim
`0.221.12` / BepInEx `5.4.2333`. A 1.0-compatible build will follow once the game updates.

## Install (players, via r2modman / Thunderstore)

1. Install [r2modman](https://thunderstore.io/package/ebkr/r2modman/) or the in-game Thunderstore
   mod manager.
2. Search **NoVikingLeftBehind** (and, if your server runs it, **SmoothServer** — server-only, players don't
   need it) under the Valheim community and install it into your profile.
3. Join the server. If `EnforceClientMod` is on, the server rejects clients that don't have a
   matching NoVikingLeftBehind version — r2modman keeps you updated automatically.

## Install (server owners)

1. Drop `NoVikingLeftBehind.dll` into `BepInEx/plugins/NoVikingLeftBehind/` on the dedicated server (and `SmoothServer.dll`
   into `BepInEx/plugins/SmoothServer/` if you want the networking tuning). Both depend on
   [BepInExPack_Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
   5.4.2333.
2. Start the server once to generate `BepInEx/config/Noseferatu.NoVikingLeftBehind.cfg` and
   `Noseferatu.SmoothServer.cfg`, then edit the values you want (see below). NoVikingLeftBehind's synced
   settings are pushed to every connecting client automatically.
3. Players only need to install NoVikingLeftBehind (not SmoothServer) client-side.

## Config overview

**NoVikingLeftBehind** (server-authoritative, synced to clients):
- `ServerKeys` — skill XP rate, skill loss on death, free build/craft, unlockable recipes.
- `Frontier` / `Tiers` — world tier from boss keys, per-material tier map; drives every
  "behind the frontier" catch-up module below.
- `TrailingTierDiscount`, `RichSmelting`, `TraderStock` — cheaper/faster progression for
  materials behind the group's frontier.
- `OreRegrowth` — mined-out nodes behind the frontier respawn after a cooldown.
- `VanguardShadow` — a damage/XP/stamina buff for under-geared players near a stronger ally.
- `PlaytimeRubberBand`, `GroupSkillCatchup` — catch-up bonuses scaled off the group's own
  hours/skill levels, not fixed numbers.
- `CombatRecharge`, `DualPowers` — forsaken powers: cooldown shaved by combat, and two powers
  carried at once with independent cooldowns.
- `FoodNoDecay`, `LongFires` — food keeps its value far longer; fireplace fuel and hand torches
  burn much longer.
- `FastMining`, `PortalTrail` — behind-the-frontier ore mines faster, and normally
  non-teleportable materials behind the frontier go through portals.
- `CraftFromChests` — crafting, building, smelters and fires pull materials from nearby
  containers (adapted from AzuCraftyBoxes, MIT-0 — see `THIRD_PARTY.md`).
- `EnforceClientMod` — require every connecting client to run a matching NoVikingLeftBehind version.
- `HotReload` — cfg edits on a running server are picked up live, no restart.

Full defaults and headless test evidence: see the module table in the project's build notes
(`research/BUILD-LAB.md` in the working repo this was extracted from — not included here).
In-game: `nvlb.status` (console command, client-side) prints the live config.

**SmoothServer** (server-only):
- `Telemetry` — periodic fps/frame-time/ZDO-rate log line, no gameplay effect.
- `FrameRate` — raise the dedicated server's Unity frame cap (default 0 = untouched, vanilla ~30).
- `SendCadence` — send ZDO updates to every connected peer on a fixed interval instead of
  vanilla's one-peer-per-frame round robin (bigger effect the more players are online).
- `SendBudget`, `CreateBudget` — the ZDO send-queue high-water mark and objects-created-per-frame
  cap, exposed as config instead of hardcoded.

## Credits

- [ServerSync](https://github.com/blaxxun-boop/ServerSync) by **blaxxun-boop**, MIT-0 — vendored
  as source in `src/NoVikingLeftBehind/Vendor/ServerSync.cs` (unmodified; see the file header before editing).
- Networking-tuning ideas informed by [BetterNetworking](https://github.com/CW-Jesse/valheim-betternetworking)
  (**CW_Jesse**, MIT) and by [Serverside Simulations](https://github.com/ddormer/valheim-serverside)
  (**ddormer**, no published license — credited for the idea, not the code; SmoothServer's
  send-cadence/budget modules are an independent implementation).
- See [`THIRD_PARTY.md`](THIRD_PARTY.md) for the full list and license texts.

## Building from source

See `src/Directory.Build.props` for how game/BepInEx references resolve — either via the
build-lab's container mount, or via `VALHEIM_MANAGED` / `VALHEIM_BEPINEX_CORE` environment
variables (or `-p:ValheimManagedDir=... -p:BepInExCoreDir=...`) pointing at a local Valheim +
BepInEx install. CI (`.github/workflows/build.yml`) fetches both from scratch on every push.

```powershell
$env:VALHEIM_MANAGED = "D:\SteamLibrary\steamapps\common\Valheim\valheim_Data\Managed"
$env:VALHEIM_BEPINEX_CORE = "D:\SteamLibrary\steamapps\common\Valheim\BepInEx\core"
dotnet build src\NoVikingLeftBehind -c Release
dotnet build src\SmoothServer -c Release
python scripts\package.py   # builds thunderstore/dist zips
```

## Versioning

- `NoVikingLeftBehind` — currently `0.3.0` (renamed from `OrionQoL`; adds the Food, Powers,
  Mining, Fires, Portals and Chests modules plus live config reload).
- `SmoothServer` — currently `0.2.0` (renamed from `OrionNet`; adds live config reload).

Names and the Thunderstore namespace are **final**: package namespace/team `Noseferatu`, packages
`NoVikingLeftBehind` and `SmoothServer`. Both are immutable once the first version is uploaded —
see `PUBLISHING.md` (in the parent `Valheim/` folder, not part of this repo) for the upload steps.

**Upgrading from OrionQoL/OrionNet:** the plugin GUIDs and config file names changed, so the
server generates fresh `Noseferatu.NoVikingLeftBehind.cfg` / `Noseferatu.SmoothServer.cfg` with
defaults on first boot — copy your old values across, delete the old `net.mjensen.orion.*.cfg`
files and the old plugin folders, and note that the catch-up data directory moved from
`BepInEx/config/orion/` to `BepInEx/config/nvlb/`.
