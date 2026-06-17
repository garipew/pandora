# Pandora
## Endpoints

METHOD PATH                                                   : authorization
\*      \*                                                    : admin

### /books

- [ ] GET    /books                                           : public
- [ ] POST   /books
- [ ] GET    /books/{bookId}                                  : public
- [ ] PUT    /books/{bookId}
- [ ] DELETE /books/{bookId}

### /authors

- [x] GET    /authors                                         : public
- [x] POST   /authors
- [x] GET    /authors/{authorId}                              : public
- [x] PUT    /authors/{authorId}
- [x] DELETE /authors/{authorId}

### /users

- [x] GET    /users                                           : public
- [x] POST   /users                                           : public
- [x] GET    /users/{userId}                                  : public
- [x] PUT    /users/{userId}                                  : owner
- [x] DELETE /users/{userId}                                  : owner
- [x] POST   /users/assign

### /users/{userId}/followers

- [ ] GET    /users/{userId}/followers                        : public
- [ ] DELETE /users/{userId}/followers/{followId}             : owner

### /users/{userId}/following

- [ ] GET    /users/{userId}/following                        : public
- [ ] POST   /users/{userId}/following                        : owner
- [ ] DELETE /users/{userId}/following/{followId}             : owner

### /users/{userId}/boxes

- [x] GET    /users/{userId}/boxes                            : public
- [x] POST   /users/{userId}/boxes                            : owner
- [x] GET    /users/{userId}/boxes/{boxId}                    : public
- [x] PUT    /users/{userId}/boxes/{boxId}                    : owner
- [x] DELETE /users/{userId}/boxes/{boxId}                    : owner

### /users/{userId}/boxes/{boxId}/books

- [ ] GET    /users/{userId}/boxes/{boxId}/books              : public
- [ ] POST   /users/{userId}/boxes/{boxId}/books              : owner
- [ ] GET    /users/{userId}/boxes/{boxId}/books/{bookId}     : public
- [ ] PUT    /users/{userId}/boxes/{boxId}/books/{bookId}     : owner
- [ ] DELETE /users/{userId}/boxes/{boxId}/books/{bookId}     : owner

### /auth

- [x] POST /auth/login                                        : public
