# CITADEL_NEXUS_PUBLIC

**Role:** Public Gateway / Documentation Hub for the
[DJ-Goana-Coding](https://github.com/DJ-Goana-Coding) fleet.

This repository is the public-facing facade of the **Citadel Omega Mesh**.
It does not host application code, workers, tunnels, or secrets — those
live in the private spokes of the fleet. The canonical machine-readable
identity for this node lives in [`fleet_role.yaml`](./fleet_role.yaml).

## Citadel Omega Mesh

The Citadel Omega Mesh is a **321 GB Spoke-and-Wheel architecture** spanning
12 repositories. A central hub coordinates a ring of specialized spokes,
each with a single, well-defined role:

- **Hub (Mapping-and-Inventory)** — owns the Global Mesh Map, the Spoke
  Registry, and the Master Inventory Ledger. Every spoke registers itself
  here so the fleet has one source of truth.
- **Oracle spokes** — observation, signal collection, and forecasting.
- **Librarian spokes** — indexing, archival, and retrieval across the
  321 GB working set.
- **Trader spokes** — outbound action and execution surfaces.
- **Worker spokes** — long-running compute, bridges, and reporters.
- **Public Gateway (this repo)** — public documentation and identity
  surface so external tools and the Librarian can index the fleet.

The Master Hub for the mesh is
[`DJ-Goana-Coding/Mapping-and-Inventory`](https://github.com/DJ-Goana-Coding/Mapping-and-Inventory),
which holds the authoritative Mesh Map, Spoke Registry, and Inventory
Ledger referenced by every other node.

## System Status

Live operational status, telemetry, and command surface for the mesh are
exposed through the **Vercel HUD** (the Command Face):

- HUD: <https://citadel-nexus-private.vercel.app>

This repository itself is intentionally minimal; its health is simply
"present and indexable." Runtime status of the spokes is reported in the
HUD, not here.
