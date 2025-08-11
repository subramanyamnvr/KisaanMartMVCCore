## View Models

### LoginViewModel
- Id: int (Key)
- UserPhoneNo: string
- OTP: int
- Role: string

Used by `AccountController` to handle login and OTP verification.

### UserMachineViewModel
- Id: int
- AmountPerHour: double (Required) — display "Amount Per Hour in Rupees"
- AmountPerAcre: double — display "Amount Per Acre in Rupees"
- SelectedMachineId: int? (Required)
- Machines: SelectList — populated for machine dropdowns (NotMapped)
- userId: int?

### ErrorViewModel
- RequestId: string
- ShowRequestId: bool (computed)