# PaySkyTask

Overview
This repository contains the completed solution for a system with two user types (Employer and Applicant). The system allows for self-registration, login, CRUD operations for vacancies, application limits for vacancies, vacancy posting and deactivation, vacancy expiry dates, applicant listing for vacancies, vacancy searching, application with limits, and restrictions on multiple applications per day. The system also includes an archiving mechanism for expired vacancies, fully functional APIs,  and implementation using .NET Core 8 and MS SQL Server. It incorporates caching using InMemoryCache, logging using Serilog, and the Result pattern in a generic repository.

Prerequisites
.NET Core 8 SDK
MS SQL Server
Redis server
Git
Visual Studio Code or any other IDE
Setup Instructions
1. Clone the Repository

git clone https://github.com/Passant127/PaySkyTaskBackend.git
cd your-repo-name
2. Configure the Database
Create a new database in MS SQL Server.
Update the connection string in the appsettings.json file with your database details:

"ConnectionStrings": {
  "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;"
}



4. Configure Serilog
Update the Serilog settings in the appsettings.json file as needed for your logging requirements.

5. Apply Migrations
Open a terminal and navigate to the project directory. Run the following commands to apply database migrations:
Update-Database


6. Run Application




Features
Self-registration for Employers and Applicants
Login functionality
CRUD operations for vacancies
Application limits for vacancies
Vacancy posting and deactivation
Vacancy expiry dates
Applicant listing for vacancies
Vacancy searching
Application with limits
Restrictions on multiple applications per day
Archiving mechanism for expired vacancies
Fully functional APIs
Caching using InMemoryCache
Logging using Serilog
Pagination and Result pattern in the generic repository
Contributing
Fork the repository
Create a new branch (git checkout -b feature/your-feature-name)
Commit your changes (git commit -am 'Add some feature')
Push to the branch (git push origin feature/your-feature-name)
Create a new Pull Request







