# Dienynas - Student Grade Management System

## Overview
Dienynas is a WPF-based desktop application for managing student grades across different modules. It provides an intuitive interface for educational institutions to efficiently track and manage student academic performance.

## Key Features
- **Student Management**: Add, view, search, edit, and delete students
- **Module Management**: Create and manage course modules
- **Grade Management**: Record, edit, and delete student grades
- **Advanced Searching**: Quickly find students by name or partial matches
- **Smart Sorting**: Sort students by name, module grades, or overall average
- **Grade Analysis**: Automatically calculate and display student averages
- **User-Friendly Interface**: Intuitive design with keyboard shortcuts
- **Data Visualization**: Clear tabular presentation of student grades
- **Database Management**: Built-in tools for database maintenance

## Technical Details
- C# and WPF framework for responsive UI
- Clean architecture with separation of concerns
- MySQL database backend for reliable data storage
- Optimized data processing for large datasets

## Requirements
- .NET Framework 4.7.2 or higher
- MySQL Server 5.7 or higher
- Required NuGet packages:
  - MySql.Data
  - System.Configuration.ConfigurationManager
  - System.Diagnostics.DiagnosticSource

## Installation
1. Clone or download the repository
2. Open the solution file in Visual Studio
3. Restore NuGet packages
4. Set up the MySQL database (see Database Setup)
5. Build and run the application

## Database Setup
The application needs a MySQL database named "database" with the following structure:
- Students table: id (INT, PK), Vardas (VARCHAR), Pavarde (VARCHAR)
- Modules table: id (INT, PK), Modulis (VARCHAR)
- Grades table: id (INT, PK), StudentId (INT, FK), ModuleId (INT, FK), Pazymys (INT)

The application includes functionality to reset and create these tables automatically.

## Usage Tips
1. Press Ctrl+F to quickly access the search function
2. Use the top toolbar for common operations
3. Sort any column by clicking its header
4. Right-click for contextual options
5. Use the search bar for filtering students in real-time
6. Toggle between ascending/descending sort with the arrow buttons

## Recent Updates
- Added student sorting by various criteria
- Improved search functionality with real-time filtering
- Enhanced UI with better organization and keyboard shortcuts
- Optimized database operations for better performance
- Improved code organization with logical grouping and better readability

## Contributing
Contributions to improve Dienynas are welcome. Please fork the repository, create a feature branch, and submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
