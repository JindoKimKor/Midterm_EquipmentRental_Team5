# DTO Usage Analysis Report - Equipment Rental System

**Analysis Date**: November 24, 2025  
**Status**: Critical Issues Found  
**Priority**: URGENT - Security & Design Issues

---

## EXECUTIVE SUMMARY

This C# backend and Vue.js frontend application has **critical DTO usage issues** that expose security vulnerabilities and violate architectural best practices:

1. **SECURITY CRITICAL**: Customer entity with password field is returned in API responses
2. **ARCHITECTURAL VIOLATION**: 20+ API endpoints return raw domain entities instead of DTOs
3. **INCONSISTENT DESIGN**: Mix of DTO and non-DTO responses across controllers
4. **FRONTEND MISMATCH**: Path mismatch in rental API endpoint

---

## FINDINGS

### 1. CRITICAL SECURITY ISSUE - Password Exposure

**Location**: `Midterm_EquipmentRental_Team5/Presentation/Controllers/CustomerController.cs`

The Customer domain entity (which contains a Password field) is returned directly in API responses:

| Endpoint | Issue | Risk |
|----------|-------|------|
| GET /api/customer | Returns `IEnumerable<ICustomer>` with Password field | High |
| GET /api/customer/{id} | Returns `ICustomer` with Password field | High |
| POST /api/customer | Accepts full Customer entity | Medium |
| PUT /api/customer/{id} | Accepts full Customer entity | Medium |

**Impact**: Frontend and network traffic will contain customer password hashes in plain responses.

**Fix Required**: Create `CustomerListDto` and `CustomerDetailDto` without Password field (IMMEDIATE)

---

### 2. Domain Entity Exposure - All Controllers

**Summary Table**:
```
AuthController         Returns Customer entity in login response           CRITICAL
EquipmentController    0/7 endpoints have response DTOs                   HIGH
RentalController       0/7 endpoints have response DTOs                   HIGH  
CustomerController     0/7 endpoints have response DTOs (+ PASSWORD)      CRITICAL
ChatController         Returns raw Chat/Message entities                  MEDIUM
```

---

### 3. Detailed Issues by Controller

#### **AuthController** - `/Presentation/Controllers/AuthController.cs`

| Line | Method | Issue | Severity |
|------|--------|-------|----------|
| 55 | Login | Returns full Customer object in response | CRITICAL |
| 98-103 | Me | Returns custom anonymous object (inconsistent) | MEDIUM |

**Example Issue**:
```csharp
return Ok(new { message = "Login successful", user, token });
// 'user' is full Customer entity with Password field!
```

---

#### **EquipmentController** - `/Presentation/Controllers/EquipmentController.cs`

| Line | Method | Current Return Type | Issue | Severity |
|------|--------|---------------------|-------|----------|
| 19 | GetAllEquipment | `ActionResult<IEnumerable<IEquipment>>` | Returns raw entities | HIGH |
| 38 | GetEquipmentRentalHistory | `ActionResult<IEnumerable<IRental>>` | Returns raw entities | HIGH |
| 67 | GetEquipment | `ActionResult<IEquipment>` | Returns raw entity | HIGH |
| 87 | AddEquipment | Accepts `Equipment` entity | No input DTO | MEDIUM |
| 103 | UpdateEquipment | Accepts `Equipment` entity | No input DTO | MEDIUM |
| 134 | GetAvailableEquipment | `ActionResult<IEnumerable<IEquipment>>` | Returns raw entities | HIGH |
| 153 | GetRentedEquipmentSummary | `ActionResult<IEquipment>` | Returns raw entity | HIGH |

---

#### **RentalController** - `/Presentation/Controllers/RentalController.cs`

