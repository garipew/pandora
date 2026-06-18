# Pandora
## Models

### User

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| id              | UUID     | PK          |
| username        | string   | Unique      |
| email           | string   | Unique      |
| passwordHash    | string   |             |
| createdAt       | ISO 8601 |             |
| role            | string   |             |

### UserUser

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| userFollowerId  | UUID     | PK,FK       |
| userFollowedId  | UUID     | PK,FK       |
| createdAt       | ISO 8601 |             |
 
### UserBook

|      Field      |   Type                                                    | Constraints |
|-----------------|-----------------------------------------------------------|-------------|
| userId          | UUID                                                      | PK,FK       |
| bookId          | UUID                                                      | PK,FK       |
| rating          | int                                                       |             |
| status          | enum { READING, REREADING, FINISHED, ABANDONED, PLANNED } |             |
| pagesRead       | int                                                       |             |
| beginDate       | ISO 8601                                                  |             |
| finishDate      | ISO 8601                                                  |             |

### Box

|      Field      |   Type   | Constraints  |
|-----------------|----------|--------------|
| id              | UUID     | PK           |
| userId          | UUID     | FK           |
| title           | string   | UNIQUE on id |
| description     | string   |              |
 
### BoxBook

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| boxId           | UUID     | PK,FK       |
| bookId          | UUID     | PK,FK       |

### Book

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| id              | UUID     | PK          |
| title           | string   |             |
| description     | string   |             |
| ISBN            | string   | UNIQUE      |
| pages           | int      |             |

### AuthorBook

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| authorId        | UUID     | PK,FK       |
| bookId          | UUID     | PK,FK       |
 
### Author

|      Field      |   Type   | Constraints |
|-----------------|----------|-------------|
| id              | UUID     | PK          |
| name            | string   | UNIQUE      |
