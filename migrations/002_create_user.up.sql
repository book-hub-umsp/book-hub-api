BEGIN;

CREATE TYPE user_status as ENUM ('active', 'blocked', 'deleted');

CREATE TYPE user_role AS ENUM ('default', 'moderator', 'admin');

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    name TEXT NOT NULL,
    email TEXT DEFAULT NULL,
    status user_status NOT NULL DEFAULT 'active',
    about TEXT NOT NULL DEFAULT 'about',
    CONSTRAINT "pk_users" PRIMARY KEY (id)
);

ALTER TABLE books 
    ADD CONSTRAINT "fk_author_id" 
	FOREIGN KEY (author_id) 
	REFERENCES users (id);

COMMIT;
