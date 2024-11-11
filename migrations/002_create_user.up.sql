BEGIN;

CREATE TYPE user_status as ENUM ('active', 'blocked', 'deleted');

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    name TEXT NOT NULL,
    email TEXT DEFAULT NULL,
    status user_status NOT NULL DEFAULT 'active',
    about TEXT NOT NULL DEFAULT 'about'
);

COMMIT;
