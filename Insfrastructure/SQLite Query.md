# SQLite Query

## Get Users with Roles and Claims

### Query

```sql
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
    '') AS UserClaims
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY u.Id;
```
