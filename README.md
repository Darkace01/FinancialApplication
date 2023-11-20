# Financial Application

This is a .NET Core application that provides financial services. The application is structured into several projects including a main web application, data access layer, service layer, and a test project.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/download) or any code editor of your choice.
- Docker (Optional)

### Setup

1. Clone the repository to your local machine.
2. Open the solution file [`FinancialApplication.sln`](command:_github.copilot.openRelativePath?%5B%22FinancialApplication.sln%22%5D 'FinancialApplication.sln') in your code editor.
3. Restore the NuGet packages by running `dotnet restore` in the terminal.
4. Build the solution by running `dotnet build`.

### Running the Application

You can run the application by executing `dotnet run --project FinancialApplication/FinancialApplication.csproj` in the terminal.

If you're using Visual Studio Code, you can also use the provided launch configurations in [`.vscode/launch.json`](command:_github.copilot.openRelativePath?%5B%22.vscode%2Flaunch.json%22%5D '.vscode/launch.json') to run the application.

### Running with Docker

If you have Docker installed, you can build and run the application in a Docker container. The Dockerfile is located at [`FinancialApplication/Dockerfile`](command:_github.copilot.openRelativePath?%5B%22FinancialApplication%2FDockerfile%22%5D 'FinancialApplication/Dockerfile').

To build the Docker image, run:

```sh
docker build -t financialapplication -f FinancialApplication/Dockerfile .
```

To run the application in a Docker container, execute:

```sh
docker run -p 7249:7249 financialapplication
```

The application will be accessible at `http://localhost:7249`.

## Running the Tests

You can run the unit tests by executing `dotnet test FinancialApplication.Test/FinancialApplication.Test.csproj` in the terminal.

## Contributing

We welcome contributions from everyone. Before you start, please read our Code of Conduct. We expect all our contributors to adhere to it.

1. Fork the repository to your GitHub account.
2. Clone the forked repository to your local machine.
3. Create a new branch for your feature or bug fix.
4. Make your changes and commit them with a meaningful commit message.
5. Push your changes to the new branch on your forked repository.
6. Submit a pull request from the new branch on your forked repository to the main branch on the original repository.

Please make sure to update tests as appropriate when making changes.

## License

This project is licensed under the terms of the [`LICENSE.txt`](command:_github.copilot.openRelativePath?%5B%22LICENSE.txt%22%5D 'LICENSE.txt').

## Contact

If you have any questions, feel free to reach out to us.
