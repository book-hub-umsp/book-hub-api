BEGIN;

CREATE TYPE user_status as ENUM ('active', 'blocked');

CREATE TYPE user_permission as ENUM ('none', 'moderation');

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    name TEXT NOT NULL,
    email BIGINT DEFAULT NULL,
    status user_status NOT NULL DEFAULT 'active',
	premission user_permission NOT NULL DEFAULT 'none',
    about TEXT NOT NULL DEFAULT 'about'
);


COMMIT;