# EF Core PM Command For Migrations

## Add migrations

### Command

```
dotnet ef migrations add IntitalCreate --startup-project [API dir] --project [Infrastructure dir] --context [StoreContext or AppIndentityDbContext] --output-dir [Data/Migrations or Identity/Migrations]
```

## Update database
### Command
```
dotnet ef database update
```