| Line | Method | Current Return Type | Has DTO? | Issue | Severity |
|------|--------|---------------------|----------|-------|----------|
| 24 | GetAllRentals | `ActionResult<IEnumerable<IRental>>` | No | Raw entity | HIGH |
| 54 | GetRentalDetails | `ActionResult<IRental>` | No | Raw entity | HIGH |
| 89 | IssueEquipment | Uses IssueRequest | YES | Good! | - |
| 123 | ReturnEquipment | Uses ReturnRequest | YES | Good! | - |
| 164 | GetActiveRentals | `ActionResult<IEnumerable<IRental>>` | No | Raw entity | HIGH |
| 194 | GetCompletedRentals | `ActionResult<IEnumerable<IRental>>` | No | Raw entity | HIGH |
| 225 | GetOverdueRentals | `ActionResult<IEnumerable<IRental>>` | No | Raw entity | HIGH |
| 244 | GetEquipmentRentalHistory | `ActionResult<IEnumerable<IRental>>` | No | Raw entity | HIGH |
| 264 | ExtendRental | Uses ExtensionRequest | YES | Good! | - |

---

#### **CustomerController** - `/Presentation/Controllers/CustomerController.cs`

| Line | Method | Current Return Type | Issue | Severity |
|------|--------|---------------------|-------|----------|
| 21 | GetAllCustomers | `ActionResult<IEnumerable<ICustomer>>` | No DTO + Password exposed | CRITICAL |
| 40 | GetUnactiveCustomers | `ActionResult<IEnumerable<ICustomer>>` | No DTO + Password exposed | CRITICAL |
| 60 | GetCustomer | `ActionResult<ICustomer>` | No DTO + Password exposed | CRITICAL |
| 88 | CreateCustomer | Accepts `Customer` entity | No input DTO | MEDIUM |
| 103 | UpdateCustomer | Accepts `Customer` entity | No input DTO | MEDIUM |
| 149 | GetCustomerRentalHistory | `ActionResult<IEnumerable<IRental>>` | Returns raw entities | HIGH |
| 174 | GetCustomerActiveRentals | `ActionResult<IEnumerable<IRental>>` | Returns raw entities | HIGH |

---

#### **ChatController** - `/Presentation/Controllers/ChatController.cs`

| Line | Method | Issue | Severity |
|------|--------|-------|----------|
| 25 | GetUserChats | Returns raw Chat entities with nested Customer objects | MEDIUM |
| 39 | GetChatHistory | Returns raw Message entities | MEDIUM |

---

### 4. Frontend Communication Issues

**File**: `/Midterm_EquipmentRental_Team5_FrontEnd/src/api/RentalController.js`

**Line 69 - Path Mismatch**:
```javascript
// WRONG - Frontend calls
const res = await RequestHandler.get(`rental/equipment/${equipmentId}/history`)

// CORRECT - Backend endpoint
[HttpGet("{id}/rental-history")]  // In EquipmentController!
```

**Expected Endpoint**: `equipment/{equipmentId}/rental-history`  
**Frontend Calls**: `rental/equipment/{equipmentId}/history`

This causes a 404 error in the frontend.

---

### 5. Existing DTOs (Incomplete List)

**Location**: `/Application/DTOs/`

Currently only **4 DTOs exist** (all request DTOs):
- LoginRequest.cs
- IssueRequest.cs
- ReturnRequest.cs
- ExtensionRequest.cs

**Missing**: 20+ Response DTOs for list/detail views

---

## MISSING DTOs - REQUIREMENTS

### Response DTOs Needed

#### Equipment (3 DTOs):
- `EquipmentListDto` - For GET /api/equipment
- `EquipmentDetailDto` - For GET /api/equipment/{id}
- `AvailableEquipmentDto` - For GET /api/equipment/available

#### Rental (5 DTOs):
- `RentalListDto` - For GET /api/rental
- `RentalDetailDto` - For GET /api/rental/{id}
- `ActiveRentalDto` - For GET /api/rental/active
- `CompletedRentalDto` - For GET /api/rental/completed
- `OverdueRentalDto` - For GET /api/rental/overdue

