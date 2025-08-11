## Examples and Usage

Note: Many endpoints assume you are authenticated via the browser and Cookie auth. The OTP-based login flow is intended for interactive use. JSON endpoints shown below are accessible after login within the same cookie session.

### Apps
- List apps
```bash
curl -s http://localhost:5000/Apps/LoadAppData
```
Response shape:
```json
{ "data": [ { "id": 1, "appName": "...", "appDescription": "...", "appUrl": "...", "appImage": "<bytes>" }, ... ] }
```

### Machines
- Get machines as value/text pairs for dropdowns
```bash
curl -s http://localhost:5000/Machines/GetMachines
```
- DataTables server-side load
```bash
curl -s -X POST \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -d 'draw=1&start=0&length=10&search[value]=tractor' \
  http://localhost:5000/Machines/LoadMachineData
```
- Render machine image (PNG)
```bash
curl -s -o machine.png http://localhost:5000/Machines/RenderImage/1
```

### User Machines
- Load all machines for current user
```bash
curl -s http://localhost:5000/UserMachines/LoadMachinesForCurrentUser
```
- Details of a specific user machine
```bash
curl -s 'http://localhost:5000/UserMachines/GetUserMachineDetails?usermachineId=42'
```
- DataTables search for available machines by selected machine id
```bash
curl -s -X POST \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -d 'selectedMachineId=3&StartDate=2025-01-01&EndDate=2025-01-07&draw=1&start=0&length=10' \
  http://localhost:5000/UserMachines/LoadDataForSelectedMachine
```

### Transactions
- Current user summary
```bash
curl -s http://localhost:5000/Transactions/LoadTransactionsForCurrentUser
```
- List users moderated by current user
```bash
curl -s http://localhost:5000/Transactions/GetModeratorUsers
```
- Get current user profile
```bash
curl -s http://localhost:5000/Transactions/GetCurrentUser
```
- Render image as Base64 JSON
```bash
curl -s http://localhost:5000/Transactions/RenderImage/1
```

### Account and Registration (OTP Flow)
The login and registration flows are designed for interactive use:
1. POST `/Account/Login` with `UserPhoneNo` triggers an SMS OTP to the registered number.
2. GET `/Account/OTPScreen` with `UserPhoneNo` and `OTP` verifies and signs in the user.
3. Registration follows a similar pattern via `/Register/Create` then `/Register/Edit/{id}` to verify OTP and create a `User`.

Programmatic use is not recommended without adapting the flow to your environment.