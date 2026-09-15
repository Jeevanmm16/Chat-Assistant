---
name: code-review-standards
description: Defines pull request review guidelines, coding standards, validation requirements, common review checkpoints, and code quality expectations. Use when generating, reviewing, or refactoring code.
---

## Purpose
Maintain high code quality by enforcing consistent review standards.

## 1. Code Review Process
- Check all database queries for N+1 issues and `AsNoTracking` usage.
- Verify authentication checks (`[Authorize]`) on every new endpoint.
- Look for race conditions or thread-blocking calls (`.Result`).
- Confirm error messages do not leak internal details.

*(See `references/review-checklist.md`)*

## 2. Refactoring Expectations
- Do not refactor code outside the scope of the requested task.
- When generating new code, ensure it aligns with the existing architecture styles (N-Tier, Repository pattern).

---

## References

- `references/review-checklist.md`
