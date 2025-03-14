BEGIN;

ALTER TABLE IF EXISTS books DROP CONSTRAINT IF EXISTS "fk_books_genre_id";
ALTER TABLE IF EXISTS books DROP CONSTRAINT IF EXISTS "fk_books_author_id";

ALTER TABLE IF EXISTS favorites DROP CONSTRAINT IF EXISTS "fk_favorites_user_id";
ALTER TABLE IF EXISTS favorites DROP CONSTRAINT IF EXISTS "fk_favorites_book_id";

ALTER TABLE IF EXISTS keywords_books_links DROP CONSTRAINT IF EXISTS "fk_keywords_books_links_keyword_id";
ALTER TABLE IF EXISTS keywords_books_links DROP CONSTRAINT IF EXISTS "fk_keywords_books_links_book_id";

DROP TABLE IF EXISTS "favorites";
DROP TABLE IF EXISTS "keywords_books_links";
DROP TABLE IF EXISTS "books";
DROP TABLE IF EXISTS "genres";
DROP TABLE IF EXISTS "partitions";

DROP TYPE IF EXISTS "book_status";

DROP FUNCTION IF EXISTS "update_change_books";

COMMIT;