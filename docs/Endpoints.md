# Pandora
## Endpoints

METHOD PATH                                             : authorization
- \*    \*                                              : admin

### /books

1. GET    /books                                           : public
2. POST   /books
3. GET    /books/{bookId}                                  : public
4. PUT    /books/{bookId}
5. DELETE /books/{bookId}

### /authors

1. GET    /authors                                         : public
2. POST   /authors
3. GET    /authors/{authorId}                              : public
4. PUT    /authors/{authorId}
5. DELETE /authors/{authorId}

### /users

1. GET    /users                                           : public
2. POST   /users                                           : public
3. GET    /users/{userId}                                  : public
4. PUT    /users/{userId}                                  : owner
5. DELETE /users/{userId}                                  : owner

### /users/{userId}/boxes

1. GET    /users/{userId}/boxes                            : public
2. POST   /users/{userId}/boxes                            : owner
3. GET    /users/{userId}/boxes/{boxId}                    : public
4. PUT    /users/{userId}/boxes/{boxId}                    : owner
5. DELETE /users/{userId}/boxes/{boxId}                    : owner

### /users/{userId}/boxes/{boxId}/books

1. GET    /users/{userId}/boxes/{boxId}/books              : public
2. POST   /users/{userId}/boxes/{boxId}/books              : owner
3. GET    /users/{userId}/boxes/{boxId}/books/{bookId}     : public
4. PUT    /users/{userId}/boxes/{boxId}/books/{bookId}     : owner
5. DELETE /users/{userId}/boxes/{boxId}/books/{bookId}     : owner

### /auth

1. POST /auth/login                                        : public
2. POST /auth/logoff                                       : authenticated
