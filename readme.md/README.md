# Financial Portfolio Management System

Group Members: Samuel Pham, Cole Peterson, Justin Anthony, Andres Montoya

## Project Overview

### Problem Statement

Investors and financial advisors have difficulty tracking and managing multiple portfolios when using manual tools like Excel spreadsheets. These methods are time-consuming, prone to error, and offer little visibility into performance trends across individual or group portfolios.

### Proposed Solution

A C#-based Financial Portfolio Management System that allows investors to create, manage, monitor, and compare portfolios. 

### Key Features

* Group and individual investors
* Create, view, update, and delete portfolios
* Add, edit, and remove assets within portfolios
* Track / visualize portfolio performance in certain scenarios

### Assumptions & Constraints

* Data is fake and not reflective of real market conditions
* Limited to basic asset types (stocks, ETFs)
* No real-time market data integration
* Built for the purpose of demonstrating OOP programming concepts

### Technologies Used

* Language: C#
* Frameworks: .NET 9.0
* Packages: AvaloniaUI
* IDE: Visual Studio 2026 & JetBrains Rider 

## Build & Run Instructions

1. Ensure you have the .NET SDK installed
    * Open a terminal and run `dotnet --version`
    * Download: [Microsoft .NET site](https://dotnet.microsoft.com/en-us/download)
2. Clone and Navigate
    *  `git clone https://github.com/ColePeterson05/Financial_Portfolio_Manager.git`
    
    * `cd Financial_Portfolio_Manager`
3. Prepare Data Files
    * Ensure /docs contains:
        * portfolios.txt
        * stocks.txt
        * etfs.txt
4. Build & Run
    * `dotnet build`
    * `dotnot run`
    * OR press green button in Visual Studio or JetBrains Rider

## OOP Features

| OOP Feature                        | File Name(s)                                 | Line Number(s) | Reasoning / Purpose                                                                                                                                         |
|------------------------------------|----------------------------------------------|----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Inheritance #1                     | PortfolioItem.cs                             | 3-25           | Stock & Etf inherits from PortfolioItem to share common attributes and behavior.                                                                            |
| Inheritance #2                     | Portfolio.cs                                 | 3-31           | IndividualPortfolio & GroupPortfolio inherits from Portfolio to share common attributes and behavior.                                                       |
| Interface Implementation #1        | IPortfolioRepository.cs & IUserRepository.cs | 3-9            | Defines contracts for storing, retrieving, and managing User and Portfolio data.                                                                            |
| Interface Implementation #2        | IDataLoader.cs                               | 3-10           | Defines a contract for loading and parsing data methods that can be implemented by multiple classes.                                                        |
| Interface Implementation #3        | IUser.cs & IGroupManager.cs                  | 3-9 & 3-7      | Defines contract for users and ensures that classes only implement the methods they actually need.                                                          |
| Polymorphism #1                    | Portfolio.cs                                 | 7              | Enables PortfolioItem list to store and manage many asset types (Stocks and ETFs) uniformly.                                                                |
| Polymorphism #2                    | PortfolioRepository.cs                       | 11             | Enables Portfolio list to store and manage many asset types (IndividualPortfolio and GroupPortfolio) uniformly.                                             |
| Struct                             | Quote.cs                                     | All            | Represents a "snapshot" of the portfolio item at a specific time.                                                                                           |
| Enum                               | PortfolioType.cs                             | All            | Represents the type of portfolio (individual or group).                                                                                                     |
| Data Structure (Nested Dictionary) | TxtDataLoader.cs                             | 61-63          | Nested Dictionary for parsing data in txt files and providing O(1) lookup for data loading.                                                                 |
| I/O                                | Program.cs                                   | All            | Provides input / output for application using the console.                                                                                                  |
| Access Modifiers                   | All                                          | All            | Private: Prevents external classes from directly modifying data \| Public: Exposes safe / controlled methods or fields that other classes can interact with |

## Design Patterns

| Pattern Name | Category   | File Name(s)                               | Line Number(s) | Rationale                                                                                                                                                                                                                         |
|--------------|------------|--------------------------------------------|----------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Singleton    | Creational | PortfolioRepository.cs & UserRepository.cs | 19-33          | Solves the problem of data inconsistency if multiple objects tried to manage the same data source. This ensures that only one shared instance of the data source manages the application's state.                                 |
| Factory      | Creational | PortfolioFactory.cs                        | All            | Addresses the complexity of instantiating different portfolio types which require different initialization rules and parameters. This encapsulates object creation for multiple portfolio types, centralizing construction logic. |
| Strategy     | Behavioral | IValuationStrategy.cs                      | All            | Solves the problem of hardcoding a single calculation method. This allows for interchangeable valuation algorithms at runtime while keeping the processor code unchanged.                                                         |

## Design Decisions

### High-Level Architecture

The system is designed using a layered architecture which separates concerns between data, logic, and presentation layers.

1. Presentation Layer: Handles user interactions and displays portfolio data. This layer relies on services to perform actions.
2. Service Layer: Contains business logic for managing portfolios, users, and assets. It interacts with repositories to fetch and store data.
3. Data Access Layer: Manages data persistence. It uses repositories to abstract the underlying data storage mechanism.

### Key Abstractions

* IUser vs IGroupManager: We separated "Group Manager" capabilities from the "Standard" capabilities to ensure regular users aren't forced to implement methods they cannot use.
* IValuationStrategy: This allows for different valuation methods to be implemented and swapped easily at runtime without changing the core portfolio logic.
* PortfolioFactory: Centralizes the creation logic for different portfolio types, making it easier to manage and extend later.
* Repositories: Repositories are restricted to a sintgle instance to ensure data consistency across the application.

### Trade-offs

1. Authentication
    * Decision: Moved session management into AuthenticationService
    * Benefit: Follows SRP as User class focuses solely on user data.
    * Cost: Must inject IAuthenticationService into classes needing session info.
2. Nested Dictionary
    * Decision: Used nested dictionary for data loading.
    * Benefit: O(1) lookup time for loading data and parsing.
    * Cost: Higher complexity in initial implementation.
3. Inheritance vs. Composition
    * Decision: Used inheritance for users rather than composition for roles.
    * Benefit: Clear demonstration of ISP.
    * Cost: Less flexibility in role management compared to composition approach.