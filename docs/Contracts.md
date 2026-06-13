# Pandora
## Contracts

### Response body skeleton
```json
	{
		"data": {...}
	}
	// Or
	{
		"error": {
			"code": string,
			"message": string,
			"details": ?[]
		}
	}
```

### POST /users

#### Request

```json
	{
		"username": string,
		"password": string,
		"email":    string
	}
```

#### Response

201:
```json
	{
		"data": {
			"username":  string,
			"email":     string,
			"createdAt": ISO 8601,
			"id":        UUID
		}
	}
```

400:
```json
	{
		"error": {
			"code": "BAD_REQUEST",
			"message": "failed to create user",
			"details": [
                    {
                        "field": "email",
                        "message": "invalid email"
                    },
                    {
                        "field": "username",
                        "message": "username too short"
                    }
            ]
		}
	}
```

409:
```json
	{
		"error": {
			"code": "EMAIL_EXISTS",
			"message": "email already used by another account"
		}
	}
```

### POST /auth/login

#### Request

```json
	{
		"emailOrUsername": string,
		"password":        string
	}
```

#### Response

200:
```json
	{
		"data": {
            "token":    JWT,
			"id":       UUID,
			"username": string
		}
	}
```

401:
```json
	{
		"error": {
			"code": "FAILED_LOGIN",
			"message": "username or password incorrect"
		}
	}
```

### POST   /users/{userId}/boxes

#### Request Header
```json
    Authorization: Bearer <jwt>
```
#### Request Body
```json
    {
        "title":       string,
        "description": ?string
    }
```

#### Response
201:
```json
	{
		"data": {
			"id":          UUID,
			"title":       string,
            "description": ?string
		}
	}
```

401:
```json
	{
		"error": {
			"code": "UNAUTHORIZED",
			"message": "missing or invalid token"
		}
	}
```

403:
```json
	{
		"error": {
			"code": "FORBIDDEN",
			"message": "you do not have permission to access this resource"
		}
	}
```

409:
```json
	{
		"error": {
			"code": "BOX_EXISTS",
			"message": "box already exists"
		}
	}
```
