#  SqlERDiagrammDrawer

A C# tool for creating, visualizing, importing and exporting SQL Entity-Relationship Diagrams.

SqlERDiagrammDrawer is a Windows Forms application written in .NET that allows developers to design database models visually and also import existing SQL schemas to automatically generate ER diagrams.

It is useful for both database design and reverse engineering of existing databases.
## 📹 Video

![ezgif-42eb9a76f804f4bc](https://github.com/user-attachments/assets/dd6fa570-7e8b-402e-8d6c-bc3bba91737b)

![ezgif-4a7a44c6f03d0b93](https://github.com/user-attachments/assets/c6b11428-ea90-4c71-ab88-2f1d7d8035ec)

![ezgif-454a24042b3a43e7](https://github.com/user-attachments/assets/6cc52b2d-47e3-45d5-bd29-bf40362d3e5d)

![ezgif-45b9deabea915f9a](https://github.com/user-attachments/assets/7230b047-4d26-4136-8e8c-a934c0272a9c)

![ezgif-47f939e35123db35](https://github.com/user-attachments/assets/e9fcaca0-652a-4671-beb8-c3ebd587ee18)

##  Overview

SqlERDiagrammDrawer is a lightweight visual tool for:

Creating relational database models

Visualizing tables and relationships

Importing existing SQL schemas

Generating SQL scripts from visual entities

Documenting and understanding complex databases

The application provides an interactive UI to manage entities, fields, primary keys and foreign keys.

##  Features

✔ Visual creation of ER diagrams
✔ Import of existing SQL database schemas
✔ Automatic visualization of tables and relationships
✔ Manual creation and editing of tables and fields
✔ SQL CREATE TABLE script generation
✔ Foreign key and relationship management
✔ Drag & drop entity editing
✔ SQL export for database migrations

## 🔁 SQL Import (Reverse Engineering)

SqlERDiagrammDrawer supports importing existing SQL scripts or schemas.

The tool can:

Load an existing SQL schema

Automatically detect:

Tables

Columns

Primary keys

Foreign keys

Generate a complete ER diagram from the imported database

This allows:

Reverse engineering of legacy databases

Visual documentation of existing systems

Faster understanding of complex data models

## 📤 SQL Export

You can also export the visual model back to SQL.

The tool generates:

CREATE TABLE statements

Primary key definitions

Foreign key constraints

This is useful for:

Database initialization

Migrations

Sharing schemas with other developers

## 🧩 Main Components

The project includes:

UI Controls

SQLErDiagramControl

SQLEntityControl

FieldControl

ConnectionLine

Database Logic

DatabaseController

SQLCommandBuilder

Data Models

SQLEntity

SQLField

Keys

These components work together to manage both the visual layer and the SQL generation/import logic.

🛠 Requirements

To build and run the project you need:

Microsoft Visual Studio (2019 / 2022 / 2025)

.NET Framework / .NET SDK

MySQL Server (optional, for testing)

## 🔌 MySQL Connector Requirement

This project uses MySqlConnector to communicate with MySQL databases.

You must install the following NuGet package:

MySqlConnector

Install via Visual Studio:

Right click on the project

Manage NuGet Packages

Search for MySqlConnector

Install the latest version

Or via Package Manager Console:
Install-Package MySqlConnector


Without this package, the project will not compile or connect to MySQL.

## 📦 Installation

Clone the repository:

git clone https://github.com/peppe960422/SqlERDiagrammDrawer.git


Open the solution in Visual Studio:

SqlERDiagrammDrawer.sln


Build the solution and run the application.

## 📌 Usage

Create new entities using the UI.

Add fields, primary keys and foreign keys.

Connect entities visually.

Import an existing SQL schema (optional).

Export the model to SQL.

## 🤝 Contributing

Contributions are welcome. Possible improvements:

Support for more database engines (PostgreSQL, SQL Server, SQLite)

UML export

Schema validation

Better import parser
