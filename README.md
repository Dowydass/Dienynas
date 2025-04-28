# Dienynas - Student Grade Management System

## Overview
Dienynas is a desktop application for managing student grades across different modules. Built with WPF in C#, it provides an intuitive interface for educational institutions to track and manage student academic performance.

## Features
- Manage students (add, view, search)
- Manage modules (add, view)
- Record and edit student grades
- Search for students by name
- Delete students from modules
- Calculate average grades automatically
- Database management with MySQL

## Requirements
- .NET Framework 4.7.2
- MySQL Server
- Required packages:
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
The application requires a MySQL database named "database" with the following structure:
- Students table: id (INT, PK), Vardas (VARCHAR), Pavarde (VARCHAR)
- Modules table: id (INT, PK), Modulis (VARCHAR)
- Grades table: id (INT, PK), StudentId (INT, FK), ModuleId (INT, FK), Pazymys (INT)

The application includes functionality to reset the database and create these tables automatically.

## Usage
1. Launch the application
2. Use the buttons in the interface to navigate between different functions:
   - Add students or modules
   - Search for students
   - Edit grades
   - Delete students from modules
3. The main grid displays student names and their grades across all modules

## Contributing
If you'd like to contribute to this project, please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