#### Customer (3 DTOs):
- `CustomerListDto` - For GET /api/customer (WITHOUT PASSWORD)
- `CustomerDetailDto` - For GET /api/customer/{id} (WITHOUT PASSWORD)
- `InactiveCustomerDto` - For GET /api/customer/unactive-customer

#### Auth (3 DTOs):
- `LoginResponseDto` - For POST /api/auth/login
- `UserBasicDto` - Shared DTO for user references
- `UserClaimsDto` - For GET /api/auth/me

#### Chat/Message (3 DTOs):
- `ChatListDto` - For GET /api/chat
- `ChatHistoryDto` - For GET /api/chat/{chatId}
- `MessageDto` - For message responses

#### Shared (3 DTOs):
- `UserBasicDto` - User reference in other DTOs
- `EquipmentBasicDto` - Equipment reference in rental DTOs
- `CustomerBasicDto` - Customer reference in other DTOs

**Total DTOs Needed**: ~29 (currently have 4)

---

## DETAILED RECOMMENDATIONS

### Priority 1 - IMMEDIATE (This Week)

1. **Create CustomerListDto & CustomerDetailDto**
   - Remove Password field
   - Affects: CustomerController.cs lines 21, 40, 60
   - Files to create:
     - `/Application/DTOs/Response/Customer/CustomerListDto.cs`
     - `/Application/DTOs/Response/Customer/CustomerDetailDto.cs`

2. **Create LoginResponseDto & UserBasicDto**
   - Prevent password exposure in auth response
   - Affects: AuthController.cs line 55
   - Files to create:
     - `/Application/DTOs/Response/Auth/LoginResponseDto.cs`
     - `/Application/DTOs/Response/Auth/UserBasicDto.cs`

3. **Fix Frontend Path Mismatch**
   - RentalController.js line 69
   - Change from: `rental/equipment/{equipmentId}/history`
   - Change to: `equipment/{equipmentId}/rental-history`

### Priority 2 - SOON (Next 2 Weeks)

4. **Create Equipment Response DTOs**
   - EquipmentListDto, EquipmentDetailDto, AvailableEquipmentDto
   - Refactor EquipmentController to use them
   - Affects: EquipmentController.cs lines 19, 38, 67, 134, 153

5. **Create Rental Response DTOs**
   - RentalListDto, RentalDetailDto, ActiveRentalDto, etc.
   - Refactor RentalController to use them
   - Affects: RentalController.cs lines 24, 54, 164, 194, 225, 244

6. **Create Chat Response DTOs**
   - MessageDto, ChatListDto, ChatHistoryDto
   - Refactor ChatController to use them
   - Affects: ChatController.cs lines 25, 39

### Priority 3 - LATER (Next Month)

7. **Reorganize DTO Structure**
   ```
   Application/DTOs/
   ├── Request/
   │   ├── Equipment/
   │   ├── Rental/
   │   ├── Customer/
   │   ├── Auth/
   │   └── Chat/
   ├── Response/
   │   ├── Equipment/
   │   ├── Rental/
   │   ├── Customer/
   │   ├── Auth/
   │   └── Chat/
   └── Shared/
   ```

8. **Add AutoMapper for Cleaner Mapping**
   - Option: Use extension methods as shown in code examples
   - Option: Use AutoMapper NuGet package

9. **Create TypeScript Interfaces for Frontend**
   - Match Response DTO structures
   - Enable type checking on frontend API calls

10. **Update Frontend API Clients**
    - Update all API controller files to handle new DTOs
    - Update stores (ChatStore.js, Authentication.js, etc.)
    - Update Vue components using API data

---

## ENDPOINT-BY-ENDPOINT FIXES

### EquipmentController Summary

