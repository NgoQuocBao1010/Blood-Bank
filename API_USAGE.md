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
    
## User

1. POST user login -> Endpoint: api/user/login
    ```
    {
        "email": "string",
        "password": "string"
    }
    ```

2. GET all user -> Endpoint: api/user
    ```
    [
        {
            "_id": "string",
            "email": "string",
            "password": "string",
            "isAdmin": boolean,
            "hospital_id": "string"
        }
    ]
    ```

3. GET user by id -> Endpoint: api/user/{id}
    ```
    {
        "_id": "string",
        "email": "string",
        "password": "string",
        "isAdmin": boolean,
        "hospital_id": "string"
    }
    ```

4. POST create user -> Endpoint: api/user
    ```
    {
        "email": "string",
        "password": "string",
        "isAdmin": boolean,
        "hospital_id": "string"
    }
    ```

5. DELETE user by id -> Endpoint: api/user/{id}
    ```
    Return true if delete success
   ```

## Event Submission

1. GET all event submission -> Endpoint: api/eventSubmission
    ```
    [
        {
            "_id": "string",
            "eventId": "string",
            "idCardNumber": "string",
            "fullName": "string",
            "phone": "string",
            "email": "string",
            "address": "string",
            "gender": "string",
            "dob": "string",
            "latestDonationDate": "string"
        }
    ]
    ```

2. GET event submission by id -> Endpoint: api/eventSubmission/{id}
    ```
    {
        "_id": "string",
        "eventId": "string",
        "idCardNumber": "string",
        "fullName": "string",
        "phone": "string",
        "email": "string",
        "address": "string",
        "gender": "string",
        "dob": "string",
        "latestDonationDate": "string"
    }
    ```

3. POST create event submission -> Endpoint: api/eventSubmission
    ```
    {
        "eventId": "string",
        "idCardNumber": "string",
        "fullName": "string",
        "phone": "string",
        "email": "string",
        "address": "string",
        "gender": "string",
        "dob": "string",
        "latestDonationDate": "string"
    }
    ```
4. DELETE event submission by id -> Endpoint: api/eventSubmission/{id}
    ```
    Return true if delete success
    ```

## Hospital

1. GET all hospital -> Endpoint: api/hospital
    ```
    [
        {
            "_id": "string",
            "name": "string",
            "address": "string",
            "phone": "string"
        }
    ]
    ```

2. GET hospital by id -> Endpoint: api/hospital/{id}
    ```
    {
        "_id": "string",
        "name": "string",
        "address": "string",
        "phone": "string"
    }
    ```

3. POST create hospital -> Endpoint: api/hospital
    ```
    {
        "name": "string",
        "address": "string",
        "phone": "string"
    }
    ```

4. DELETE hospital by id -> Endpoint: api/hospital/{id}
    ```
    Return true if delete success
    ```