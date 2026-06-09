# Pandora
## Models

### User
 - id:             UUID
 - username:       string
 - email:          string
 - passwordHash:   string
 - createdAt:      ISO 8601

### UserUser
 - userFollowerId: UUID
 - userFollowedId: UUID
 - createdAt:      ISO 8601
 
### Box
 - id:             UUID
 - title:          string
 - userId:         UUID
 - description:    string
 
### BoxBook
 - boxId:          UUID
 - bookId:         UUID
 - rating:         int
 - status:         enum { READING, REREADING, FINISHED, ABANDONED, PLANNED }
 - pagesRead:      int
 - beginDate:      ISO 8601
 - finishDate:     ISO 8601
 
### Book
 - id:             UUID
 - authorId:       UUID
 - title:          string
 - description:    string
 - ISBN:           string
 - pages:          int
 
### Author
 - id:             UUID
 - name:           string

## Constraints
### TABLE users
    - PK id
    - UNIQUE: username
    - UNIQUE: email

### TABLE user_user
    - PK (user_follower_id, user_followed_id)
    - FK user_follower_id
    - FK user_followed_id

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
    - FK author_id -> authors.id

### TABLE authors
    - PK id
    - UNIQUE: name
