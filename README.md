# Forum Management System APIs

This project provides simple Forum Management System APIs that allow different users to interact with each other.

## Features

- **Interactions**: Users can interact with each other using Likes, Comments, and Replies.
- **Moderation**: Admins can assign different permissions to users, and Moderators can suspend users or delete posts.
- **Filtering**: Ability to search for posts by applying filters.

## Project Structure

The project follows a layered architecture and includes the following components:

- **Models**: For managing entities such as ApplicationUsers, Categories, Posts, Comments, and Replies.
- **Controllers**: Responsible for handling user requests such as posting, commenting, replying, liking, and moderation actions like assigning permissions and suspending users.
- **Services**: Contains business logic for handling operations.
- **Helpers**: Used for managing Automapper, and configuration classes for JWT and other requirements.
- **Filters**: Custom attributes for applying filters, such as the Banned Author attribute.
- **DTO (Data Transfer Objects)**: Handles the different models used for user requests.

## Database Tables

The system uses the following tables:

- **Categories**: Stores different category names.
- **Posts**: Stores post content and related data.
- **Comments**: Stores comment content and related data.
- **Replies**: Stores reply content and related data.
- **AspNetUsers**: Stores application user information and data.
- **AspNetRoles**: Stores roles and permission information.
- **UserLikedPosts**: Stores data about posts that users have liked.
- **UserLikedComments**: Stores data about comments that users have liked.
- **UserLikedReplies**: Stores data about replies that users have liked.

## Technologies Used

- **C#**
- **ASP.NET Core**
- **Entity Framework Core**

## Required Tools and Technologies

This project requires the following tools and technologies:

- **Visual Studio 2022**: The recommended Integrated Development Environment (IDE) for this project, providing full support for .NET 8.0 and additional tools like scaffolding and debugging.
- **.NET 8.0 SDK**: The project is built using the .NET 8.0 framework, which must be installed for development and running the application.
- **SQL Server**: A relational database system used to store the forum data, including user information, posts, and comments.

### NuGet Packages

- **AutoMapper.Extensions.Microsoft.DependencyInjection**: Used for object mapping, converting models to DTOs and vice versa.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: For managing user authentication via token-based security.
- **Microsoft.AspNetCore.Identity**: Handles user management and role-based access control.
- **Microsoft.EntityFrameworkCore**: Provides Object-Relational Mapping (ORM) to interact with the SQL Server database.
- **Swashbuckle.AspNetCore**: For generating interactive API documentation using Swagger.
