
# IOMate Platform

IOMate is a platform for Robotic Process Automation (RPA), designed to automate repetitive business processes and tasks. The platform was built to be simple to use and highly scalable, making it suitable for organizations of any size. IOMate follows modern software engineering best practices, including the CQRS (Command Query Responsibility Segregation) pattern and Clean Architecture principles, ensuring maintainability, testability, and clear separation of concerns. It provides a modular and extensible architecture, enabling the creation, management, and orchestration of RPA bots and workflows.

## Project Structure
- `IOMate.Api/` — RESTful API layer
- `IOMate.Application/` — Application logic and use cases
- `IOMate.Domain/` — Domain models and business rules
- `IOMate.Infra/` — Infrastructure and data access
- `IOMate.Test/` — Automated tests
- `iomate.web/` — Web frontend

## Getting Started
1. **Clone the repository:**
   ```sh
   git clone https://github.com/iomateapp/IOMate.Api.git
   ```
2. **Install dependencies:**
   - .NET SDK (latest LTS recommended)
   - Any other dependencies as described in the documentation
3. **Build the solution:**
   ```sh
   dotnet build IOMate.Platform.sln
   ```
4. **Run the API:**
   ```sh
   dotnet run --project IOMate.Api/IOMate.Api.csproj
   ```
5. **Run tests:**
   ```sh
   dotnet test IOMate.Test/IOMate.Test.csproj
   ```

## Migration
Coming soon.

## Contributing
Contributions are welcome! If you want to propose a new feature, bugfix, or enhancement, please open an issue first using the appropriate template. This helps us discuss and refine the idea or problem before any code is submitted. For all contributions, follow the code style and add tests for new features or bug fixes. Pull requests should reference the related issue whenever possible.

## License
This project is licensed under the GNU General Public (GPL) License.
