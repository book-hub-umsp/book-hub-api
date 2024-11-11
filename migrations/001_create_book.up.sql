BEGIN;

CREATE TYPE book_status as ENUM('draft', 'published', 'hiden', 'removed');
 
CREATE TABLE genres (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    value TEXT NOT NULL,
	CONSTRAINT pk_genres PRIMARY KEY (id)
);

CREATE TABLE books (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    title TEXT NOT NULL,
    author_id BIGINT DEFAULT NULL,
    book_genre_id BIGINT NOT NULL DEFAULT 1,
    book_annotation TEXT NOT NULL,
    book_status book_status NOT NULL DEFAULT 'draft',
    creation_date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    last_edit_date timestamp with time zone NOT NULL DEFAULT NOW(),
    keywords_content JSON DEFAULT NULL,
    CONSTRAINT "pk_books" PRIMARY KEY (id)
);

ALTER TABLE books
    ADD CONSTRAINT "fk_books_genre"
        FOREIGN KEY (book_genre_id) 
        REFERENCES genres (id) ON DELETE SET DEFAULT;

ALTER TABLE books 
    ADD CONSTRAINT "fk_author_id" 
	FOREIGN KEY (author_id) 
	REFERENCES users (id);

INSERT INTO genres (value) 
VALUES 
    ('default'),
    ('epic_novel'),
    ('novel'),
    ('narrativa'),
    ('story'),
    ('essay'),
    ('parable'),
    ('lyrical_poem'),
    ('elegy'),
    ('epistle'),
    ('epigram'),
    ('ode'),
    ('sonnet'),
    ('comedy'),
    ('tradegy'),
    ('drama'),
    ('poem'),
    ('ballad');

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

COMMIT;
