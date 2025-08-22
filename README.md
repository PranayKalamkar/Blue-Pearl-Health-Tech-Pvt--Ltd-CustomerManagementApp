# Blue-Pearl-Health-Tech-Pvt--Ltd-CustomerManagementApp
Customer Management App
Customer Management Web Application (ASP.NET MVC with MySQL)
Overview

This is an ASP.NET MVC web application for managing customers using a MySQL database and ADO.NET. The application includes:

User logs in with username and password.

The customer list displays after successful login.

Add new customers with the following details:

Customer Name

Phone Number

Date of Birth

Address

Send an email to a customer with file attachment support.

Features

Authentication using username and password.

Prevent duplicate customers (based on phone number or name).

Simple CRUD operations using ADO.NET with MySQL.

Email sending functionality with file attachment.

Technology Stack

Frontend: ASP.NET MVC 5

Backend: C#, ADO.NET

Database: MySQL

Email Service: SMTP

Project Structure

Controllers: Handles user login and customer management.

Models: Represent data structures (User, Customer).

Views: Razor views for UI.

Repositories: Data access logic using ADO.NET.

Utilities: Email service class.

Usage

Register or log in.

View the customer list.

Add a new customer.

Send an email with an optional attachment to a customer.

Repository Guidelines

Create feature branches for each change.

Commit with meaningful messages.

Follow coding standards and include comments.

Future Enhancements

Add JWT authentication.

Add role-based access control.

Add search and pagination to the customer list.

Unit and integration tests.
