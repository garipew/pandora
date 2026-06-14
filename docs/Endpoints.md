# Pandora
## Endpoints

METHOD PATH                                             : authorization
\*      \*                                              : admin

### /books

- [ ] GET    /books                                           : public
- [ ] POST   /books
- [ ] GET    /books/{bookId}                                  : public
- [ ] PUT    /books/{bookId}
- [ ] DELETE /books/{bookId}

### /authors

- [ ] GET    /authors                                         : public
- [ ] POST   /authors
- [ ] GET    /authors/{authorId}                              : public
- [ ] PUT    /authors/{authorId}
- [ ] DELETE /authors/{authorId}

### /users

- [x] GET    /users                                           : public
- [x] POST   /users                                           : public
- [x] GET    /users/{userId}                                  : public
- [ ] PUT    /users/{userId}                                  : owner
- [ ] DELETE /users/{userId}                                  : owner
- [x] POST   /users/assign

### /users/{userId}/boxes

- [ ] GET    /users/{userId}/boxes                            : public
- [x] POST   /users/{userId}/boxes                            : owner
- [ ] GET    /users/{userId}/boxes/{boxId}                    : public
- [ ] PUT    /users/{userId}/boxes/{boxId}                    : owner
- [ ] DELETE /users/{userId}/boxes/{boxId}                    : owner

### /users/{userId}/boxes/{boxId}/books

- [ ] GET    /users/{userId}/boxes/{boxId}/books              : public
- [ ] POST   /users/{userId}/boxes/{boxId}/books              : owner
- [ ] GET    /users/{userId}/boxes/{boxId}/books/{bookId}     : public
- [ ] PUT    /users/{userId}/boxes/{boxId}/books/{bookId}     : owner
- [ ] DELETE /users/{userId}/boxes/{boxId}/books/{bookId}     : owner

### /auth

- [x] POST /auth/login                                        : public
- [ ] POST /auth/logoff                                       : authenticated
