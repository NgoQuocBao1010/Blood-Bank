## Donors API

1. GET successful donors, expected JSON return:

    ```
    {
        _id (CMND),
        name (donor name),
        phone,
        address,
        dob : int -> string,
        gender,
        email,
        blood: {
            name,
            type,
        }
    }
    ```

## Participants API

1. POST import excel
    ```
    {
        eventId,
        participants: [{
            ...
        }, ]
    }
    ```

1. GET paticipants waiting for approved

    ```
    {
        _id (CMND),
        name (donor name),
        phone,
        address,
        dob : int -> string,
        gender,
        email,
        blood: {
            name,
            type,
        }
        transaction: {
            _id: ,
            blood: {
                name,
                type,
            },
            _event: {
                _id,
                name,
            },
            amount: 500,
            dateDonated,
        },
    }
    ```

## Events

1. GET events data
    ```
    {
        _id,
        name,
        location: {
            city,
            address,
        },
        startDate,
        duration,
        detail",
    }
    ```
