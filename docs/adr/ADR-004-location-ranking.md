# ADR-004: Location Ranking and Eligibility

- **Status:** Accepted
- **Date:** 2026-01-19

## Context

Candidates need configurable location preferences because preferred regions, relocation willingness, and remote-work eligibility vary. A remote opportunity must be available to the candidate at their configured residence or otherwise satisfy their U.S. work-location requirements. The ranking pattern prioritizes the configured primary location, then acceptable relocation opportunities, then remote opportunities available at the candidate's configured residence. Acceptable relocation opportunities must not be ranked below remote opportunities outside the primary location solely because they are remote.

## Decision

Evaluate location eligibility separately from preference ranking. A remote label by itself is insufficient: the posting must establish eligibility for the candidate's configured residence or otherwise meet the candidate's U.S. work-location requirements. If the work location or eligible regions are unknown or ambiguous, mark the opportunity for review rather than assuming it is eligible.

Among opportunities that satisfy hard eligibility requirements, apply this location preference order:

1. Opportunities in the candidate's configured primary location or region.
2. Relocation opportunities within the candidate's configured relocation area.
3. Remote opportunities available to the candidate at their configured residence.

Represent the location rationale in the match explanation. Do not allow an LLM to override location eligibility or the approved priority. The exact numerical score weights, if any, are not fixed by this ADR and require validation before being adopted.

## Consequences

### Positive

- Location preference reflects the candidate's stated priorities rather than treating all remote roles as best matches.
- Work-location eligibility is explicit and reviewable.
- Unknown location details do not silently become positive matches.
- Acceptable relocation opportunities are ranked above remote opportunities outside the configured primary location.

### Trade-offs

- Job descriptions may not clearly state eligible work locations, creating `NeedsReview` cases.
- Source data and employer policy can be incomplete or stale, so location eligibility may require user confirmation.
- A strict priority order may not capture every future preference nuance; the order can be revisited through a new ADR if requirements change.

## Alternatives Considered

- **Rank all remote roles above on-site roles:** Rejected because the location preference pattern ranks acceptable relocation opportunities above remote opportunities outside the configured primary location.
- **Treat every U.S. remote posting as eligible:** Rejected because employers may restrict remote work by state or location.
- **Let AI infer location eligibility:** Rejected because eligibility is a deterministic constraint and uncertain cases require review.
