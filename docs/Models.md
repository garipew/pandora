# Pandora
## Models

### User

|      Field      |   Type   |
|-----------------|----------|
| id              | UUID     |
| username        | string   |
| email           | string   |
| passwordHash    | string   |
| createdAt       | ISO 8601 |
| role            | string   |

### UserUser

|      Field      |   Type   |
|-----------------|----------|
| userFollowerId  | UUID     |
| userFollowedId  | UUID     |
| createdAt       | ISO 8601 |
 
### UserBook

|      Field      |   Type                                                    |
|-----------------|-----------------------------------------------------------|
| userId          | UUID                                                      |
| bookId          | UUID                                                      |
| rating          | int                                                       |
| status          | enum { READING, REREADING, FINISHED, ABANDONED, PLANNED } |
| pagesRead       | int                                                       |
| beginDate       | ISO 8601                                                  |
| finishDate      | ISO 8601                                                  |

### Box

|      Field      |   Type   |
|-----------------|----------|
| id              | UUID     |
| userId          | UUID     |
| title           | string   |
| description     | string   |
 
### BoxBook

|      Field      |   Type   |
|-----------------|----------|
| boxId           | UUID     |
| bookId          | UUID     |

### Book

|      Field      |   Type   |
|-----------------|----------|
| id              | UUID     |
| title           | string   |
| description     | string   |
| ISBN            | string   |
| pages           | int      |

### AuthorBook

|      Field      |   Type   |
|-----------------|----------|
| authorId        | UUID     |
| bookId          | UUID     |
 
### Author
|      Field      |   Type   |
|-----------------|----------|
| id              | UUID     |
| name            | string   |

## Constraints
### TABLE users
    - PK id
    - UNIQUE: username
    - UNIQUE: email

### TABLE user_user
    - PK (user_follower_id, user_followed_id)
    - FK user_follower_id -> users.id
    - FK user_followed_id -> users.id

### TABLE user_book
    - PK (user_id, book_id)
    - FK user_id -> users.id
    - FK book_id -> books.id

### TABLE boxes
    - PK id
    - FK user_id -> users.id
    - UNIQUE: (user_id, title)

### TABLE box_book
    - PK (box_id, book_id)
    - FK box_id -> boxes.id
    - FK book_id -> books.id

### TABLE books
    - PK id
    - UNIQUE: isbn

### TABLE authors
    - PK id
    - UNIQUE: name

### TABLE author_book
    - PK (author_id, book_id)
    - FK author_id -> authors.id
    - FK book_id -> books.id
