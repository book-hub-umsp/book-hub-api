BEGIN;

CREATE TYPE user_role AS ENUM ('default', 'moderator', 'admin');

ALTER TABLE public.users ADD COLUMN role user_role DEFAULT 'default';

COMMIT;