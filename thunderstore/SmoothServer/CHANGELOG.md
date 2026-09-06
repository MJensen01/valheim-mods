# Changelog — SmoothServer

## 0.2.0 (2026-09-06)
- **Renamed** from `OrionNet`. New plugin GUID `Noseferatu.SmoothServer`, new config file
  `Noseferatu.SmoothServer.cfg`. Old `net.mjensen.orion.net.cfg` settings are **not** migrated —
  re-apply them once.
- Live config reload (hot reload): edits to the cfg file are picked up on a running server
  without a restart (`[General] HotReload`).

## 0.1.0 (2026-09-06)
- Initial release. Modules: `Telemetry`, `FrameRate`, `SendCadence`, `SendBudget`, `CreateBudget`.
- Server-only; no client install required.
