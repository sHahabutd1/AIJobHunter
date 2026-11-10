# ADR-002: Job Source Integration

- **Status:** Accepted
- **Date:** 2026-01-19

## Context

The system must consider Adzuna, Greenhouse, Lever, LinkedIn, and Indeed. Provider protocols, access conditions, and permitted uses may differ. The application must respect official terms and must not bypass authentication or access controls, scrape where prohibited, or automate prohibited interactions. Automated access to LinkedIn or Indeed is not established by this decision. Manual URL and job-description import must remain available when automated access is unavailable or unauthorized.

## Decision

Represent Adzuna, Greenhouse, Lever, LinkedIn, and Indeed as first-class source identities behind independent integration boundaries. Separate a source's implementation state from its authorization and runtime-enabled state. Verify current official documentation, access requirements, and intended-use terms before implementing or enabling automated access. Do not claim that a public page or a documented API grants permission for every use.

Support manual URL and description imports with explicit manual provenance. A manually supplied URL does not imply that the application will fetch or scrape the page. Keep provider-specific credentials, response models, limits, and error handling inside each adapter. No source-specific behavior should leak into core job-domain rules.

## Consequences

### Positive

- All required sources remain represented without implying that restricted access is available.
- Manual import provides a useful fallback independent of provider approvals.
- Provider-specific changes remain isolated from matching and tracking logic.
- Implementation and authorization can be reviewed and enabled separately.

### Trade-offs

- Multiple adapters and source-specific mapping require additional code and testing.
- Manual imports require user effort and may have less complete metadata.
- Provider terms and access requirements may change and need periodic review.
- Greenhouse and Lever implementations may be useful for specific employer boards without serving as general-purpose job-search indexes; actual capabilities must be validated against current official documentation.

## Alternatives Considered

- **One generic web scraper:** Rejected because it risks violating terms, bypassing controls, and coupling the system to unstable page markup.
- **Treat every source as automatically available:** Rejected because source representation is not evidence of access authorization.
- **Exclude LinkedIn and Indeed from the architecture:** Rejected because both are required target sources; they remain first-class identities with automated connectors disabled unless access is approved.
- **Manual-only imports:** Retained as a fallback, but does not prevent adding permitted provider adapters where justified.
