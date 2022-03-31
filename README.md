# Blood-Bank

## Project environment's recommendations

```
    Node version >= v14.17.4
    NPM >= v6.14.14
    .NET v5.0
    MongoDB v5.0.5
```

```markdown
    NOTES
    ⚠️ Please make sure if MongoDB and .NET is installed.
    ⚠️ Please re-install all the dependencies every time you test.
```

### Project Testing command

1. Clone repo

1. Change directory to /backend, create an .env file and enter your localhost MongoDB URL

    ```text
    MongoDB=<Your Mongo DB URL>
    ```

1. Change directory back to the root folder

    ```bash
    cd ..
    ```

1. Start server

    ```bash
    npm run start-sv
    ```

1. Install all front-end dependencies

    ```bash
    npm run install-all
    ```

1. Test for Landing Pages (go to the URL shown in the terminal)

    ```bash
    npm run start-lp

    ```

1. Test for Admin Pages (go to the URL shown in the terminal)

    ```bash
    npm run start-admin
    ```

1. Server will run on localhost, port 5000. Make sure server is run successfully by using Postman or any API testing tool to make a GET request to server endpoint: http://localhost:5000/api to see all the available endpoints.

## Independent testings:

1. Test only [Landing pages](./landing-page/README.md)
1. Test only [Admin pages](./clients/README.md)
1. Test only [Server](./backend/README.md)
