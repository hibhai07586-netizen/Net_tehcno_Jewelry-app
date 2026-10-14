# Jewels App - Team Development Guide

## Project Overview
A .NET 10 WinForms application for jewelry management system with multiple modules.

## Technology Stack
- **.NET 10** with WinForms
- **SQL Server** for database
- **GitHub** for version control

## Team Members & Assignments
- **Team Lead**: [Name] - Dashboard & Navigation
- **Dev 2**: [Name] - Customer Module
- **Dev 3**: [Name] - Product/Supplier Module
- **Dev 4**: [Name] - Customer Order Module
- **Dev 5**: [Name] - Report Module
- **Dev 6**: [Name] - Settings & Database

## Getting Started

### 1. Clone Repository
```bash
git clone <repo-url>
cd Jewels-app
```

### 2. Setup Local Environment
```bash
dotnet restore
dotnet build
```

### 3. Create Your Feature Branch
```bash
git checkout -b feature/your-module-name
```

### 4. Development Workflow
- Edit your assigned module
- Test locally: `dotnet build`
- Commit often: `git commit -m "feat: description"`
- Push daily: `git push origin feature/your-module-name`
- Create Pull Request for review

## Database Setup
Update `DatabaseConfig.cs` with your SQL Server connection string:
```csharp
"Server=YOUR_SERVER;Database=JewelsDB;User Id=sa;Password=YOUR_PASSWORD;"
```

## File Structure
```
Jewels-app/
├── dashboard.cs              (Main Form - DO NOT edit simultaneously)
├── customer.cs               (Customer Module)
├── supplier.cs               (Product Module)
├── customerOrder.cs          (Order Module)
├── report.cs                 (Report Module)
├── setting.cs                (Settings Module)
└── DatabaseConfig.cs         (Shared Database Configuration)
```

## Naming Conventions
- **Branches**: `feature/module-name` or `fix/bug-name`
- **Commits**: `feat: description` or `fix: description`
- **Classes**: `PascalCase` (e.g., `CustomerForm`, `ProductService`)
- **Methods**: `PascalCase` (e.g., `GetCustomers()`, `SaveProduct()`)
- **Variables**: `camelCase` (e.g., `customerName`, `productId`)

## Code Review Checklist
Before creating a Pull Request:
- [ ] Code builds successfully (`dotnet build`)
- [ ] No merge conflicts
- [ ] Follows naming conventions
- [ ] Comments added for complex logic
- [ ] Tested on local machine
- [ ] No changes to `dashboard.cs` (unless coordinated)

## Common Git Commands

```bash
# Update your branch with latest changes
git fetch origin
git rebase origin/develop

# Resolve merge conflicts
git status  # See conflicts
# Edit conflicted files
git add .
git commit -m "resolve: merge conflicts"

# View branch differences
git diff develop

# Stash changes temporarily
git stash
git pull origin develop
git stash pop
```

## Communication
- **Daily**: Git commits & pushes
- **Weekly**: Team standup
- **Issues**: Use GitHub Issues for bugs & discussion

## Questions?
Contact the team lead or raise a GitHub Issue.

---
Last Updated: [Date]
