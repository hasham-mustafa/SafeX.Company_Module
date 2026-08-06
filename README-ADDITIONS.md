<!--
  Paste this section into your existing README.md, e.g. right after
  "## ⚙️ Getting Started". Also add a badge line right under the
  main title if you want the CI status visible at the top of the repo:

  ![CI](https://github.com/hasham-mustafa/SafeX.Company_Module/actions/workflows/ci.yml/badge.svg)
-->

## ✅ Testing

The `AuthService` (registration, login, password reset) is covered by a
dedicated xUnit test project: `SafeX.CompanyPanel.Tests`.

### What's covered

- Registration rejects duplicate emails and never persists on failure
- Registration hashes the password with BCrypt before saving (never stores plaintext)
- Login rejects unknown emails, deactivated accounts, and wrong passwords —
  and never signs a session in on any of those paths
- Login signs the company in and updates `LastLoginAt` on valid credentials
- Password reset rejects expired/invalid tokens
- Password reset updates the hash and clears the token on success
- `PasswordHelper` hashing/verification behavior, including that BCrypt
  salts make every hash unique even for the same input password

### Running the tests

```bash
dotnet restore
dotnet test
```

### Approach

- **Moq** mocks `ICompanyRepository` and `IFileService` so tests don't hit a
  real database or filesystem.
- **EF Core InMemory** provider backs `ApplicationDbContext` for tests that
  exercise `Add`/`Update`/`SaveChangesAsync`, since `DbContext` itself isn't
  cleanly mockable.
- `IHttpContextAccessor` is backed by a `DefaultHttpContext` whose
  `RequestServices` resolves a mocked `IAuthenticationService`, so
  `SignInAsync` calls can be verified without a live ASP.NET Core pipeline.

## 🔄 Continuous Integration

Every push and pull request to `main` triggers a GitHub Actions workflow
(`.github/workflows/ci.yml`) that restores, builds, and runs the full test
suite on `ubuntu-latest`. See the **Actions** tab for run history.
