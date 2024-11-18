BEGIN;

CREATE TYPE user_status as ENUM ('active', 'blocked', 'deleted');
CREATE TYPE claim_type as ENUM ('none', 'moderate_comments', 'moderate_reviews', 'create_topics', 'change_book_visibility', 'manage_user_actions', 'manage_user_accounts', 'change_user_role', 'change_role_claims');

CREATE TABLE roles (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    name TEXT NOT NULL,
    claims claim_type[] NOT NULL DEFAULT '{"none"}',
    CONSTRAINT "pk_roles" PRIMARY KEY (id)
);

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    role_id BIGINT NOT NULL DEFAULT 1,
    name TEXT NOT NULL,
    email TEXT DEFAULT NULL,
    status user_status NOT NULL DEFAULT 'active',
    about TEXT NOT NULL DEFAULT 'about',
    CONSTRAINT 
        "pk_users" PRIMARY KEY (id),
    CONSTRAINT "fk_users_role_id" 
        FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE SET DEFAULT
);

INSERT INTO roles (name, claims) VALUES ('default_user', '{"none"}');
INSERT INTO roles (name, claims) VALUES ('moderator', '{"moderate_comments", "moderate_reviews", "create_topics", "change_book_visibility"}');
INSERT INTO roles (name, claims) VALUES ('admin', '{"manage_user_actions", "manage_user_accounts", "change_user_role", "change_role_claims"}');

COMMIT;