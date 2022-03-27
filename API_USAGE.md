## Donors API

1. GET list successful donors, expected JSON return:  -> Endpoint: api/Donor/success

    ```
    {
        _id (CMND),
        name (donor name),
        dob : int -> string,
        gender,
        address,
        phone,
        email,
        blood: {
            name,
            type,
        }
    }
    ```
    
2. GET data of a donor, expected JSON return:  -> Endpoint: api/Donor/{_id} (id of a donor)

    ```
    {
        _id (CMND),
        name (donor name),
        dob : int -> string,
        gender,
        address,
        phone,
        email,
        blood: {
            name,
            type,
        }
    }
    ```
    
## DonorTransaction API

1. GET list donation of a donor, expected JSON return:  -> Endpoint: api/DonorTransaction/listDonation/{_id}  (id of donor)

    ```
    {
        _id (CMND),
        name (donor name),
        dob : int -> string,
        gender,
        address,
        phone,
        email,
        blood: {
            name,
            type,
        }
    }
    ```

## Participants API

1. GET paticipants waiting for approved (Pending -> status: 0)     -> Endpoint: api/Donor

    ```
    [
        {
            _id (CMND),
            name (donor name),
            dob : int -> string,
            gender,
            address,
            phone,
            email,
            blood: {
                name,
                type,
            }
            transaction: {
                _id,
                blood: {
                    name,
                    type
                },
                eventDonated: {
                    _id,
                    name
                },
                dateDonated: string,
                amount: int,
                status: int,
                rejectReason,
                donorId
            }
        },
    ]
    ```

2. POST (import from excel) -> Endpoint: api/Donor
    ```
    {
        eventId: string,
        listParticipants: [
            {
                _id: string (CMND),
                name,
                dob: string,
                gender,
                address,
                phone: string,
                email,
                transaction: {
                    blood: {
                        name,
                        type
                    },
                    dateDonated: string,
                    amount: int
                }
            },
        ]
    }
    ```
    
3. PUT participants waiting for approved -> Endpoint: api/DonorTransaction/approve
    ```
    [
        {
            _id,
            eventId
        },
    ]
    ```

4. PUT participants waiting for rejected -> Endpoint: api/DonorTransaction/reject
    ```
    [
        {
            _id,
            eventId,
            rejectReason
        },
    ]
    ```


## Events

1. GET events data -> Endpoint: api/Event
    ```
    {
        _id,
        name,
        location: {
            city,
            address
        },
        startDate: string,
        duration: int,
        detail,
        participants (default: 0)
    }
    ```
2. POST events data -> Endpoint: api/Event
    ```
    {
        name,
        location: {
            city,
            address
        },
        startDate: string,
        duration: int,
        detail
    }
    ```
    
3. DELETE events data -> Endpoint: api/Event/{_id}


    
## Blood

1. GET blood data -> Endpoint: api/Blood
    ```
    {
        _id,
        name,
        type,
        quantity: int
    }
    ```
    
