# /users

This file presents the documentation for the API contracts on the `/users` endpoint.

For the contracts on `/users/{userId}/boxes` check [Boxes.md](Boxes.md).

The following types are shared across actions on this document:

**UserResponseData**

```json
{
    "id":        int,
    "email":     string,
    "username":  string,
    "createdAt": ISO 8601
}
```

**UserResponse**

```json
{
    "data": UserResponseData
}
```

**UserPublicResponseData**

```json
{
    "username":  string,
    "createdAt": ISO 8601
}
```

**UserPublicResponse**

```json
{
    "data": UserPublicResponseData
}
```

---

## Create

New users can be created via `POST /users` where the request body contains the following:

```json
{
    "email":    string,
    "username": string,
    "password": string
}
```

This action have two possible outcomes, Success or Conflict. On success, it produces `UserResponse` and the response code is `HTTP 201`.

The Conflict (`HTTP 409`) result packs two types of conflicts in one, username conflict and email conflict. The product of the first is:

```json
{
    "error": {
        "code": "USERNAME_EXISTS",
        "message": "username already in use by another account"
    }
}
```

And for the second:

```json
{
    "error": {
        "code": "EMAIL_EXISTS",
        "message": "email already in use by another account"
    }
}
```

## Read

To get information on a specific user, use `GET /users/{userId}`. This action produces `UserPublicResponse` on success and the response code is `HTTP 200`. On failure, it produces `USER_NOT_FOUND` described below and the response code is `HTTP 404`.

```json
{
    "error": {
        "code": "USER_NOT_FOUND",
        "message": "user [{userId}] do not exist"
    }
}
```

To get the information of **all** users, use `GET /users`. This action produces a list of `UserPublicResponse`. The response body has the following shape:

```json
{
    "data": [
        UserPublicResponse,
        UserPublicResponse,
        ...
        UserPublicResponse
    ]
}
```

Response code is `HTTP 200`.


## Update

The action `PUT /users/{userId}` is the responsible to update existing users. *Only the user* can update itself and the user can *only update itself*.

The request body should contain the following:

```json
{
    "email":    string,
    "username": string,
    "password": string
}
```

This action produces the following errors: `UNAUTHORIZED` (`HTTP 401`), `FORBIDDEN` (`HTTP 403`) and `USER_NOT_FOUND` (`HTTP 404`).

The `USER_NOT_FOUND` error was detailed on the previous section.

Both `UNAUTHORIZED` and `FORBIDDEN` are common to every action protected by authentication and are defined on [Auth.md](Auth.md).

On success, it produces `UserResponse` and response code is `HTTP 200`.

## Delete

To delete users, the action is `DELETE /users/{userId}`. Users can only be deleted by themselves or by an `Admin`.

For details on `Admin` and `Roles` check [Auth.md](Auth.md).

On success, it produces `UserResponse` (`HTTP 200`).

This action produces the following errors: `UNAUTHORIZED` (`HTTP 401`), `FORBIDDEN` (`HTTP 403`) and `USER_NOT_FOUND` (`HTTP 404`).
