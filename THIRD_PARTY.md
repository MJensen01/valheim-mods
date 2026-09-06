# Third-party code and credits

## Vendored source

### ServerSync (`src/OrionQoL/Vendor/ServerSync.cs`)

- Author: **blaxxun-boop**
- Source: https://github.com/blaxxun-boop/ServerSync
- Commit vendored: `c57c2aa54e07cdcc7630d6068699ea781622323e` (2025-04-06)
- License: **MIT-0** (MIT No Attribution) — full text in
  `src/OrionQoL/Vendor/ServerSync-LICENSE.txt`
- Not modified. Vendored as source (rather than built as a separate DLL and merged) so it
  compiles cleanly in the same headless build pipeline as the rest of OrionQoL. Do not edit
  this file directly — re-copy from upstream if it needs to change, so it stays diffable.

## Credited ideas (no code copied)

### BetterNetworking

- Author: **CW_Jesse** (CW-Jesse)
- Source: https://github.com/CW-Jesse/valheim-betternetworking
- License: MIT
- OrionNet's `SendCadence`/`SendBudget`/`CreateBudget` modules address the same class of
  problem (ZDO send-rate/throughput) as BetterNetworking's "Update Rate" and Steamworks
  send-rate tuning, implemented independently (Harmony prefix/transpiler against the
  0.221.12 decompile). No BetterNetworking source is included in this repository.
- Note: while OrionNet's `SendCadence` module is enabled, BetterNetworking's own "Update Rate"
  option becomes inert (OrionNet's patch runs at higher Harmony priority and returns before
  BetterNetworking's runs) — its Steamworks send-rate patch is unaffected and still applies.
  Server owners running both should be aware of this overlap.

### Serverside Simulations

- Author: **ddormer**
- Source: https://github.com/ddormer/valheim-serverside
- License: none published by upstream (all rights reserved by default) — credited here for
  the architectural idea (server-authoritative simulation ownership), not for any code, which
  is why nothing from that repository is included or adapted here.

## Runtime dependency (not bundled)

- **BepInEx** / **BepInExPack_Valheim** (denikson), 5.4.2333 — LGPL-2.1 (BepInEx core). Not
  distributed with these mods; required separately (see the Thunderstore dependency in each
  mod's `manifest.json`).
- **Harmony (Lib.Harmony / 0Harmony)** — MIT, distributed as part of BepInEx, referenced but
  not bundled.

## Build-time only (not shipped in the plugin DLLs)

- `Microsoft.NETFramework.ReferenceAssemblies` (Microsoft, MIT) — compile-time only.
- `BepInEx.AssemblyPublicizer.MSBuild` (BepInEx project, MIT) — compile-time only, rewrites
  reference assemblies so private members are visible; does not affect the shipped DLL.