| Endpoint | From | To | Status |
|----------|------|----|----|
| GET /api/equipment | `IEnumerable<IEquipment>` | `EquipmentListDto[]` | Recommended |
| GET /api/equipment/{id} | `IEquipment` | `EquipmentDetailDto` | Recommended |
| POST /api/equipment | Accepts `Equipment` | Accepts `CreateEquipmentRequest` | Recommended |
| PUT /api/equipment/{id} | Accepts `Equipment` | Accepts `UpdateEquipmentRequest` | Recommended |
| DELETE /api/equipment/{id} | NoContent | NoContent | OK |
| GET /api/equipment/available | `IEnumerable<IEquipment>` | `AvailableEquipmentDto[]` | Recommended |
| GET /api/equipment/rented | `IEquipment` | `RentedEquipmentDto[]` | Recommended |
| GET /api/equipment/{id}/rental-history | `IEnumerable<IRental>` | `RentalHistoryDto[]` | Recommended |

---

### RentalController Summary

| Endpoint | From | To | Status |
|----------|------|----|----|
| GET /api/rental | `IEnumerable<IRental>` | `RentalListDto[]` | Recommended |
| GET /api/rental/{id} | `IRental` | `RentalDetailDto` | Recommended |
| POST /api/rental/issue | IssueRequest | IssueRequest | OK |
| POST /api/rental/return | ReturnRequest | ReturnRequest | OK |
| GET /api/rental/active | `IEnumerable<IRental>` | `ActiveRentalDto[]` | Recommended |
| GET /api/rental/completed | `IEnumerable<IRental>` | `CompletedRentalDto[]` | Recommended |
| GET /api/rental/overdue | `IEnumerable<IRental>` | `OverdueRentalDto[]` | Recommended |
| GET /api/rental/equipment/{id} | `IEnumerable<IRental>` | `RentalHistoryDto[]` | Recommended |
| PUT /api/rental/{id} | ExtensionRequest | ExtensionRequest | OK |
| DELETE /api/rental/{id} | NoContent | NoContent | OK |

---

### CustomerController Summary

| Endpoint | From | To | Status | **CRITICAL** |
|----------|------|----|----|---------|
| GET /api/customer | `IEnumerable<ICustomer>` | `CustomerListDto[]` | Recommended | Remove Password |
| GET /api/customer/{id} | `ICustomer` | `CustomerDetailDto` | Recommended | Remove Password |
| POST /api/customer | Accepts `Customer` | Accepts `CreateCustomerRequest` | Recommended | - |
| PUT /api/customer/{id} | Accepts `Customer` | Accepts `UpdateCustomerRequest` | Recommended | - |
| DELETE /api/customer/{id} | NoContent | NoContent | OK | - |
| GET /api/customer/unactive-customer | `IEnumerable<ICustomer>` | `InactiveCustomerDto[]` | Recommended | Remove Password |
| GET /api/customer/{id}/rentals | `IEnumerable<IRental>` | `RentalHistoryDto[]` | Recommended | - |
| GET /api/customer/{id}/active-rental | `IEnumerable<IRental>` | `ActiveRentalDto[]` | Recommended | - |

---

## CODE EXAMPLES PROVIDED

Detailed code examples are available in `code_examples.md` showing:

1. Security fix for Customer DTOs
2. Equipment response DTO implementation
3. Rental response DTO implementation
4. Auth response DTO implementation
5. Chat response DTO implementation
6. Frontend path fixes
7. TypeScript interface examples
8. Optional mapping helper class

---

## IMPLEMENTATION CHECKLIST

### Backend Tasks
- [ ] Create CustomerListDto.cs (URGENT)
- [ ] Create CustomerDetailDto.cs (URGENT)
- [ ] Create LoginResponseDto.cs (URGENT)
- [ ] Create UserBasicDto.cs (URGENT)
- [ ] Create EquipmentListDto.cs
- [ ] Create EquipmentDetailDto.cs
- [ ] Create AvailableEquipmentDto.cs
- [ ] Create RentalListDto.cs
- [ ] Create RentalDetailDto.cs
- [ ] Create ActiveRentalDto.cs
- [ ] Create MessageDto.cs
- [ ] Create ChatListDto.cs
- [ ] Refactor AuthController (line 55)
- [ ] Refactor EquipmentController
- [ ] Refactor RentalController
- [ ] Refactor CustomerController (URGENT)
- [ ] Refactor ChatController
- [ ] Test all endpoints
- [ ] Update API documentation

