## Controllers and Endpoints

Default route template: `{controller}/{action}/{id?}`. Unless specified, actions respond to GET and return Views. JSON endpoints are explicitly called out.

### AccountController
- GET `/Account/Index`
  - Returns: View of `LoginViewModel` list (admin view).
- GET `/Account/Login`
  - Returns: Login View.
- POST `/Account/Login`
  - Anti-forgery: Yes (`[ValidateAntiForgeryToken]`).
  - Body (form): `Id`, `UserPhoneNo`, `UserPassword`, `Role`, `OTP` (model bound).
  - Behavior: Generates OTP for existing user, sends SMS, then redirects to `/Account/OTPScreen`.
  - Returns: Redirect to `OTPScreen` on success; re-displays View with model error on failure.
- GET `/Account/OTPScreen`
  - Query/Form: `Id`, `UserPhoneNo`, `UserPassword`, `Role`, `OTP`.
  - Behavior: Verifies OTP, signs user in via Cookie auth, redirects to `/Home/Index` on success; re-displays View otherwise.
- GET `/Account/Logout`
  - Signs out and redirects to `/Home/Index`.

### AppsController
- GET `/Apps/LoadAppData` [JSON]
  - Returns: `{ data: Apps[] }` with all apps.
- GET `/Apps/Index`
  - Returns: View with all apps.
- GET `/Apps/Details/{id}`
  - Returns: Details View for `Apps` by `id` or 404.
- GET `/Apps/Create`
  - Returns: Create View.
- POST `/Apps/Create`
  - Anti-forgery: Yes.
  - Body (form): `Id, AppName, AppDescription, AppUrl, AppImage(file)`.
  - Behavior: Saves image bytes if provided, creates `Apps`, redirects to Index.
- GET `/Apps/Edit/{id}`
  - Returns: Edit View for `Apps`.
- POST `/Apps/Edit/{id}`
  - Anti-forgery: Yes.
  - Body (form): `Id, AppName, AppDescription, AppUrl, AppImage`.
  - Behavior: Updates record; redirects to Index.
- GET `/Apps/Delete/{id}` / POST `/Apps/Delete/{id}`
  - Anti-forgery (POST): Yes. Deletes `Apps` and redirects to Index.

### HomeController
- GET `/Home/Index`
  - Behavior: If authenticated, sets session keys: `AccumulatedPoints`, `UserName`, `UserPhoneNo`.
  - Returns: Home View.
- GET `/Home/Privacy`
- GET `/Home/Error`
- GET `/Home/PolyhouseFarming`
- GET `/Home/CropInsurance`
- GET `/Home/Advisory`
- GET `/Home/ProcessingUnit`
- GET `/Home/Fertilizer`
- GET `/Home/SeedTesting`
- GET `/Home/AgroMarketing`
- GET `/Home/StorageUnit`
- GET `/Home/SoilTesting`
- GET `/Home/OrganicFarming`

### MachinesController
- GET `/Machines/Index`
  - Returns: View with all machines.
- GET `/Machines/GetMachines` [JSON]
  - Returns: `SelectListItem[]` with `{ Value, Text }` for machines.
- POST `/Machines/LoadMachineData` [JSON]
  - Body (form): DataTables fields `draw`, `start`, `length`, `order[0][column]`, `order[0][dir]`, `search[value]`.
  - Filters by `search[value]` on `MachineName`.
  - Returns: `{ draw, recordsFiltered, recordsTotal, data: Machine[] }`.
- GET `/Machines/Details/{id}`
  - Returns: Details View or 404.
- GET `/Machines/Create` — POST `/Machines/Create`
  - POST Body: `Id, MachineName, MachineDescription, MachineImage(file)`.
  - Behavior: Saves image bytes, creates `Machine`, redirects to Index.
- GET `/Machines/RenderImage/{id}`
  - Returns: image/png bytes of the machine image.
- GET `/Machines/Edit/{id}` — POST `/Machines/Edit/{id}`
  - POST Body: `Id, MachineName, MachineDescription`.
  - Behavior: Updates record; redirects to Index.
