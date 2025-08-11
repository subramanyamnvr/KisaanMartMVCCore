## Utilities

### UtilityHelper
Namespace: `KisaanMart.Utility`

- int GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
  - Description: Generates a numeric OTP of length `iOTPLength` using the provided character set.
  - Returns: int OTP value.

Example (C#):
```csharp
using KisaanMart.Utility;

var helper = new UtilityHelper();
int otp = helper.GenerateRandomOTP(4, new []{"0","1","2","3","4","5","6","7","8","9"});
```

Used in `AccountController.Login` and `RegisterController.Create`.