### Frontend Tasks
- [ ] Fix RentalController.js line 69
- [ ] Create TypeScript interfaces for DTOs
- [ ] Update EquipmentController.js API calls
- [ ] Update RentalController.js API calls
- [ ] Update CustomerController.js API calls
- [ ] Update ChatController.js API calls
- [ ] Update ChatStore.js
- [ ] Update components using API data
- [ ] Test all API integrations

---

## FILE PATHS FOR REFERENCE

### Controllers to Modify
```
/Midterm_EquipmentRental_Team5/Presentation/Controllers/
  ├── AuthController.cs (line 55)
  ├── EquipmentController.cs (lines 19,38,67,87,103,134,153)
  ├── RentalController.cs (lines 24,54,164,194,225,244)
  ├── CustomerController.cs (lines 21,40,60,88,103,149,174) [CRITICAL]
  └── ChatController.cs (lines 25,39)
```

### DTOs Location
```
/Application/DTOs/
├── Current: LoginRequest.cs, IssueRequest.cs, ReturnRequest.cs, ExtensionRequest.cs
├── Create: Response/ subdirectory structure
└── Create: Request/ subdirectory for better organization
```

### Frontend Files to Update
```
/Midterm_EquipmentRental_Team5_FrontEnd/src/
├── api/
│   ├── EquipmentController.js
│   ├── RentalController.js (line 69 - PATH FIX)
│   ├── CustomerController.js
│   └── ChatController.js
└── stores/
    ├── ChatStore.js
    └── Authentication.js
```

---

## IMPACT ASSESSMENT

### Security Impact
- **Critical**: Password fields exposed in Customer API responses
- **High**: Login response exposes full Customer entity
- **Medium**: Other domain entities expose internal structure

### Performance Impact
- **Neutral**: DTOs may slightly reduce payload size (fewer fields)
- **Positive**: Cleaner API contracts improve caching

### Development Impact
- **Effort**: Medium - approximately 40-60 hours for full refactor
- **Testing**: Critical - all endpoints must be retested
- **Documentation**: Needed for new DTO structures

### User Impact
- **None** if changes are done correctly (same functionality)
- **Positive**: Better API security
- **Positive**: More predictable API contracts

---

## ARCHITECTURE RECOMMENDATIONS

### Current (Violated)
```
Presentation (Controllers)
        ↓
  Returns domain entities directly
        ↓
  Frontend receives raw database models
```

### Recommended (Clean Architecture)
```
Presentation (Controllers)
        ↓
  Uses DTOs for serialization
        ↓
Application Services
        ↓
  Works with domain entities internally
        ↓
Database
```

---

## QUICK SUMMARY

| Finding | Count | Severity |
|---------|-------|----------|
| Missing Response DTOs | 20+ | HIGH |
| Password Exposure Vulnerabilities | 4+ endpoints | CRITICAL |
| Inconsistent DTO Usage | All controllers | HIGH |
| Frontend Path Mismatches | 1 | MEDIUM |
| No Input DTO Validation | 4+ endpoints | MEDIUM |

---

## NEXT STEPS

1. **Week 1**: Create and deploy Customer DTOs (SECURITY FIX)
2. **Week 2**: Fix RentalController.js path mismatch
3. **Week 3-4**: Create Equipment and Rental DTOs
4. **Week 5-6**: Complete refactoring and testing
5. **Week 7**: Frontend integration and deployment

---

**Report Generated**: November 24, 2025  
**Analyst**: Claude Code Analysis Tool  
**Recommendations**: HIGH PRIORITY - Begin with security fixes immediately
