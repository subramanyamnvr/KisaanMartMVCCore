## Domain Models

### Apps
- Id: int
- AppName: string (Name)
- AppDescription: string (multiline)
- AppUrl: string
- AppImage: byte[]

### Machine
- Id: int
- MachineName: string
- MachineDescription: string (multiline)
- MachineImage: byte[]

### TempUser
- Id: int (Key)
- UserName: string
- UserPhoneNo: string (Required)
- Latitude: double (Hidden)
- Longitude: double (Hidden)
- IsModerator: bool?
- RandOTP: int?

### Transaction
- Id: int
- StartDate: Date (Required)
- EndDate: Date
- Purpose: string
- NoOfHours: int (Required)
- NoOfAcres: int
- TotalAmountToBePaid: double
- UserMachineId: int (Required, FK -> UserMachine)
- requesteduserId: int (Requested By)
- behalfuserId: int? (Request On Behalf Of)

### User
- Id: int (Key)
- UserName: string
- UserPhoneNo: string (Required)
- IsActive: bool? (Hidden)
- Role: string (Hidden) — values: "User" or "Moderator"
- Latitude: double (Hidden)
- Longitude: double (Hidden)
- ModeratorId: int (Hidden)
- PointsAccumulated: int (Hidden)
- RandOTP: int?

### UserMachine
- Id: int
- AmountPerHour: double (Required)
- AmountPerAcre: double
- Distance: int
- userId: int (Display: User)
- user: User (FK)
- MachineId: int (Required)
- Machines: Machine (FK)

### Worker
- Id: int (Key)
- UserName: string (Required)
- UserPhoneNo: string (Required)