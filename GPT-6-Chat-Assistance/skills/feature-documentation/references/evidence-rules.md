# Evidence Rules

Every significant factual statement generated must be supported by project evidence. 

## 1. Allowed Evidence Types
- **CODE**: Method names, class names, variables, syntax trees.
- **SPECIFICATION**: Identifiers (e.g. `BOOK-001`) or direct quotes from spec files.
- **TEST**: Test method names verifying a specific path.
- **DATABASE**: Schema properties, DbSets, or EF configurations.
- **GIT**: Commit messages, PR titles.

## 2. Evidence Classification
Classify all conclusions into one of three buckets:
- **Observed**: Directly supported by explicit project evidence. (e.g., "The Cancel method sets Status to Cancelled").
- **Inferred**: Derived from multiple relationships but not explicitly stated. (e.g., "This service likely handles emails because it injects IEmailSender").
- **Unknown**: Insufficient evidence.

## 3. Strict Prohibitions
- **NEVER** present an inferred or unknown behavior as an observed fact.
- **NEVER** infer a graph relationship merely because a class is named similarly (e.g., do not assume `BookingController` calls `BookingService` without explicit code evidence).
- If evidence conflicts (e.g., the code does X but the spec says Y), explicitly report the conflict.
