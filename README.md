# Blood-Bank

## Front end development environment's recommendations

```
    Node version >= v14.17.4
    NPM >= v6.14.14
```

### Admin Page installation guide

1. Clone repo
1. Install admin page dependencies

    ```bash
    npm run install-client
    ```

1. Start front-end's dev server

    ```bash
    npm run start-client
    ```

1. If everything is successfull, Go to http://localhost:3000/

### Landing Page installation guide

1. Clone repo
1. Install landing-page dependencies

    ```bash
    npm run install-lp
    ```

1. Start landing-page's dev server

    ```bash
    npm run start-lp
    ```

1. If everything is successfull, Go to http://localhost:3000/ **(make sure no service is run on the port 3000)**

&nbsp;

```markdown
    NOTES
    ⚠️ Please re-install all the dependencies every time you test.
```

&nbsp;
&nbsp;

## Server development environment's recommendations

```
    .NET v5.0
    MongoDB v5.0.5
```

### Server installation guide (⚠️ Remember to install .NET and Mongo first)

1. Clone repo

1. Change directory to /backend, create an .env file and enter your localhost MongoDB URL

    ```text
    MongoDB=<Your Mongo DB URL>
    ```

1. Start landing-page's dev server

    ```bash
    dotnet run
    ```

    If you get permission denied error, run:

    ```bash
    sudo dotnet run
    ```

1. Server will run on localhost, port 5000. Using Postman or any API testing tool to make a GET request to server endpoint: http://localhost:5000/Blood
