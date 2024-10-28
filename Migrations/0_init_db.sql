CREATE DATABASE books_hub IF NOT EXISTS;

CREATE TYPE book_genre as ENUM (
    'epic_novel', 'novel', 'narrativa', 'story', 'essay', 'parable', 'lyrical_poem', 'elegy', 'epistle', 'epigram', 'ode', 'sonnet', 'comedy', 'tradegy', 'drama', 'poem', 'ballad');

CREATE TYPE book_status as ENUM ('draft', 'published', 'hiden', 'removed');

CREATE TABLE books (
    id bigint GENERATED ALWAYS AS IDENTITY,
    title text NOT NULL,
    author_id bigint NOT NULL DEFAULT 0,
    book_genre book_genre NOT NULL,
    book_annotation text NOT NULL,
    book_status book_status NOT NULL,
    creation_date timestamp with time zone NOT NULL,
    last_edit_date timestamp with time zone NOT NULL,
    keywords_content json DEFAULT NULL,
    CONSTRAINT pk_books PRIMARY KEY (id)
);

CREATE FUNCTION update_change_books()
RETURNS trigger
LANGUAGE plpgsql
AS $function$
    BEGIN
        NEW.last_edit_date = NOW();
        RETURN NEW;
    END
$function$;
CREATE TRIGGER tr_update_change_books
    BEFORE UPDATE ON books
FOR EACH ROW EXECUTE PROCEDURE 
    update_change_books();
