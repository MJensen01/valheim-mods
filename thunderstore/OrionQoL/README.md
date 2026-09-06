# OrionQoL

One mod, server-enforced settings. Install on the **server** (`BepInEx/plugins/OrionQoL/`) and
on **every client** (r2modman: Settings > Import local mod, or search "OrionQoL" once it's
listed here). Clients without a matching version are rejected while the server has
`EnforceClientMod = true`.

Every number lives in the server's `net.mjensen.orion.qol.cfg`; clients receive it on connect
via [ServerSync](https://github.com/blaxxun-boop/ServerSync) (MIT-0, blaxxun-boop).

## Modules (each has its own `Enabled` toggle)

- **ServerKeys** (server) — skill XP rate, skill loss on death, free-build/craft keys pushed to
  vanilla clients.
- **Frontier + Tiers** (both) — world tier = highest boss defeated; per-material tier map;
  "behind the frontier" = `tier <= WorldTier - TiersBehind`.
- **TrailingTierDiscount** (client) — recipes/pieces behind the frontier cost less
  (`CostMultiplier`, default 0.5).
- **RichSmelting** (client, smelter owner) — smelter output and bar recipes ×2 behind the
  frontier.
- **TraderStock** (client) — Haldor sells bars of behind-the-frontier metals, gated by the
  relevant boss key.
- **OreRegrowth** (server) — mined-out ore nodes behind the frontier respawn after a cooldown
  (default 7 in-game days) when nobody is nearby.
- **VanguardShadow** (client) — near a better-geared ally: -25% damage taken, +50% skill XP,
  +20% stamina regen.
- **PlaytimeRubberBand** (both) — players below the group's median tracked hours get up to
  +100% gather/XP.
- **GroupSkillCatchup** (both) — skills below the group's best level up faster, capped at ×3.

Console (client): `orion.status` prints the live config.

## Dependencies

- [BepInExPack_Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) 5.4.2333

## Source & issues

https://github.com/OrionMods/orion-valheim — MIT licensed.
