# SQLite Query

## Get Users with Roles and Claims

### Query

```sql
-- Script Date: 28/02/2025 3:59 PM  - ErikEJ.SqlCeScripting version 3.5.2.95
SELECT 
    u.Id AS UserId,
    u.UserName,
    r.Name AS RoleName,
    COALESCE(
        (SELECT GROUP_CONCAT(rc.ClaimType, ', ') 
         FROM (SELECT DISTINCT ClaimType, RoleId FROM AspNetRoleClaims) rc
         WHERE rc.RoleId = r.Id), 
    '') AS RoleClaims,
    COALESCE(
        (SELECT GROUP_CONCAT(uc.ClaimType, ', ') 
         FROM (SELECT DISTINCT ClaimType, UserId FROM AspNetUserClaims) uc
         WHERE uc.UserId = u.Id), 
    '') AS UserClaims,
    t.Name AS TokenName,
    t.Value As TokenValue
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
join AspNetUserTokens t ON t.UserId = u.Id
ORDER BY u.Id;
```
