# EF Core PM Command For Migrations

## Add migrations

### Command


```v1
dotnet ef migrations add IntitalCreate --startup-project [API dir] --project [Infrastructure dir] --context [StoreContext or AppIndentityDbContext] --output-dir [Data/Migrations or Identity/Migrations]
```

```v2
dotnet ef migrations add AddressAdded --context StoreContext --startup-project API --project  Insfrastructure
```

## Update database
### Command
```
dotnet ef database update
```