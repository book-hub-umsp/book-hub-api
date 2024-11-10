BEGIN;

CREATE TYPE user_role AS ENUM ('default', 'moderator');

ALTER TABLE public.users ADD COLUMN role user_role DEFAULT 'default';

COMMIT;