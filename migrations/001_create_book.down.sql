ALTER TABLE IF EXISTS books DROP CONSTRAINT IF EXISTS "fk_books_genre";

DROP TYPE IF EXISTS "book_status";

DROP TABLE IF EXISTS "books";
DROP TABLE IF EXISTS "genres";

DROP FUNCTION IF EXISTS "update_change_books";