- POST `/Machines/Delete/{id}`
  - Deletes and redirects to Index.

### RegisterController
- GET `/Register/Index`
  - Returns: View with all temporary registrations.
- GET `/Register/Details/{id}`
- GET `/Register/Create`
  - Query: `Register` determines `UserType` (Moderator vs User) via `TempData`.
- POST `/Register/Create`
  - Anti-forgery: Yes.
  - Body: `Id, UserName, UserPhoneNo, UserPassword, IsModerator, Latitude, Longitude, RandOTP` (model bound).
  - Behavior: Prevents duplicate phone, sets `IsModerator` from `TempData`, generates and SMSes OTP, saves `TempUser`, redirects to `Edit` (OTP screen).
- GET `/Register/Edit/{id}`
  - Behavior: Displays OTP entry; sets `TempData["Success"]` message.
- POST `/Register/Edit/{id}`
  - Anti-forgery: Yes.
  - Body: `Id, UserName, UserPhoneNo, UserPassword, IsModerator, Latitude, Longitude, RandOTP`.
  - Behavior: Verifies OTP, creates persistent `User`, signs in via Cookie auth, redirects Home.
- GET `/Register/Delete/{id}` — POST `/Register/Delete/{id}`

### TransactionsController
- GET `/Transactions/RenderImage/{id}` [JSON]
  - Returns: `{ base64imgage: string }` with Base64 of machine image tied to `id`.
- GET `/Transactions/LoadTransactionsForCurrentUser` [JSON]
  - Auth: Requires logged-in user.
  - Returns: `{ data: { StartDate, EndDate, Noofhours, Noofacres, Totalamount }[] }` for current user.
- GET `/Transactions/GetModeratorUsers` [JSON]
  - Auth: Requires logged-in user; returns users moderated by current user as `SelectListItem[]`.
- GET `/Transactions/GetCurrentUser` [JSON]
  - Returns: `{ data: User }` for current user.
- GET `/Transactions/Index`
  - Returns: View of current user's transactions (includes `UserMachines`).
- Standard CRUD: `Details`, `Create` (adds points to moderator, SMS to user/owner), `Edit`, `Delete` with anti-forgery on POST.

### UserController
- GET `/User/Index`
  - Returns: View of users where `ModeratorId == currentUser.Id`.
- Standard CRUD Views: `Details`, `Create` (assigns `ModeratorId` to current moderator), `Edit` (edits current user), `Delete`.

### UserMachinesController
- GET `/UserMachines/Index`
  - Returns: View of current user's machines with includes.
- GET `/UserMachines/GetUserMachineDetails` [JSON]
  - Query: `usermachineId`.
  - Returns: `{ data: UserMachine[] }` filtered by `usermachineId`.
- POST `/UserMachines/LoadDataForSelectedMachine` [JSON]
  - Body (form): DataTables fields + `selectedMachineId`, `StartDate`, `EndDate`.
  - Returns: `{ draw, recordsFiltered, recordsTotal, data: { MachineId, AmountPerHour, AmountPerAcre, MachineName, UserPhoneNo, Distance }[] }`.
- GET `/UserMachines/LoadMachinesForCurrentUser` [JSON]
  - Returns: `{ data: { MachineId, AmountPerHour, AmountPerAcre, MachineName, UserPhoneNo, MachineImage }[] }` for current user.
- Standard CRUD: `Details`, `Create` (binds current user and random `Distance`), `Edit` (preserves `Distance`), `Delete`.

### WorkersController
- GET `/Workers/Index`
  - Returns: View with all workers.
- GET `/Workers/Details/{id}`
  - Returns: Details View or 404.
- GET `/Workers/Create` — POST `/Workers/Create`
  - POST Body: `Id, UserName, UserPhoneNo`.
  - Behavior: Creates a `Worker`, redirects to Index.
- GET `/Workers/Edit/{id}` — POST `/Workers/Edit/{id}`
  - POST Body: `Id, UserName, UserPhoneNo`.
  - Behavior: Updates record; redirects to Index.
- GET `/Workers/Delete/{id}` — POST `/Workers/Delete/{id}`
  - Deletes and redirects to Index.