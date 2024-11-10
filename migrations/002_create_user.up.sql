CREATE TYPE user_status as ENUM ('active', 'blocked', 'deleted');

CREATE TYPE user_role AS ENUM ('default', 'moderator', 'admin');

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    name TEXT NOT NULL,
    email TEXT DEFAULT NULL,
    status user_status NOT NULL DEFAULT 'active',
    role user_role NOT NULL DEFAULT 'default',
    about TEXT NOT NULL DEFAULT 'about');
