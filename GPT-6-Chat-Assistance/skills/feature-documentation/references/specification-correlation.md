# Specification Correlation

When specifications (`/specs/` folder) are available, correlate them with the implementation.

## 1. Deterministic Ranking
Do not use LLM guessing to link specs. Rank candidates using this deterministic order:
1. **Exact Feature ID**: (e.g., `BOOK-001` in code comment matches `BOOK-001` in spec).
2. **Exact Feature Name**: Exact string match.
3. **Explicit Endpoint**: Spec mentions `POST /api/bookings`.
4. **Controller/Class/Method References**: Spec explicitly mentions `BookingController`.
5. **Exact Requirement Terms**: High lexical overlap.
6. **Semantic Similarity**: Used only as a last resort fallback.

## 2. Requirement Status Mapping
For each relevant requirement found in the spec, determine its status in the code:
- **Implemented**: Code explicitly matches the spec requirement.
- **Partially Implemented**: Only some evidence supports the requirement.
- **Not Found**: No code evidence supports the requirement.
- **Conflict**: The code implements something explicitly different from the spec.

## 3. Handling Conflicts
When the specification and implementation disagree, DO NOT choose one silently.
Document the discrepancy explicitly using the format:
- Specification says: X
- Observed Implementation: Y
- Difference: Z
