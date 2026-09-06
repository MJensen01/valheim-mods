# Orion Valheim Mods

Two BepInEx 5 mods for Valheim 0.221.12, built and maintained for a small dedicated-server
group and released here for anyone to use. MIT licensed.

- **OrionQoL** — quality-of-life and catch-up mechanics. One DLL, installed on the server and
  by every player. The server enforces every setting (via [ServerSync](https://github.com/blaxxun-boop/ServerSync)),
  so nobody has to agree on config by hand.
- **OrionNet** — server-only networking tuning (frame rate, ZDO send cadence/budgets,
  telemetry). Nothing to install on the client.

Both are pre-release (see version plan below) and were built and tested against Valheim
`0.221.12` / BepInEx `5.4.2333`. A 1.0-compatible build will follow once the game updates.

## Install (players, via r2modman / Thunderstore)

1. Install [r2modman](https://thunderstore.io/package/ebkr/r2modman/) or the in-game Thunderstore
   mod manager.
2. Search **OrionQoL** (and, if your server runs it, **OrionNet** — server-only, players don't
   need it) under the Valheim community and install it into your profile.
3. Join the server. If `EnforceClientMod` is on, the server rejects clients that don't have a
   matching OrionQoL version — r2modman keeps you updated automatically.

## Install (server owners)

1. Drop `OrionQoL.dll` into `BepInEx/plugins/OrionQoL/` on the dedicated server (and `OrionNet.dll`
   into `BepInEx/plugins/OrionNet/` if you want the networking tuning). Both depend on
   [BepInExPack_Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
   5.4.2333.
2. Start the server once to generate `BepInEx/config/net.mjensen.orion.qol.cfg` and
   `net.mjensen.orion.net.cfg`, then edit the values you want (see below). OrionQoL's synced
   settings are pushed to every connecting client automatically.
3. Players only need to install OrionQoL (not OrionNet) client-side.

## Config overview

**OrionQoL** (server-authoritative, synced to clients):
- `ServerKeys` — skill XP rate, skill loss on death, free build/craft, unlockable recipes.
- `Frontier` / `Tiers` — world tier from boss keys, per-material tier map; drives every
  "behind the frontier" catch-up module below.
- `TrailingTierDiscount`, `RichSmelting`, `TraderStock` — cheaper/faster progression for
  materials behind the group's frontier.
- `OreRegrowth` — mined-out nodes behind the frontier respawn after a cooldown.
- `VanguardShadow` — a damage/XP/stamina buff for under-geared players near a stronger ally.
- `PlaytimeRubberBand`, `GroupSkillCatchup` — catch-up bonuses scaled off the group's own
  hours/skill levels, not fixed numbers.
- `EnforceClientMod` — require every connecting client to run a matching OrionQoL version.

Full defaults and headless test evidence: see the module table in the project's build notes
(`research/BUILD-LAB.md` in the working repo this was extracted from — not included here).
In-game: `orion.status` (console command, client-side) prints the live config.

**OrionNet** (server-only):
- `Telemetry` — periodic fps/frame-time/ZDO-rate log line, no gameplay effect.
- `FrameRate` — raise the dedicated server's Unity frame cap (default 0 = untouched, vanilla ~30).
- `SendCadence` — send ZDO updates to every connected peer on a fixed interval instead of
  vanilla's one-peer-per-frame round robin (bigger effect the more players are online).
- `SendBudget`, `CreateBudget` — the ZDO send-queue high-water mark and objects-created-per-frame
  cap, exposed as config instead of hardcoded.

## Credits

- [ServerSync](https://github.com/blaxxun-boop/ServerSync) by **blaxxun-boop**, MIT-0 — vendored
  as source in `src/OrionQoL/Vendor/ServerSync.cs` (unmodified; see the file header before editing).
- Networking-tuning ideas informed by [BetterNetworking](https://github.com/CW-Jesse/valheim-betternetworking)
  (**CW_Jesse**, MIT) and by [Serverside Simulations](https://github.com/ddormer/valheim-serverside)
  (**ddormer**, no published license — credited for the idea, not the code; OrionNet's
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
dotnet build src\OrionQoL -c Release
dotnet build src\OrionNet -c Release
python scripts\package.py   # builds thunderstore/dist zips
```

## Versioning

- `OrionQoL` — currently `0.2.0`; `0.3.0` is next (adds the Food and Powers modules).
- `OrionNet` — currently `0.1.0`.

Thunderstore package names and namespaces are **immutable once uploaded** — see `PUBLISHING.md`
(in the parent `Valheim/` folder, not part of this repo) for the naming/team decision this
needs before the first upload.
