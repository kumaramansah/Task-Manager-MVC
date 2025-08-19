# 📋 Task Manager

A robust web-based task management application built with ASP.NET Core 8.0, featuring user authentication, role-based access control, and comprehensive task management capabilities.

## 🖼️ Project Screenshots
> - Home page with welcome interface
> - <img width="1350" height="905" alt="Screenshot 2025-08-20 012859" src="https://github.com/user-attachments/assets/4562cb0a-715a-4d76-b97c-2e4629a8cd64" />
> - Login And Register Page
> - <img width="1355" height="560" alt="Screenshot 2025-08-20 015011" src="https://github.com/user-attachments/assets/0bdaeeb1-3f2e-4f3b-91fc-29b74e363fb4" />
> - <img width="1354" height="676" alt="Screenshot 2025-08-20 015018" src="https://github.com/user-attachments/assets/c1f46e64-c5f7-4522-8720-c334bed98714" />
> - Admin panel
> - <img width="1367" height="889" alt="Screenshot 2025-08-20 012749" src="https://github.com/user-attachments/assets/49747065-9d29-4e44-b50b-b77fd37607b7" />
> - Task management dashboard
> - <img width="1349" height="574" alt="Screenshot 2025-08-20 013841" src="https://github.com/user-attachments/assets/0da97b02-3844-4f4a-b3cb-0f4672b80001" />
> - <img width="1310" height="907" alt="Screenshot 2025-08-20 013906" src="https://github.com/user-attachments/assets/61253a16-4547-4c6a-8d90-755859fae49c" />
> - <img width="1349" height="401" alt="Screenshot 2025-08-20 013934" src="https://github.com/user-attachments/assets/aae2392f-10d5-4121-994d-d45566b3d767" />
> - <img width="1366" height="614" alt="Screenshot 2025-08-20 015054" src="https://github.com/user-attachments/assets/df6ad018-2221-4de8-83c9-cd0423c74b58" />
> - User panel
> - <img width="1314" height="903" alt="Screenshot 2025-08-20 014020" src="https://github.com/user-attachments/assets/adc89fae-4fc5-4fd7-b7fe-eb7e34a9bc48" />
> - <img width="1337" height="478" alt="Screenshot 2025-08-20 014345" src="https://github.com/user-attachments/assets/1a3e1040-0a18-46b1-b108-311a6ab4e29f" />
> - <img width="1321" height="519" alt="Screenshot 2025-08-20 014358" src="https://github.com/user-attachments/assets/afc51da1-7118-41b8-aab9-303a31cbaade" />
---

## ✨ Features

### 🔐 Authentication & Authorization
- **User Registration & Login**: Secure user authentication system with ASP.NET Core Identity
- **Role-Based Access Control**: Admin and regular user roles with different permissions
- **Identity Management**: Built on ASP.NET Core Identity with custom ApplicationUser model

### 📝 Task Management
- **Create Tasks**: Add new tasks with title, description, and due date validation
- **Task Assignment**: Admin users can assign tasks to specific team members
- **Status Tracking**: Monitor task progress through three states (Pending, In Progress, Completed)
- **Due Date Management**: Set and track task deadlines with validation
- **Task CRUD Operations**: Full Create, Read, Update, Delete functionality for tasks

### 👥 User Management
- **User Profiles**: Manage user information including full names
- **Admin Dashboard**: Comprehensive overview of users and tasks
- **User Editing**: Modify user details and manage permissions
- **User List Management**: View and manage all registered users

### 🎨 User Interface
- **Responsive Design**: Bootstrap 5.3.3 based modern UI
- **Intuitive Navigation**: Clean navigation with role-based menu items
- **Form Validation**: Client and server-side validation with custom error messages
- **Bootstrap Icons**: Professional iconography throughout the interface

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core 9.0
- **Authentication**: ASP.NET Core Identity with custom user model
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5.3.3
- **ORM**: Entity Framework Core with code-first approach
- **Development**: Visual Studio 2022 / VS Code compatible
- **Validation**: Data annotations and custom validation attributes

## 📋 Prerequisites

Before running this application, ensure you have:

- **.NET 8.0 SDK** or later
- **SQL Server** (LocalDB, Express, or Developer Edition)
- **Visual Studio 2022** or **VS Code** with C# extensions
- **Git** for version control

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/kumaramansah/task-manager.git
cd task-manager
```

### 2. Database Setup
1. **Update Connection String**: Modify `appsettings.json` with your SQL Server connection details
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=taskDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. **Run Migrations**: Open Package Manager Console and run:
   ```bash
   Update-Database
   ```

### 3. Build and Run
```bash
dotnet restore
dotnet build
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## 🔑 Default Admin Account

