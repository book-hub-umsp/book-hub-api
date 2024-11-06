BEGIN;

ALTER TABLE books
DROP CONSTRAINT (SELECT constraint_name
FROM information_schema.table_constraints
WHERE table_name = 'books' AND constraint_type = 'FOREIGN KEY');

ALTER TABLE books
ADD CONSTRAINT fk_books_genre
FOREIGN KEY (book_genre_id)
REFERENCES genres (id) ON DELETE SET DEFAULT 1;

CREATE FUNCTION remove_books_genre()
RETURNS trigger
LANGUAGE plpgsql
AS $function$
    BEGIN
    UPDATE books
    SET last_edit_date = NOW()
    WHERE book_genre_id = OLD.id;

    RETURN OLD;
    END
$function$;

CREATE TRIGGER tr_remove_books_genre
    AFTER DELETE ON genres
FOR EACH ROW EXECUTE PROCEDURE 
    remove_books_genre();

COMMIT;