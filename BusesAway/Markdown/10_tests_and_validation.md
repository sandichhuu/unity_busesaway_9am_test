# Tests & Validation

Objective
- Provide a minimal test plan to validate core mechanics and guard against regressions.

Test Plan (high level)
- Unit tests:
- Passenger: color integrity, spawn color matches color enum.
- Lane: moving queue and transferring to waiting room.
- WaitingRoom: per-color buckets, draws correctly for bus color.
- Bus: boarding logic restricted to matching color and capacity constraints.
- Integration: End-to-end flow from Spawner -> Lanes -> WaitingRoom -> Bus.

- Playtests:
- Verify color-spawn cadence and bus color matching.
- Verify edge cases: bus not full, bus full, all lanes empty handling, end-of-level reset.
- Accessibility: quick checks for color-blind readability (patterns/icons).

Acceptance Criteria
- All core mechanics are covered by tests or validated in playtests.
