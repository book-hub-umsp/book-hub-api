BEGIN;

CREATE TYPE user_role AS ENUM ('default', 'moderator');

ALTER TABLE public.users ADD COL

COMMIT;