The application automatically creates an admin user on first run:

- **Email**: `admin@gmail.com`
- **Password**: `Admin@123`
- **Role**: Admin

⚠️ **Important**: Change these credentials in production!

## 📁 Project Structure

```
Task Manager/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs      # Authentication & user management
│   ├── AdminController.cs        # Admin dashboard & user oversight
│   ├── HomeController.cs         # Home page & error handling
│   └── TasksController.cs       # Task CRUD operations
├── Models/              # Data Models
│   ├── AppDbContext.cs          # Entity Framework context
│   ├── ApplicationUser.cs       # Custom user model (extends Identity)
│   ├── TaskItem.cs              # Task entity with validation
│   └── ErrorViewModel.cs        # Error page model
├── Views/               # Razor Views
│   ├── Account/        # Authentication views (Login/Register)
│   ├── Admin/          # Admin dashboard views
│   ├── Tasks/          # Task management views
│   ├── Home/           # Home page view
│   └── Shared/         # Layout and common views
├── ViewModels/          # View Models for data binding
│   ├── AssignTaskViewModel.cs   # Task assignment form
│   ├── AdminDashboardViewModel.cs # Admin dashboard data
│   ├── EditUserViewModel.cs     # User editing form
│   ├── LoginViewModel.cs        # Login form
│   └── RegisterViewModel.cs     # Registration form
├── Migrations/          # Entity Framework migrations
└── wwwroot/            # Static files (CSS, JS, images)
    ├── css/            # Custom stylesheets
    ├── js/             # JavaScript files
    └── lib/            # Third-party libraries (Bootstrap, jQuery)
```

## 🎯 Key Models

### TaskItem
- **Id**: Unique identifier (auto-generated)
- **Title**: Task name (max 100 characters, required)
- **Description**: Task details (max 500 characters, required)
- **DueDate**: Task deadline with custom validation (cannot be in past for new tasks)
- **Status**: Task progress enum (Pending/InProgress/Completed)
- **UserId**: Assigned user (required)
- **CreatedAt**: Creation timestamp (auto-generated)

### ApplicationUser
- Extends ASP.NET Core Identity
- **FullName**: User's complete name (custom property)
- **Email**: User's email address (from Identity)
- **Roles**: User permissions and access levels

## 🔧 Configuration

### Environment Variables
- **ConnectionStrings**: Database connection string
- **Logging**: Application logging levels
- **AllowedHosts**: Host configuration for deployment

### Customization
- Modify `Program.cs` to change admin credentials
- Update `appsettings.json` for environment-specific settings
- Customize validation rules in model classes
- Modify Bootstrap theme in `_Layout.cshtml`

## 🚀 Deployment

### Local Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
dotnet run --environment Production
```
## 🧪 Testing & Validation

The application includes comprehensive validation and error handling:

- **Model Validation**: Data annotations with custom validation logic
- **Form Validation**: Client-side and server-side validation
- **Custom Validation**: Due date validation preventing past dates for new tasks
- **Error Handling**: Standard ASP.NET Core error pages
- **Input Sanitization**: String length limits and required field validation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues:

1. Check the [Issues](https://github.com/kumaramansah/task-manager/issues) page
2. Create a new issue with detailed information
3. Include error messages and steps to reproduce

## 🔮 Future Enhancements

- [ ] Email notifications for task deadlines
- [ ] File attachments for tasks
- [ ] Task categories and tags
- [ ] Reporting and analytics dashboard
- [ ] Mobile-responsive PWA
- [ ] API endpoints for external integrations
- [ ] Real-time updates with SignalR
- [ ] Task priority levels
- [ ] Bulk task operations
- [ ] Export functionality (PDF, Excel)

## 📊 Project Status

- ✅ **Core Functionality**: Complete
- ✅ **User Authentication**: Complete
- ✅ **Task Management**: Complete
- ✅ **Admin Dashboard**: Complete
- ✅ **Database Design**: Complete
- ✅ **Form Validation**: Complete
- ✅ **Role-Based Access**: Complete
- 🔄 **Testing**: In Progress
- 🔄 **Documentation**: In Progress

## 🎨 UI/UX Features

- **Responsive Design**: Works on all device sizes
- **Modern Interface**: Clean, professional appearance
- **Intuitive Navigation**: Easy-to-use menu system
- **Consistent Styling**: Bootstrap-based design system
- **Accessibility**: Proper semantic HTML and ARIA labels

---

**Built with ❤️ using ASP.NET Core 8.0**

*Last updated: December 2024*
