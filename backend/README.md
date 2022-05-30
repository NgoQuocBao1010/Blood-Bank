## Server installation guide

1. Clone repo

1. Change directory to /backend, create an .env file and enter your localhost MongoDB URL

    ```text
    MongoDB=<Your Mongo DB URL>
    ```

1. Start landing-page's dev server

    ```bash
    dotnet run
    ```

    If you get "Permission denied error", run:

    ```bash
    sudo dotnet run
    ```

1. Server will run on localhost, port 5000. Using Postman or any API testing tool to make a GET request to server endpoint: http://localhost:5000/Blood

1. [API documentation](./API_USAGE.md)
