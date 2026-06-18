# Pandora
## Contracts

The API produces responses to clients requests in this general shape:

```json
	{
		"data": {...}
	}
```

Errors have their own general shape:
```json
	{
		"error": {
			"code": string,
			"message": string,
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
