# Pandora
## Endpoints

METHOD PATH                                                   : authorization
\*      \*                                                    : admin

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

### /users/{userId}/books

- [ ] GET    /users/{userId}/books                            : public
- [ ] POST   /users/{userId}/books                            : owner
- [ ] GET    /users/{userId}/books/{bookId}                   : public
- [ ] PUT    /users/{userId}/books/{bookId}                   : owner
- [ ] DELETE /users/{userId}/books/{bookId}                   : owner

### /users/{userId}/boxes

- [x] GET    /users/{userId}/boxes                            : public
- [x] POST   /users/{userId}/boxes                            : owner
- [x] GET    /users/{userId}/boxes/{boxId}                    : public
- [x] PUT    /users/{userId}/boxes/{boxId}                    : owner
- [x] DELETE /users/{userId}/boxes/{boxId}                    : owner

### /books

- [x] GET    /books                                           : public
- [x] POST   /books
- [x] GET    /books/{bookId}                                  : public
- [x] PUT    /books/{bookId}
- [x] DELETE /books/{bookId}

### /authors

- [x] GET    /authors                                         : public
- [x] GET    /authors/{authorId}                              : public
- [x] DELETE /authors/{authorId}

### /auth

- [x] POST /auth/login                                        : public
