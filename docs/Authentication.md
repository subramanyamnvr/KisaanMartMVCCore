## Authentication, Authorization, and Session

### Authentication
- Scheme: Cookie Authentication (`CookieAuthenticationDefaults.AuthenticationScheme`)
- Login Path: `/account/login`
- Sign-in: Upon successful OTP verification in `AccountController.OTPScreen` and `RegisterController.Edit`.
- Claims: 
  - Name: `UserPhoneNo`
  - Role: `User.Role` ("User" or "Moderator")

### Authorization
- Role checks: Some behaviors vary by role, e.g., `TransactionsController.Create` adds points for `Moderator`.
- Many endpoints rely on `User.Identity.Name` to fetch current user; they assume an authenticated session.

### Session
Set in `HomeController.Index` for authenticated users:
- `AccumulatedPoints`
- `UserName`
- `UserPhoneNo`

### Notes
- Anti-forgery tokens are required on most POST actions that modify state (`[ValidateAntiForgeryToken]`). For API-style JSON endpoints that POST (e.g., DataTables), anti-forgery is not applied unless present in code.
- The application uses the default MVC route: `{controller=Home}/{action=Index}/{id?}`.