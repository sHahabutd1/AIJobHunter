# ADR-003: AI Integration

- **Status:** Accepted
- **Date:** 2026-01-19

## Context

The application needs job-description analysis, structured requirement extraction, skill matching, resume relevance analysis, skill-gap identification, and explainable job matching. The approved MVP uses the OpenAI API exclusively and requires Structured Outputs. Candidate claims must not be fabricated, and hard eligibility constraints—particularly work authorization and sponsorship—must not be decided by an LLM. Human approval is required for application actions.

## Decision

Use OpenAI through an application-owned AI service boundary. Use Structured Outputs for bounded extraction and analysis tasks, validate returned data before use, and retain sufficient version metadata to understand analysis results. Combine AI-assisted extraction with deterministic business rules and scoring. Use verified profile facts as candidate evidence; AI-generated or extracted claims remain unverified until reviewed. Deterministic rules determine eligibility outcomes, and uncertain cases are presented for human review. The AI integration cannot submit applications or take other external application actions.

## Consequences

### Positive

- AI provider-specific code is isolated from domain and application rules.
- Structured data can support repeatable validation and explainable matching.
- Deterministic eligibility preserves control over non-negotiable constraints.
- Human review and verified facts reduce the risk of fabricated professional claims.

### Trade-offs

- Model responses can still omit, misinterpret, or inconsistently extract information; validation and review remain necessary.
- OpenAI calls introduce cost, latency, availability, and external data-processing considerations.
- Prompt/schema and model changes may affect analysis output and should be tracked.
- Sending resume or job-description content to an external provider requires an explicit privacy and data-minimization review.

## Alternatives Considered

- **LLM-only eligibility and ranking:** Rejected because critical eligibility decisions must remain deterministic and auditable.
- **No AI in the MVP:** Rejected because structured AI analysis is an approved core capability.
- **Multiple AI providers in the MVP:** Rejected because OpenAI API only is the approved initial provider; the application-owned boundary allows future extension without adding providers now.
- **AI-generated resume customization:** Out of scope; the original resume remains unchanged, and customized documents are not generated in the MVP.
