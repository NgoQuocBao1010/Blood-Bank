## Donors API

1. GET successful donors, expected JSON return:

    ```json
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

1. GET paticipants waiting for approved

    ```json
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
    ```json
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
