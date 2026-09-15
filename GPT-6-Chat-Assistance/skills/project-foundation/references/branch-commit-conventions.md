# Branch & Commit Conventions

## Branch Naming
Branches must follow a specific prefix structure:

- **Features**: `feature/<ticket-id>-<short-description>` (e.g., `feature/PRO-123-add-user-login`)
- **Bugfixes**: `bugfix/<ticket-id>-<short-description>` (e.g., `bugfix/PRO-456-fix-null-reference`)
- **Hotfixes**: `hotfix/<ticket-id>-<short-description>` (e.g., `hotfix/PRO-789-api-crash`)
- **Chores/Tech Debt**: `chore/<short-description>` (e.g., `chore/update-nuget-packages`)

## Commit Messages
We follow the **Conventional Commits** standard. This allows for automated changelog generation and semantic versioning.

### Format:
`<type>(<scope>): <subject>`

### Allowed Types:
- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation only changes
- `style`: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
- `refactor`: A code change that neither fixes a bug nor adds a feature
- `test`: Adding missing tests or correcting existing tests
- `chore`: Changes to the build process or auxiliary tools and libraries such as documentation generation

### Examples:
- ✅ `feat(auth): implement JWT token generation`
- ✅ `fix(api): resolve null reference exception in user controller`
- ✅ `chore(deps): update Entity Framework Core to 8.0.2`
- ❌ `added login` (Missing type and scope)
- ❌ `fix bug` (Too vague)
